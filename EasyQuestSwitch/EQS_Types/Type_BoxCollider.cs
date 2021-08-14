#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_BoxCollider : Type_Collider
    {
        [System.NonSerialized]
        private BoxCollider type;

        public SharedVector3 Size = new SharedVector3();
        public SharedVector3 Center = new SharedVector3();

        public override void Setup(Object type)
        {
            base.Setup(type);
            BoxCollider component = (BoxCollider)type;
            Size.Setup(component.size);
            Center.Setup(component.center);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            BoxCollider component = (BoxCollider)type;
            component.size = Size.Get(buildTarget);
            component.center = Center.Get(buildTarget);
        }
    }
}
#endif

