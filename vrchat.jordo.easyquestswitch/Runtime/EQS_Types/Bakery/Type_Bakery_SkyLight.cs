#if UNITY_EDITOR && BAKERY_INCLUDED
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Bakery_SkyLight : Type_Behaviour
    {
        [System.NonSerialized]
        private BakerySkyLight type;

        public SharedColor color = new SharedColor();
        public SharedFloat intensity = new SharedFloat();
        public SharedInt samples = new SharedInt();
        public SharedBool hemispherical = new SharedBool();


        public override void Setup(Object type)
        {
            base.Setup(type);
            BakerySkyLight component = (BakerySkyLight)type;
            color.Setup(component.color);
            intensity.Setup(component.intensity);
            samples.Setup(component.samples);
            hemispherical.Setup(component.hemispherical);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            BakerySkyLight component = (BakerySkyLight)type;
            component.color = color.Get(buildTarget);
            component.intensity = intensity.Get(buildTarget);
            component.samples = samples.Get(buildTarget);
            component.hemispherical = hemispherical.Get(buildTarget);
        }
    }
}
#endif
