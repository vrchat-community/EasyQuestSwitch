#if UNITY_EDITOR && BAKERY_INCLUDED
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Bakery_DirectLight : Type_Base
    {
        [System.NonSerialized]
        private BakeryDirectLight type;

        public SharedColor color = new SharedColor();
        public SharedFloat intensity = new SharedFloat();
        public SharedFloat shadowSpread = new SharedFloat();
        public SharedInt shadowSamples = new SharedInt();
        public SharedFloat indirectIntensity = new SharedFloat();
        public SharedBool antiAlias = new SharedBool();


        public override void Setup(Object type)
        {

            //   base.Setup(type);
            BakeryDirectLight component = (BakeryDirectLight)type;
            color.Setup(component.color);
            intensity.Setup(component.intensity);
            shadowSpread.Setup(component.shadowSpread);
            shadowSamples.Setup(component.samples);
            indirectIntensity.Setup(component.indirectIntensity);
            antiAlias.Setup(component.supersample);

        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            // base.Process(type, buildTarget);
            BakeryDirectLight component = (BakeryDirectLight)type;
            component.color = color.Get(buildTarget);
            component.intensity = intensity.Get(buildTarget);
            component.shadowSpread = shadowSpread.Get(buildTarget);
            component.samples = shadowSamples.Get(buildTarget);
            component.indirectIntensity = indirectIntensity.Get(buildTarget);
            component.supersample = antiAlias.Get(buildTarget);


        }
    }
}
#endif