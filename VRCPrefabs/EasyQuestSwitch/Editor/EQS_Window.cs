using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using EasyQuestSwitch.Fields;
using System.IO;

namespace EasyQuestSwitch
{

    public class EQS_Window : EditorWindow
    {

        [MenuItem("VRC Prefabs/Easy Quest Switch")]
        static void Init()
        {
            EQS_Window window = (EQS_Window)EditorWindow.GetWindow(typeof(EQS_Window), false, "Easy Quest Switch");
            window.Show();
        }

        private SerializedObject serializedObject;
        private SerializedProperty eqsData;
        private ReorderableList reorderableList;
        private EQS_Localization localization;

        private EQS_Data _data;
        private EQS_Data data
        {
            get
            {
                if(_data == null) _data = GameObject.Find("EQS_DATA")?.GetComponent<EQS_Data>();
                return _data;
            }
            set 
            {
                _data = value;
            }
        }

        private Vector2 scrollPos;

        private Texture2D logo;
        private Texture2D headerBG;

        private bool settingsMenu;
        private int chosenLanguage;

        private void OnEnable()
        {
            serializedObject = null;
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            Undo.undoRedoPerformed += OnUndoRedo;

            minSize = new Vector2(512, 320);

            localization = new EQS_Localization();
            localization.LoadLanguages();
            chosenLanguage = EditorPrefs.GetInt("EQS_Language", 0);
            localization.SetLanguage(chosenLanguage);

            if (data != null) SetupEQS();

            logo = (Texture2D)Resources.Load("EQS_Logo", typeof(Texture2D));
            headerBG = new Texture2D(1, 1);
            headerBG.SetPixel(0, 0, new Color(0, 0.8f, 0.3f));
            headerBG.Apply();
        }

        private void CreateEQS()
        {
            GameObject sceneRefGO = new GameObject("EQS_DATA");
            EQS_Data sceneRef = sceneRefGO.AddComponent<EQS_Data>();
            sceneRefGO.tag = "EditorOnly";
            sceneRefGO.hideFlags = HideFlags.HideInHierarchy;
            data = sceneRef;
            SetupEQS();
        }

        private void SetupEQS()
        {
            serializedObject = new SerializedObject(data);
            eqsData = serializedObject.FindProperty("Objects");

            reorderableList = new ReorderableList(serializedObject, eqsData, true, false, false, false);
            reorderableList.drawElementCallback += DrawElementCallback;
            reorderableList.elementHeightCallback += ElementHeightCallback;
            reorderableList.drawElementBackgroundCallback += DrawElementBackgroundCallback;
            reorderableList.onAddCallback += OnAddCallback;
            reorderableList.onRemoveCallback += OnRemoveCallback;
            reorderableList.onMouseDragCallback += OnMouseDragCallback;
            reorderableList.showDefaultBackground = false;
            reorderableList.headerHeight = 0;
        }

        private void DestroyEQS()
        {   
            serializedObject = null;
            DestroyImmediate(data.gameObject);
            _data = data = null;
        }

        private void OnUndoRedo() => Repaint();

        private void OnSceneChanged(Scene current, Scene next) => OnEnable();

        #region ReorderableList callbacks

