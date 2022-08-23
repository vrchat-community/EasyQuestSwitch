using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using EasyQuestSwitch.Fields;
using System.IO;
using UnityEditor.Build;

namespace EasyQuestSwitch
{

    public class EQS_Window : EditorWindow
    {
        private const string version = "1.2.0";
        
        [MenuItem("Window/Easy Quest Switch")]
        public static void ShowWindow()
        {
            GetWindow(typeof(EQS_Window), false, "Easy Quest Switch");
        }

        private SerializedObject serializedObject;
        private SerializedProperty eqsData;
        private ReorderableList reorderableList;

        private EQS_Data _data;
        private EQS_Data data {
            get {
                if (_data == null) _data = GameObject.Find("EQS_DATA")?.GetComponent<EQS_Data>();
                return _data;
            }
            set {
                _data = value;
            }
        }

        private Vector2 scrollPos;

        private Texture2D logo;
        [SerializeField]
        private Texture2D headerBG;

        private bool settingsMenu;
        private int chosenLanguage;
        private int chosenListFormat;
        private bool revealEQSdata = false;
        private float sideOffset = 0f;
        private bool showHierarchyIcon = true;

        private void CreatePlatformDependantHeader(BuildTarget buildTarget)
        {
            headerBG = new Texture2D(1, 1);
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    headerBG.SetPixel(0, 0, new Color(0.15f, 0.5f, 0.75f));
                    break;
                case BuildTarget.Android:
                    headerBG.SetPixel(0, 0, new Color(0, 0.8f, 0.3f));
                    break;
                default:
                    headerBG.SetPixel(0, 0, new Color(0.75f, 0.15f, 0.2f));
                    break;
            }
            headerBG.Apply();
        }

