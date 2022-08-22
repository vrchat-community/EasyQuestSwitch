#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using Object = UnityEngine.Object;
using EasyQuestSwitch.Types;
using System.Reflection;
using System.Linq;
using System.Collections;

namespace EasyQuestSwitch
{
    [ExecuteInEditMode, AddComponentMenu("")]
    public class EQS_Data : MonoBehaviour
    {

        [Serializable]
        public class Data
        {
            public Object Target;
            public Type_Base Type;
            public bool Foldout;
        }

        public List<Data> Objects;

        public void ValidateData(int index)
        {
            Data data = Objects[index];
            if (data.Target != null)
            {
                for(int i = 0; i < Objects.Count; i++)
                {
                    if (i == index) continue;
                    if (Objects[i].Target == data.Target)
                    {
                        Debug.LogError(EQS_Localization.Current.LogPrefix + EQS_Localization.Current.LogComponentExists);
                        DestroyImmediate(Objects[index].Type);
                        Objects.RemoveAt(index);
                        return;
                    }
                }
                if(data.Type != null)
                {
                    // Target has been changed, remove current type and apply a new one
                    DestroyImmediate(Objects[index].Type);
                    Objects[index].Type = null;
                    Objects[index].Foldout = false;
                }

                IEnumerable<Type> everyTypes = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == "EasyQuestSwitch.Types" select t;
                for(int i = 0; i < everyTypes.Count(); i++)
                {
                    FieldInfo field = everyTypes.ElementAt(i).GetField("type", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field == null) continue;
                    Type fieldType = field.FieldType;
                    if (data.Target.GetType() == fieldType)
                    {
                        Objects[index].Type = (Type_Base)gameObject.AddComponent(everyTypes.ElementAt(i));
                        break;
                    }
                }
                if(Objects[index].Type == null)
                {
                    for(int i = 0; i < everyTypes.Count(); i++)
                    {
                        FieldInfo field = everyTypes.ElementAt(i).GetField("type", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (field == null) continue;
                        Type fieldType = field.FieldType;
                        if (data.Target.GetType().IsSubclassOf(fieldType))
                        {
                            Objects[index].Type = (Type_Base)gameObject.AddComponent(everyTypes.ElementAt(i));
                            break;
                        }
                    }
                }
                if(Objects[index].Type == null)
                {
                    Debug.LogError(EQS_Localization.Current.LogPrefix + EQS_Localization.Current.LogUnsupportedComponent);
                    Objects[index].Target = null;
                    Objects[index].Foldout = false;
                    return;
                }
                Objects[index].Type.Setup(Objects[index].Target);
                Objects[index].Foldout = true;
            } 
            else if (data.Target == null && data.Type != null)
            {
                // Target has been removed but not the corresponding type, remove the type
                DestroyImmediate(Objects[index].Type);
                Objects[index].Type = null;
                Objects[index].Foldout = false;
            }
        }

        [Serializable]
        public class BuildInfo
        {
            public BuildTarget CachedBuildTarget; // Last build target used in this scene
            public BuildTarget NewBuildTarget;
        }
        [SerializeField]
        private BuildInfo buildInfo = new BuildInfo();

        public void OnChangedBuildTarget(BuildTarget newTarget)
        {
            buildInfo.NewBuildTarget = newTarget;
            if (buildInfo.CachedBuildTarget != buildInfo.NewBuildTarget && Objects != null) ApplyTarget(buildInfo.NewBuildTarget);
            buildInfo.CachedBuildTarget = buildInfo.NewBuildTarget;
        }

        public void OnSceneOpened()
        {
            buildInfo.NewBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (buildInfo.CachedBuildTarget != buildInfo.NewBuildTarget && Objects != null)
            {
                string displayDialog = string.Format(EQS_Localization.Current.PopupTargetChanged, buildInfo.NewBuildTarget.ToString());
                if (EditorUtility.DisplayDialog("", displayDialog, EQS_Localization.Current.PopupAccept, EQS_Localization.Current.PopupDecline))
                {
                    ApplyTarget(buildInfo.NewBuildTarget);
                    buildInfo.CachedBuildTarget = buildInfo.NewBuildTarget;
                }
            }
        }

        public void ApplyTarget(BuildTarget newTarget)
        {
            if(newTarget == BuildTarget.StandaloneWindows64 || newTarget == BuildTarget.Android)
            {
                for(int i = 0; i < Objects.Count; i++)
                {
                    Data d = Objects[i];
                    if(d.Target == null)
                    {
                        Debug.LogErrorFormat(EQS_Localization.Current.LogPrefix + EQS_Localization.Current.LogSwitchMissing, i);
                    }
                    else
                    {
                        try
                        {
                            d.Type.Process(d.Target, newTarget);
                            PrefabUtility.RecordPrefabInstancePropertyModifications(d.Target);
                        }
                        catch (Exception e)
                        {
                            Debug.LogErrorFormat(EQS_Localization.Current.LogPrefix + EQS_Localization.Current.LogSwitchFailure, i, d.Target.name, e.Message);
                        }
                    }
                }
                Debug.LogFormat(EQS_Localization.Current.LogPrefix + EQS_Localization.Current.LogSwitchSuccess, newTarget);
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
            }
            else
            {
                Debug.LogError(EQS_Localization.Current.LogPrefix + EQS_Localization.Current.LogSwitchUnsupported);
            }
        }
    }

}
#endif
