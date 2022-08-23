using UnityEditor;
using UnityEngine;

namespace EasyQuestSwitch
{
    public static class EQS_HierarchyController
    {
        private static GUIStyle entryStyle;
        private static EQS_Data data;
        private static bool initialized;
        private static Texture2D logo;
        private static float sideOffset = 0f;
        private static bool showHierarchyIcon = true;

        [InitializeOnLoadMethod]
        private static void OnInitialize()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
        }

        public static void InitializeEQSHierarchy()
        {
            if (logo == null)
            {
                logo = (Texture2D)Resources.Load("EQS_Logo_Crop", typeof(Texture2D));
            }
            if (data == null)
            {
                data = GameObject.Find("EQS_DATA")?.GetComponent<EQS_Data>();
            }

            sideOffset = EditorPrefs.GetFloat("EQS_HierarchySideOffset", 0f);
            showHierarchyIcon = EditorPrefs.GetBool("EQS_ShowHierarchyIcon", true);
            if (!showHierarchyIcon)
            {
                EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItemGUI;
            }
            else if (initialized)
            { // only re-add the callback if the settings were edited
                EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemGUI;
            }

            initialized = true;
        }


        private static void OnHierarchyItemGUI(int id, Rect rect)
        {
            var instance = EditorUtility.InstanceIDToObject(id) as GameObject;
            if (instance == null) return;

            if (!initialized)
            {
                InitializeEQSHierarchy();
            }

            if (!showHierarchyIcon)
            {
                return;
            }

            if (entryStyle == null)
            {
                entryStyle = new GUIStyle(EditorStyles.label)
                {
                    alignment = TextAnchor.MiddleRight,
                    fixedHeight = 18,
                    fixedWidth = 18
                };
            }

            if (data == null || data.Objects == null) return;

            var targetIndex = data.Objects.FindIndex(i =>
              (i.Target as Component)?.gameObject.GetHashCode() == instance.GetHashCode() ||
              (i.Target as GameObject)?.GetHashCode() == instance.GetHashCode());

            if (targetIndex == -1)
            {
                return;
            }

            var newRect = rect;
            newRect.xMax += sideOffset;
            newRect.xMin = newRect.xMax - 18;
            EditorGUI.LabelField(newRect, new GUIContent(logo), entryStyle);
            var evt = Event.current;
            if (evt.type == EventType.MouseUp && newRect.Contains(evt.mousePosition))
            {
                EditorWindow.GetWindow<EQS_Window>(false, "Easy Quest Switch");
                var tSo = new SerializedObject(data);
                tSo.FindProperty("Objects").GetArrayElementAtIndex(targetIndex).FindPropertyRelative("Foldout").boolValue =
                  true;
                tSo.ApplyModifiedProperties();
            }
        }
    }
}
