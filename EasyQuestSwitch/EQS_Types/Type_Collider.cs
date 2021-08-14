#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Collider : Type_Base
    {
        [System.NonSerialized]
        private Collider type;

        public SharedBool Enabled = new SharedBool();
        public SharedBool IsTrigger = new SharedBool();

        public override void Setup(Object type)
        {
            Collider component = (Collider)type;
            Enabled.Setup(component.enabled);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            Collider component = (Collider)type;
            component.enabled = Enabled.Get(buildTarget);
            component.isTrigger = IsTrigger.Get(buildTarget);
        }
    }
}
#endif

