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

    [ExecuteInEditMode]
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
                if(data.Type != null)
                {
                    // Target has been changed, remove current type and apply a new one
                    DestroyImmediate(Objects[index].Type);
                    Objects[index].Type = null;
                    Objects[index].Foldout = false;
                }

                IEnumerable<Type> everyTypes = from t in Assembly.GetExecutingAssembly().GetTypes() where t.IsClass && t.Namespace == "EasyQuestSwitch.Types" select t;
                // TODO: I wrote this while I was very sleep deprived due to crunch time for Vket 4, so expect an optimisation sometime in the future
                for(int i = 0; i < everyTypes.Count(); i++)
                {
                    FieldInfo field = everyTypes.ElementAt(i).GetField("type", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field == null) continue;
                    Type fieldType = field.FieldType;
                    /*if (data.Target.GetType().IsAssignableFrom(fieldType)) Debug.LogFormat("{0} is Assignable from {1} ? {2}", data.Target.GetType(), fieldType, data.Target.GetType().IsAssignableFrom(fieldType));
                    if (data.Target.GetType().IsSubclassOf(fieldType)) Debug.LogFormat("{0} is Subclass of {1} ? {2}", data.Target.GetType(), fieldType, data.Target.GetType().IsSubclassOf(fieldType));*/
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
                    Debug.LogError("This is an unsupported component");
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

        public BuildTarget CachedBuildTarget; // Last build target used in this scene
        public BuildTarget NewBuildTarget;

        private void Update()
        {
            NewBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            if (CachedBuildTarget != NewBuildTarget && Objects != null) CheckTarget(NewBuildTarget);
            CachedBuildTarget = NewBuildTarget;
        }

        public void CheckTarget(BuildTarget newTarget)
        {
            if(newTarget == BuildTarget.StandaloneWindows64 || newTarget == BuildTarget.StandaloneWindows || newTarget == BuildTarget.Android)
            {
                foreach(Data d in Objects)
                {
                    if(d.Type != null) d.Type.Process(d.Target, newTarget);
                }
                Debug.LogFormat("EasyQuestSwitch applied the scene changes of build target {0}", newTarget);
            }
            else
            {
                Debug.LogError("Switched to an unsupported build target.");
            }
        }
    }

}
#endif
