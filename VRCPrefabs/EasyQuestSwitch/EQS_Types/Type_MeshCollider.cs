#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_MeshCollider : Type_Collider
    {
        [System.NonSerialized]
        private MeshCollider type;

        public SharedBool Convex = new SharedBool();
        public SharedMesh SharedMesh = new SharedMesh();

        public override void Setup(Object type)
        {
            base.Setup(type);
            MeshCollider component = (MeshCollider)type;
            Convex.Setup(component.convex);
            SharedMesh.Setup(component.sharedMesh);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            MeshCollider component = (MeshCollider)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.enabled = Enabled.PC;
                    component.isTrigger = IsTrigger.PC && Convex.PC ? true : false;
                    component.convex = Convex.PC;
                    component.sharedMesh = SharedMesh.PC;
                    break;
                case BuildTarget.Android:
                    component.enabled = Enabled.Quest;
                    component.isTrigger = IsTrigger.Quest && Convex.Quest ? true : false;
                    component.convex = Convex.Quest;
                    component.sharedMesh = SharedMesh.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

