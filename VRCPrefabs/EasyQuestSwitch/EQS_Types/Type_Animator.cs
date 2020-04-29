#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_Animator : Type_Behaviour
    {
        [System.NonSerialized]
        private Animator type;

        public SharedRuntimeAnimatorController Controller = new SharedRuntimeAnimatorController();
        public SharedAnimatorCullingMode CullingMode = new SharedAnimatorCullingMode();

        public override void Setup(Object type)
        {
            base.Setup(type);
            Animator component = (Animator)type;
            if(component.runtimeAnimatorController != null) Controller.Setup(component.runtimeAnimatorController);
            CullingMode.Setup(component.cullingMode);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            Animator component = (Animator)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.runtimeAnimatorController = Controller.PC;
                    component.cullingMode = CullingMode.PC;
                    break;
                case BuildTarget.Android:
                    component.runtimeAnimatorController = Controller.Quest;
                    component.cullingMode = CullingMode.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif
