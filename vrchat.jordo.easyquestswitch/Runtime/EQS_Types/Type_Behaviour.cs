#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
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
            component.enabled = Enabled.Get(buildTarget);
        }
    }
}
#endif

