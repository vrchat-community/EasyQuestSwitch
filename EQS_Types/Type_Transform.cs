#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Transform : Type_Base
    {
        [System.NonSerialized]
        private Transform type;

        public SharedVector3 Position = new SharedVector3();
        public SharedVector3 Rotation = new SharedVector3();
        public SharedVector3 Scale = new SharedVector3();

        public override void Setup(Object type)
        {
            Transform component = (Transform)type;
            Position.Setup(component.position);
            Rotation.Setup(component.localEulerAngles);
            Scale.Setup(component.localScale);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            Transform component = (Transform)type;
            component.position = Position.Get(buildTarget);
            component.localEulerAngles = Rotation.Get(buildTarget);
            component.localScale = Scale.Get(buildTarget);
        }
    }
}
#endif

