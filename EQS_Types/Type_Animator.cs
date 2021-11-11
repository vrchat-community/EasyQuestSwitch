#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
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
            component.runtimeAnimatorController = Controller.Get(buildTarget);
            component.cullingMode = CullingMode.Get(buildTarget);
        }
    }
}
#endif