        private void OnEnable()
        {
            serializedObject = null;
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
            Undo.undoRedoPerformed += OnUndoRedo;

            minSize = new Vector2(512, 400);
            CreatePlatformDependantHeader(EditorUserBuildSettings.activeBuildTarget);
            logo = (Texture2D)Resources.Load("EQS_Logo", typeof(Texture2D));

            EQS_Localization.LoadLanguages();
            // Prior to 1.2, language preferences were set using the index order instead of the language code
            if (System.String.IsNullOrEmpty(EditorPrefs.GetString("EQS_Language", null)))
            {
                switch(EditorPrefs.GetInt("EQS_Language", 0))
                {
                    default:
                    case 0:
                        EditorPrefs.SetString("EQS_Language", "en");
                        break;
                    case 1:
                        EditorPrefs.SetString("EQS_Language", "jp");
                        break;
                }
            }
            chosenLanguage = EQS_Localization.SetLanguage(EditorPrefs.GetString("EQS_Language", "en"));
            chosenListFormat = EditorPrefs.GetInt("EQS_ListFormat", 0); // 0 - Simple, 1 - Reorderable
            sideOffset = EditorPrefs.GetFloat("EQS_HierarchySideOffset", 0f);
            showHierarchyIcon = EditorPrefs.GetBool("EQS_ShowHierarchyIcon", true);

            if (data != null)
            {
                SetupEQS();
                revealEQSdata = data.gameObject.hideFlags == HideFlags.HideInHierarchy ? false : true;
            }
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

        private void RevealEQSData() { if(data != null) data.gameObject.hideFlags = HideFlags.None; }
        private void HideEQSData()
        {
            if (data != null)
            {
                data.gameObject.hideFlags = HideFlags.HideInHierarchy;
                EditorApplication.DirtyHierarchyWindowSorting();
            }
        }

        private void SetupEQS()
        {
            serializedObject = new SerializedObject(data);
            eqsData = serializedObject.FindProperty("Objects");

            if(chosenListFormat == 0) // Simple
            {
                if (reorderableList != null)
                {
                    UnregisterReorderableListCallbacks();
                    reorderableList = null;
                }
            }
            else if (chosenListFormat == 1) // Reorderable
            {
                reorderableList = new ReorderableList(serializedObject, eqsData, true, false, false, false);
                RegisterReorderableListCallbacks();
                reorderableList.showDefaultBackground = false;
                reorderableList.headerHeight = 0;
            }

            EQS_HierarchyController.InitializeEQSHierarchy();
        }

        private void RegisterReorderableListCallbacks()
        {
            reorderableList.drawElementCallback += DrawElementCallback;
            reorderableList.elementHeightCallback += ElementHeightCallback;
            reorderableList.drawElementBackgroundCallback += DrawElementBackgroundCallback;
            reorderableList.onMouseDragCallback += OnMouseDragCallback;
        }

        private void UnregisterReorderableListCallbacks()
        {
            reorderableList.drawElementCallback -= DrawElementCallback;
            reorderableList.elementHeightCallback -= ElementHeightCallback;
            reorderableList.drawElementBackgroundCallback -= DrawElementBackgroundCallback;
            reorderableList.onMouseDragCallback -= OnMouseDragCallback;
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

        private void OnMouseDragCallback(ReorderableList list)
        {
            SerializedProperty element = reorderableList.serializedProperty.GetArrayElementAtIndex(list.index);
            SerializedProperty showFoldout = element.FindPropertyRelative("Foldout");
            if (showFoldout.boolValue) showFoldout.boolValue = false;
        }
        #endregion

        private void OnAddCallback(SerializedProperty list)
        {
            int index = list.arraySize;
            list.arraySize++;
            SerializedProperty element = list.GetArrayElementAtIndex(index);
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
            if (elementObj != null) DestroyImmediate(elementObj);
            ReorderableList.defaultBehaviours.DoRemoveButton(reorderableList);
        }

        private void OnRemoveCallback(SerializedProperty list, int index)
        {
            if (list.arraySize == 0) return;
            SerializedProperty element = list.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Target").objectReferenceValue = null;
            UnityEngine.Object elementObj = element.FindPropertyRelative("Type").objectReferenceValue;
            if (elementObj != null) DestroyImmediate(elementObj);
            list.DeleteArrayElementAtIndex(index);
            EditorApplication.DirtyHierarchyWindowSorting();
        }

        private void OnGUI()
        {
            if(serializedObject != null) serializedObject.Update();

            // Header
            GUIStyle headerStyle = new GUIStyle("Box");
            using (new GUILayout.HorizontalScope(headerStyle))
            {
                headerStyle.normal.background = headerBG;
                using (var scope = new EditorGUILayout.HorizontalScope(headerStyle))
                {

                    GUILayout.FlexibleSpace();
                    Color guiBackgroundColor = GUI.backgroundColor;
                    GUI.backgroundColor = new Color(1,1,1,0);
                    GUILayout.Box(logo, GUILayout.Width(150), GUILayout.Height(70));
                    GUI.backgroundColor = guiBackgroundColor;
                    GUILayout.FlexibleSpace();

                    Rect versionLabelRect, settingsButtonRect;
                    versionLabelRect = settingsButtonRect = scope.rect;

                    settingsButtonRect.xMin = scope.rect.xMax - EditorStyles.label.CalcSize(new GUIContent(EQS_Localization.Current.SettingsButton)).x - 40;
                    settingsButtonRect.yMax = scope.rect.yMin + EditorGUIUtility.singleLineHeight + 2;
                    settingsButtonRect.x -= 3;
                    settingsButtonRect.y += 3;
                    settingsMenu = GUI.Toggle(settingsButtonRect, settingsMenu, EQS_Localization.Current.SettingsButton, new GUIStyle("Button"));
                    
                    string eqsVersion = $"v{version}";
                    versionLabelRect.xMin = scope.rect.xMax - EditorStyles.label.CalcSize(new GUIContent(eqsVersion)).x - 2;
                    versionLabelRect.yMin = scope.rect.yMax - EditorGUIUtility.singleLineHeight;
                    GUI.Label(versionLabelRect, $"v{version}");
                }
            }

            if(settingsMenu)
            {
#if UNITY_2019_4_OR_NEWER
                if (EditorSettings.assetPipelineMode != AssetPipelineMode.Version2)
                {
                    
                    using (new GUILayout.HorizontalScope())
                    {
                        GUIContent helpBoxContent = new GUIContent(EQS_Localization.Current.SettingsAssetPipelineV2, EditorGUIUtility.IconContent("console.warnicon").image);
                        GUIContent helpBoxButton = new GUIContent(EQS_Localization.Current.SettingsCacheButton);
                        float width = GUI.skin.button.CalcSize(helpBoxButton).x - 11;
                        float height = EditorStyles.helpBox.CalcHeight(helpBoxContent, EditorGUIUtility.currentViewWidth - width);
                        GUILayout.Box(helpBoxContent, EditorStyles.helpBox);
                        if (GUILayout.Button(helpBoxButton, GUILayout.Height(height))) SettingsService.OpenProjectSettings("Project/Editor");
                    }
                }
#else
                if (EditorPrefs.GetInt("CacheServerMode") == 2) // CacheServerMode 2 = Disabled (not local or remote)
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        GUIContent helpBoxContent = new GUIContent(EQS_Localization.Current.SettingsCacheWarning, EditorGUIUtility.IconContent("console.warnicon").image);
                        GUIContent helpBoxButton = new GUIContent(EQS_Localization.Current.SettingsCacheButton);
                        float width = GUI.skin.button.CalcSize(helpBoxButton).x - 11;
                        float height = EditorStyles.helpBox.CalcHeight(helpBoxContent, EditorGUIUtility.currentViewWidth - width);
                        GUILayout.Box(helpBoxContent, EditorStyles.helpBox);
                        if (GUILayout.Button(helpBoxButton, GUILayout.Height(height))) SettingsService.OpenUserPreferences("Preferences/Cache Server");
                    }
                }
#endif

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(new GUIContent(EQS_Localization.Current.SettingsLanguage));
                    using(var changeLanguage = new EditorGUI.ChangeCheckScope())
                    {
                        chosenLanguage = EditorGUILayout.Popup(chosenLanguage, EQS_Localization.GetLanguages());
                        if(changeLanguage.changed)
                        {
                            EQS_Localization.SetLanguage(chosenLanguage);
                            EditorPrefs.SetString("EQS_Language", EQS_Localization.Current.Code);
                        }
                    }

                    GUILayout.Label(new GUIContent(EQS_Localization.Current.SettingsListFormat));
                    using (var changeListFormat = new EditorGUI.ChangeCheckScope())
                    {
                        chosenListFormat = EditorGUILayout.Popup(chosenListFormat, EQS_Localization.Current.SettingsListFormatArray);
                        if (changeListFormat.changed)
                        {
                            SetupEQS();
                            EditorPrefs.SetInt("EQS_ListFormat", chosenListFormat);
                        }
                    }
                }

                using (new EditorGUI.DisabledGroupScope(serializedObject == null))
                {
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Button(EQS_Localization.Current.SettingsApplyPC)) data.ApplyTarget(BuildTarget.StandaloneWindows64);
                        if (GUILayout.Button(EQS_Localization.Current.SettingsApplyQuest)) data.ApplyTarget(BuildTarget.Android);
                    }
                    if (GUILayout.Button(EQS_Localization.Current.SettingsRemoveEQS, GUILayout.Height(32)))
                    {
                        if (EditorUtility.DisplayDialog(string.Empty, EQS_Localization.Current.PopupDeleteWarning, EQS_Localization.Current.PopupAccept, EQS_Localization.Current.PopupDecline))
                        {
                            DestroyEQS();
                        }
                    }
                    using (var changeHierarchySettings = new EditorGUI.ChangeCheckScope())
                    {
                        EditorGUILayout.LabelField(EQS_Localization.Current.SettingsHierarchy, EditorStyles.boldLabel);
                        showHierarchyIcon = EditorGUILayout.Toggle(EQS_Localization.Current.SettingsHierarchyIconShow, showHierarchyIcon);
                        sideOffset = EditorGUILayout.Slider(EQS_Localization.Current.SettingsHierarchyIconOffset, sideOffset, -100f, 15f);
                        if (changeHierarchySettings.changed)
                        {
                            EditorPrefs.SetFloat("EQS_HierarchySideOffset", sideOffset);
                            EditorPrefs.SetBool("EQS_ShowHierarchyIcon", showHierarchyIcon);
                            EQS_HierarchyController.InitializeEQSHierarchy();
                            EditorApplication.DirtyHierarchyWindowSorting();
                        }
                    }
                    EditorGUILayout.Space(10);
                }

                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Box(EQS_Localization.Current.SettingsExplanation, EditorStyles.wordWrappedLabel);
                    GUILayout.FlexibleSpace();
                    using (new EditorGUI.DisabledGroupScope(serializedObject == null))
                    {
                        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                        {
                            GUILayout.FlexibleSpace();
                            GUIContent helpBoxContent = new GUIContent(EQS_Localization.Current.SettingsDebugOptions, EditorGUIUtility.IconContent("console.warnicon").image);
                            GUILayout.Box(helpBoxContent, EditorStyles.miniLabel);
                            GUILayout.FlexibleSpace();
                            using(new GUILayout.VerticalScope())
                            {
                                using (var toggle = new EditorGUI.ChangeCheckScope())
                                {
                                    revealEQSdata = GUILayout.Toggle(revealEQSdata, EQS_Localization.Current.SettingsDebugReveal);
                                    if(toggle.changed)
                                    {
                                        if (revealEQSdata)
                                        {
                                            RevealEQSData();
                                        }
                                        else
                                        {
                                            HideEQSData();
                                        }
                                    }
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                    }
                    using (new GUILayout.HorizontalScope())
                    {
                        GUILayout.Box(EQS_Localization.Current.SettingsFeedback, EditorStyles.wordWrappedLabel);
                        if (GUILayout.Button(EQS_Localization.Current.SettingsTwitter, GUILayout.ExpandHeight(true))) Application.OpenURL("https://twitter.com/JordoVR");
                        if (GUILayout.Button(EQS_Localization.Current.SettingsGithub, GUILayout.ExpandHeight(true))) Application.OpenURL("https://github.com/JordoVR/EasyQuestSwitch");
                    }

                }

            }
            else
            {
                if(data == null)
                {
                    if(GUILayout.Button(EQS_Localization.Current.ListSetupEQS)) CreateEQS();
                } 
                else
                {
                    // List header
                    using (new GUILayout.HorizontalScope())
                    {
                        if (GUILayout.Toggle(false, EQS_Localization.Current.ListExpand, EditorStyles.toolbarButton))
                        {
                            for (int i = 0; i < eqsData.arraySize; i++)
                            {
                                SerializedProperty element = eqsData.GetArrayElementAtIndex(i);
                                if (element.FindPropertyRelative("Type").objectReferenceValue != null) element.FindPropertyRelative("Foldout").boolValue = true;
                            }
                            Repaint();
                        }
                        if (GUILayout.Toggle(false, EQS_Localization.Current.ListFold, EditorStyles.toolbarButton))
                        {
                            for (int i = 0; i < eqsData.arraySize; i++)
                            {
                                SerializedProperty element = eqsData.GetArrayElementAtIndex(i);
                                element.FindPropertyRelative("Foldout").boolValue = false;
                            }
                            Repaint();
                        }
                        if (GUILayout.Button("+", EditorStyles.toolbarButton))
                        {
                            if (chosenListFormat == 0) // Simple
                            {
                                OnAddCallback(eqsData);
                            }
                            else if (chosenListFormat == 1) // Reorderable
                            {
                                OnAddCallback(reorderableList.serializedProperty);
                                reorderableList.index++;
                            }
                            scrollPos = new Vector2(0,Mathf.Infinity);
                        }
                        if (chosenListFormat == 1) // Reorderable
                        {
                            if (GUILayout.Button("-", EditorStyles.toolbarButton))
                            {
                                OnRemoveCallback(reorderableList);
                            }
                        }
                    }

                    // Begin scroll & list
                    using (var scrollView = new GUILayout.ScrollViewScope(scrollPos, false, true, GUIStyle.none, GUI.skin.verticalScrollbar, GUI.skin.scrollView, GUILayout.ExpandHeight(false)))
                    {
                        scrollPos = scrollView.scrollPosition;

                        if (chosenListFormat == 0) // Simple
                        {
                            for (int index = 0; index < eqsData.arraySize; index++)
                            {
                                SerializedProperty element = eqsData.GetArrayElementAtIndex(index);
                                SerializedProperty elementTarget = element.FindPropertyRelative("Target");


                                using (new GUILayout.HorizontalScope())
                                {
                                    if (GUILayout.Button("\u2005\u2006▲", EditorStyles.toolbarButton, GUILayout.Width(24)))
                                    {
                                        if (index == 0) break;
                                        eqsData.MoveArrayElement(index, index - 1);
                                    }
                                    if (GUILayout.Button("\u2005\u2006▼", EditorStyles.toolbarButton, GUILayout.Width(24)))
                                    {
                                        if (index == eqsData.arraySize) break;
                                        eqsData.MoveArrayElement(index, index + 1);
                                    }
                                    EditorGUI.BeginChangeCheck();
                                    EditorGUILayout.PropertyField(elementTarget, new GUIContent());
                                    if (EditorGUI.EndChangeCheck())
                                    {
                                        serializedObject.ApplyModifiedProperties();
                                        data.ValidateData(index);
                                        EditorApplication.DirtyHierarchyWindowSorting();
                                    }
                                    if (GUILayout.Button("\u2005\u2006-", EditorStyles.toolbarButton, GUILayout.Width(24)))
                                    {
                                        OnRemoveCallback(eqsData, index);
                                        continue;
                                    }
                                }
                                // Color the background
                                Rect newRect = new Rect(GUILayoutUtility.GetLastRect());
                                float boxHeight = newRect.height += 20;
                                EditorGUI.DrawRect(newRect, new Color(0, 0, 0, 0.15f));

                                UnityEngine.Object elementTypeObj = element.FindPropertyRelative("Type").objectReferenceValue;
                                if (elementTypeObj != null)
                                {
                                    // Create foldout
                                    SerializedProperty showFoldout = element.FindPropertyRelative("Foldout");
                                    showFoldout.boolValue = EditorGUILayout.Foldout(showFoldout.boolValue, "");

                                    if (showFoldout.boolValue)
                                    {
                                        PlatformLabel platformLabel = new PlatformLabel();
                                        platformLabel.OnGUI(GUILayoutUtility.GetLastRect(), null, null);

                                        SerializedObject elementType = new SerializedObject(elementTypeObj);
                                        SerializedProperty elementTypeIterator = elementType.GetIterator();
                                        elementTypeIterator.NextVisible(true);

                                        while (elementTypeIterator.NextVisible(false))
                                        {
                                            elementTypeIterator.serializedObject.Update();
                                            EditorGUILayout.PropertyField(elementTypeIterator, true);
                                            elementTypeIterator.serializedObject.ApplyModifiedProperties();
                                            boxHeight += EditorGUI.GetPropertyHeight(elementTypeIterator) + EditorGUIUtility.standardVerticalSpacing;
                                        }
                                    }
                                }
                                else
                                {
                                    GUILayout.Space(boxHeight);
                                }

                                GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
                                newRect.y += boxHeight + EditorGUIUtility.standardVerticalSpacing - 1;
                                newRect.height = 1;
                                EditorGUI.DrawRect(newRect, Color.grey);
                            }
                        }
                        else if (chosenListFormat == 1) // Reorderable
                        {
                            reorderableList.DoLayoutList();
                        }
                    }

                    EditorGUILayout.LabelField(new GUIContent(EQS_Localization.Current.ListDragAndDrop), new GUIStyle(EditorStyles.toolbar)
                    {
                        fixedWidth = EditorGUIUtility.currentViewWidth,
                        alignment = TextAnchor.MiddleCenter,
                        stretchHeight = true,
                        fontSize = 24,
                        fixedHeight = 0,
                    }, GUILayout.ExpandHeight(true), GUILayout.MinHeight(100));

                    var rect = GUILayoutUtility.GetLastRect();
                    if (rect.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.DragUpdated)
                        {
                            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                            Event.current.Use();
                        }
                        else if (Event.current.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
                            foreach (var draggedObject in DragAndDrop.objectReferences)
                            {
                                serializedObject?.Update();
                                int index = eqsData.arraySize;
                                eqsData.arraySize++;
                                SerializedProperty element = eqsData.GetArrayElementAtIndex(index);
                                element.FindPropertyRelative("Target").objectReferenceValue = draggedObject;
                                element.FindPropertyRelative("Type").objectReferenceValue = null;
                                element.FindPropertyRelative("Foldout").boolValue = false;
                                serializedObject?.ApplyModifiedProperties();
                                data.ValidateData(index);
                            }
                            EditorApplication.DirtyHierarchyWindowSorting();
                            scrollPos = new Vector2(0, Mathf.Infinity);
                        }
                    }
                }
            }
            serializedObject?.ApplyModifiedProperties();
        }
    }
}
