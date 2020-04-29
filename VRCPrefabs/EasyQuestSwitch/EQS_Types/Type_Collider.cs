#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
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
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.enabled = Enabled.PC;
                    component.isTrigger = IsTrigger.PC;
                    break;
                case BuildTarget.Android:
                    component.enabled = Enabled.Quest;
                    component.isTrigger = IsTrigger.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

