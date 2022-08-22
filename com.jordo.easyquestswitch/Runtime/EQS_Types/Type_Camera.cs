#if UNITY_EDITOR
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEditor;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Camera : Type_Behaviour
    {
        [System.NonSerialized]
        private Camera type;

        public SharedFloat ClippingPlaneNear = new SharedFloat();
        public SharedFloat ClippingPlaneFar = new SharedFloat();
        public SharedRenderTexture TargetTexture = new SharedRenderTexture();
        public SharedBool OcclusionCulling = new SharedBool();

        public override void Setup(Object type)
        {
            base.Setup(type);
            Camera component = (Camera)type;
            ClippingPlaneNear.Setup(component.nearClipPlane);
            ClippingPlaneFar.Setup(component.farClipPlane);
            TargetTexture.Setup(component.targetTexture);
            OcclusionCulling.Setup(component.useOcclusionCulling);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            Camera component = (Camera)type;
            component.nearClipPlane = ClippingPlaneNear.Get(buildTarget);
            component.farClipPlane = ClippingPlaneFar.Get(buildTarget);
            component.targetTexture = TargetTexture.Get(buildTarget);
            component.useOcclusionCulling = OcclusionCulling.Get(buildTarget);
        }
    }
}
#endif