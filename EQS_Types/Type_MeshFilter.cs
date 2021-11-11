#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
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
            component.sharedMesh = Mesh.Get(buildTarget);
        }
    }
}
#endif

