#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_MeshFilter : Type_Base
    {
        [System.NonSerialized]
        private MeshFilter type;

        public SharedMesh Mesh = new SharedMesh();

        public override void Setup(Object type)
        {
            MeshFilter component = (MeshFilter)type;
            Mesh.Setup(component.sharedMesh);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            MeshFilter component = (MeshFilter)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.sharedMesh = Mesh.PC;
                    break;
                case BuildTarget.Android:
                    component.sharedMesh = Mesh.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

