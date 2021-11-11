#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_SphereCollider : Type_Collider
    {
        [System.NonSerialized]
        private SphereCollider type;

        public SharedVector3 Center = new SharedVector3();
        public SharedFloat Radius = new SharedFloat();

        public override void Setup(Object type)
        {
            base.Setup(type);
            SphereCollider component = (SphereCollider)type;
            Center.Setup(component.center);
            Radius.Setup(component.radius);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            SphereCollider component = (SphereCollider)type;
            component.center = Center.Get(buildTarget);
            component.radius = Radius.Get(buildTarget);
        }
    }
}
#endif

