#if UNITY_EDITOR && VRC_SDK_VRCSDK3
using UnityEditor;
using UnityEngine;
#if UDON
using VRC.SDK3.Components;
#else
using VRC.SDK3.Avatars.Components;
#endif
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_VRC_SpatialAudioSource : Type_Base
    {
        [System.NonSerialized]
        private VRCSpatialAudioSource type;

        public SharedFloat Gain = new SharedFloat();
        public SharedFloat Far = new SharedFloat();
        public SharedFloat Near = new SharedFloat();
        public SharedFloat VolumetricRadius = new SharedFloat();
        public SharedBool EnableSpatialization = new SharedBool();

        public override void Setup(Object type)
        {
            VRCSpatialAudioSource component = (VRCSpatialAudioSource)type;
            Gain.Setup(component.Gain);
            Far.Setup(component.Far);
            Near.Setup(component.Near);
            VolumetricRadius.Setup(component.VolumetricRadius);
            EnableSpatialization.Setup(component.EnableSpatialization);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            VRCSpatialAudioSource component = (VRCSpatialAudioSource)type;
            component.Gain = Gain.Get(buildTarget);
            component.Far = Far.Get(buildTarget);
            component.Near = Near.Get(buildTarget);
            component.VolumetricRadius = VolumetricRadius.Get(buildTarget);
            component.EnableSpatialization = EnableSpatialization.Get(buildTarget);
        }
    }
}
#endif