        private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.width = EditorGUIUtility.currentViewWidth - GUI.skin.verticalScrollbar.fixedWidth - 32;
            float oldY = rect.y;
            rect.y += EditorGUIUtility.standardVerticalSpacing * 2;

            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty elementTarget = element.FindPropertyRelative("Target");
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width + 4, EditorGUIUtility.singleLineHeight), elementTarget, GUIContent.none);
            if(EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                data.ValidateData(index);
            }

            UnityEngine.Object elementTypeObj = element.FindPropertyRelative("Type").objectReferenceValue;
            if (elementTypeObj != null)
            {
                // Create foldout
                SerializedProperty showFoldout = element.FindPropertyRelative("Foldout");
                rect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 4;
                showFoldout.boolValue = EditorGUI.Foldout(new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth - GUI.skin.verticalScrollbar.fixedWidth, EditorGUIUtility.singleLineHeight), showFoldout.boolValue, "");
                if (showFoldout.boolValue)
                {
                    PlatformLabel platformLabel = new PlatformLabel();
                    platformLabel.OnGUI(rect, null, null);

                    SerializedObject elementType = new SerializedObject(elementTypeObj);
                    SerializedProperty elementTypeIterator = elementType.GetIterator();
                    elementTypeIterator.NextVisible(true);
                    while (elementTypeIterator.NextVisible(false))
                    {
                        elementTypeIterator.serializedObject.Update();
                        rect.y += EditorGUI.GetPropertyHeight(elementTypeIterator, false) * 1.5f;
                        EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), elementTypeIterator, true);
                        if (elementTypeIterator.isArray && elementTypeIterator.isExpanded && elementTypeIterator.arraySize != 0)
                        {
                            for (int i = 0; i < elementTypeIterator.arraySize; i++)
                            {
                                rect.y += EditorGUI.GetPropertyHeight(elementTypeIterator.GetArrayElementAtIndex(i), true) * 1.5f;
                            }
                        }
                        elementTypeIterator.serializedObject.ApplyModifiedProperties();
                    }
                }
            }

            rect.x = 0;
            rect.y = oldY + ElementHeightCallback(index) - EditorGUIUtility.standardVerticalSpacing;
            rect.width = EditorGUIUtility.currentViewWidth;
            rect.height = 1;
            EditorGUI.DrawRect(rect, Color.grey);
        }

        private float ElementHeightCallback(int index)
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            float propertyHeight = EditorGUI.GetPropertyHeight(element, false); // true = include children
            float nestedHeight = 0.0f;
            if(element.FindPropertyRelative("Foldout").boolValue)
            {
                UnityEngine.Object elementTypeObj = element.FindPropertyRelative("Type").objectReferenceValue;
                if(elementTypeObj != null)
                {
                    SerializedObject elementType = new SerializedObject(elementTypeObj);
                    SerializedProperty elementTypeIterator = elementType.GetIterator();
                    bool enableChildren = true;
                    while (elementTypeIterator.NextVisible(enableChildren))
                    {
                        if (elementTypeIterator.isExpanded && elementTypeIterator.isArray && elementTypeIterator.arraySize != 0)
                        {
                            nestedHeight += EditorGUI.GetPropertyHeight(elementTypeIterator, false) * 1.5f;
                            for (int i = 0; i < elementTypeIterator.arraySize; i++)
                            {
                                nestedHeight += EditorGUI.GetPropertyHeight(elementTypeIterator.GetArrayElementAtIndex(i), true) * 1.5f;
                            }
                        }
                        else
                        {
                            nestedHeight += EditorGUI.GetPropertyHeight(elementTypeIterator, true) * 1.5f;
                        }
                        enableChildren = false;
                    }
                }
            }
            else
            {
                nestedHeight += EditorGUIUtility.singleLineHeight;
            }
            float spacing = EditorGUIUtility.singleLineHeight;
            return propertyHeight + nestedHeight + spacing;
        }

        private void DrawElementBackgroundCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            if(reorderableList.count > 0)
            {
                if (reorderableList.serializedProperty != null)
                {
                    Rect newRect = new Rect(rect);
                    newRect.height = EditorGUI.GetPropertyHeight(reorderableList.serializedProperty.GetArrayElementAtIndex(index), false) * 2f + EditorGUIUtility.singleLineHeight;
                    EditorGUI.DrawRect(newRect, new Color(0, 0, 0, 0.15f));
                }
            }
            ReorderableList.defaultBehaviours.DrawElementBackground(rect, index, isActive, isFocused, reorderableList.draggable);
        }

        private void OnAddCallback(ReorderableList list)
        {
            int index = list.serializedProperty.arraySize;
            list.serializedProperty.arraySize++;
            list.index = index;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Target").objectReferenceValue = null;
            element.FindPropertyRelative("Type").objectReferenceValue = null;
            element.FindPropertyRelative("Foldout").boolValue = false;
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            if (list.count == 0) return;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
            element.FindPropertyRelative("Target").objectReferenceValue = null;
            UnityEngine.Object elementObj = element.FindPropertyRelative("Type").objectReferenceValue;
            if(elementObj != null) DestroyImmediate(elementObj);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }

        private void OnMouseDragCallback(ReorderableList list)
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(list.index);
            SerializedProperty showFoldout = element.FindPropertyRelative("Foldout");
            if (showFoldout.boolValue) showFoldout.boolValue = false;
        }
        #endregion

        private void OnGUI()
        {
            if(serializedObject != null) serializedObject.Update();

            // Header
            GUIStyle headerStyle = new GUIStyle("Box");
            GUILayout.BeginHorizontal(headerStyle);
            headerStyle.normal.background = headerBG;
            GUILayout.BeginHorizontal(headerStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Box(logo, GUILayout.Width(150), GUILayout.Height(70));
            GUILayout.FlexibleSpace();
            Rect settingsRect = GUILayoutUtility.GetLastRect();
            settingsRect.width /= 2;
            settingsRect.x += settingsRect.width;
            settingsRect.height = EditorGUIUtility.singleLineHeight + 2;
            settingsMenu = GUI.Toggle(settingsRect, settingsMenu, localization.Current.SettingsButton, new GUIStyle("Button"));
            GUILayout.EndHorizontal();
            GUILayout.EndHorizontal();

            if(settingsMenu)
            {
                if (EditorPrefs.GetInt("CacheServerMode") == 2) // CacheServerMode 2 = Disabled (not local or remote)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUIContent helpBoxContent = new GUIContent(localization.Current.SettingsCacheWarning, EditorGUIUtility.IconContent("console.warnicon").image);
                    GUIContent helpBoxButton = new GUIContent(localization.Current.SettingsCacheButton);
                    float width = GUI.skin.button.CalcSize(helpBoxButton).x - 11;
                    float height = EditorStyles.helpBox.CalcHeight(helpBoxContent, EditorGUIUtility.currentViewWidth - width);
                    GUILayout.Box(helpBoxContent, EditorStyles.helpBox);
                    if (GUILayout.Button(helpBoxButton, GUILayout.Height(height))) SettingsService.OpenUserPreferences("Preferences/Cache Server");
                    EditorGUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label(new GUIContent(localization.Current.SettingsLanguage));
                EditorGUI.BeginChangeCheck();
                chosenLanguage = EditorGUILayout.Popup(chosenLanguage, localization.GetLanguages());
                if(EditorGUI.EndChangeCheck())
                {
                    localization.SetLanguage(chosenLanguage);
                    EditorPrefs.SetInt("EQS_Language", chosenLanguage);
                }
                if (serializedObject == null) EditorGUI.BeginDisabledGroup(true);
                if (GUILayout.Button(localization.Current.SettingsApplyPC)) data.CheckTarget(BuildTarget.StandaloneWindows64);
                if (GUILayout.Button(localization.Current.SettingsApplyQuest)) data.CheckTarget(BuildTarget.Android);
                GUILayout.EndHorizontal();
                if (GUILayout.Button(localization.Current.SettingsRemoveEQS, GUILayout.Height(32)))
                {
                    if (EditorUtility.DisplayDialog(string.Empty, localization.Current.PopupDeleteWarning, localization.Current.PopupAccept, localization.Current.PopupDecline))
                    {
                        DestroyEQS();
                    }
                }
                if (serializedObject == null) EditorGUI.EndDisabledGroup();
                GUILayout.BeginVertical();
                    GUILayout.Box(localization.Current.SettingsExplanation, EditorStyles.wordWrappedLabel);
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                        GUILayout.Box(localization.Current.SettingsFeedback, EditorStyles.wordWrappedLabel);
                        if(GUILayout.Button(localization.Current.SettingsTwitter)) Application.OpenURL("https://twitter.com/JordoVR");
                        if(GUILayout.Button(localization.Current.SettingsGithub)) Application.OpenURL("https://github.com/JordoVR/EasyQuestSwitch");
                    GUILayout.EndHorizontal();
                GUILayout.EndVertical();

            }
            else
            {
                if(data == null)
                {
                    if(GUILayout.Button(localization.Current.ListSetupEQS)) CreateEQS();
                } 
                else
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Toggle(false, localization.Current.ListExpand, EditorStyles.toolbarButton))
                    {
                        for (int i = 0; i < reorderableList.serializedProperty.arraySize; i++)
                        {
                            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(i);
                            if (element.FindPropertyRelative("Type").objectReferenceValue != null) element.FindPropertyRelative("Foldout").boolValue = true;
                        }
                        Repaint();
                    }
                    if (GUILayout.Toggle(false, localization.Current.ListFold, EditorStyles.toolbarButton))
                    {
                        for (int i = 0; i < reorderableList.serializedProperty.arraySize; i++)
                        {
                            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(i);
                            element.FindPropertyRelative("Foldout").boolValue = false;
                        }
                        Repaint();
                    }
                    if (GUILayout.Button("+", EditorStyles.toolbarButton)) OnAddCallback(reorderableList);
                    if (GUILayout.Button("-", EditorStyles.toolbarButton)) OnRemoveCallback(reorderableList);
                    GUILayout.EndHorizontal();
                    scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.scrollView);
                    reorderableList.DoLayoutList();
                    EditorGUILayout.EndScrollView();
                }
            }
            if(serializedObject != null) serializedObject.ApplyModifiedProperties();
        }
    }
}