#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_SkinnedMeshRenderer : Type_Renderer
    {
        [System.NonSerialized]
        private SkinnedMeshRenderer type;

        public SharedMesh Mesh = new SharedMesh();
        public SharedBool UpdateWhenOffscreen = new SharedBool();

        public override void Setup(Object type)
        {
            base.Setup(type);
            SkinnedMeshRenderer component = (SkinnedMeshRenderer)type;
            Mesh.Setup(component.sharedMesh);
            UpdateWhenOffscreen.Setup(component.updateWhenOffscreen);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            SkinnedMeshRenderer component = (SkinnedMeshRenderer)type;
            component.sharedMesh = Mesh.Get(buildTarget);
            component.updateWhenOffscreen = UpdateWhenOffscreen.Get(buildTarget);
        }
    }
}
#endif

