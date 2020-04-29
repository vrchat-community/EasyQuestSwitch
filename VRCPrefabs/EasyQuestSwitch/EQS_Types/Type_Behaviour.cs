#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_Behaviour : Type_Base
    {
        [System.NonSerialized]
        private Behaviour type;

        public SharedBool Enabled = new SharedBool();

        public override void Setup(Object type)
        {
            Behaviour component = (Behaviour)type;
            Enabled.Setup(component.enabled);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            Behaviour component = (Behaviour)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.enabled = Enabled.PC;
                    break;
                case BuildTarget.Android:
                    component.enabled = Enabled.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

