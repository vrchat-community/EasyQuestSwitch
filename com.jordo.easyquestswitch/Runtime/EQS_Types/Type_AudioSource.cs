#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_AudioSource : Type_Behaviour
    {
        [System.NonSerialized]
        private AudioSource type;

        public SharedAudioClip AudioClip = new SharedAudioClip();
        public SharedFloat Volume = new SharedFloat();
        public SharedFloat SpatialBlend = new SharedFloat();
        public SharedFloat ReverbZoneMix = new SharedFloat();
        public SharedBool BypassEffects = new SharedBool();
        public SharedBool BypassListenreEffects = new SharedBool();
        public SharedBool BypassReverbZones = new SharedBool();

        public override void Setup(Object type)
        {
            base.Setup(type);
            AudioSource component = (AudioSource)type;
            AudioClip.Setup(component.clip);
            Volume.Setup(component.volume);
            SpatialBlend.Setup(component.spatialBlend);
            ReverbZoneMix.Setup(component.reverbZoneMix);
            BypassEffects.Setup(component.bypassEffects);
            BypassListenreEffects.Setup(component.bypassListenerEffects);
            BypassReverbZones.Setup(component.bypassReverbZones);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            AudioSource component = (AudioSource)type;
            component.clip = AudioClip.Get(buildTarget);
            component.volume = Volume.Get(buildTarget);
            component.spatialBlend = SpatialBlend.Get(buildTarget);
            component.reverbZoneMix = ReverbZoneMix.Get(buildTarget);
            component.bypassEffects = BypassEffects.Get(buildTarget);
            component.bypassListenerEffects = BypassListenreEffects.Get(buildTarget);
            component.bypassReverbZones = BypassReverbZones.Get(buildTarget);
        }
    }
}
#endif

