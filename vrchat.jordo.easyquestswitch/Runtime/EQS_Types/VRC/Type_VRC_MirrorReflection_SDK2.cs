#if UNITY_EDITOR && VRC_SDK_VRCSDK2
using UnityEditor;
using UnityEngine;
using VRCSDK2;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_VRC_MirrorReflection : Type_Behaviour
    {
        [System.NonSerialized]
        private VRC_MirrorReflection type;

        public SharedBool DisablePixelLights = new SharedBool();
        public SharedBool TurnOffMirrorOcclusion = new SharedBool();
        public SharedLayerMask ReflectLayers = new SharedLayerMask();

        public override void Setup(Object type)
        {
            base.Setup(type);
            VRC_MirrorReflection component = (VRC_MirrorReflection)type;
            DisablePixelLights.Setup(component.m_DisablePixelLights);
            TurnOffMirrorOcclusion.Setup(component.TurnOffMirrorOcclusion);
            ReflectLayers.Setup(component.m_ReflectLayers);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            VRC_MirrorReflection component = (VRC_MirrorReflection)type;
            component.m_DisablePixelLights = DisablePixelLights.Get(buildTarget);
            component.TurnOffMirrorOcclusion = TurnOffMirrorOcclusion.Get(buildTarget);
            component.m_ReflectLayers = ReflectLayers.Get(buildTarget);
        }
    }
}
#endif
