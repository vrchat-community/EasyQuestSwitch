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

        public override void Setup(Object type)
        {
            base.Setup(type);
            AudioSource component = (AudioSource)type;
            AudioClip.Setup(component.clip);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            AudioSource component = (AudioSource)type;
            component.clip = AudioClip.Get(buildTarget);
        }
    }
}
#endif

