#if UNITY_EDITOR
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEditor;

namespace EasyQuestSwitch.Types
{
    public class Type_Camera : Type_Behaviour
    {
        [System.NonSerialized]
        private Camera type;

        public SharedFloat ClippingPlaneNear = new SharedFloat();
        public SharedFloat ClippingPlaneFar = new SharedFloat();

        public override void Setup(Object type)
        {
            base.Setup(type);
            Camera component = (Camera)type;
            ClippingPlaneNear.Setup(component.nearClipPlane);
            ClippingPlaneFar.Setup(component.farClipPlane);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            Camera component = (Camera)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.nearClipPlane = ClippingPlaneNear.PC;
                    component.farClipPlane = ClippingPlaneFar.PC;
                    break;
                case BuildTarget.Android:
                    component.nearClipPlane = ClippingPlaneNear.Quest;
                    component.farClipPlane = ClippingPlaneFar.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif