#if UNITY_EDITOR && BAKERY_INCLUDED
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Bakery_PointLight : Type_Base
    {
        [System.NonSerialized]
        private BakeryPointLight type;

        public SharedColor color = new SharedColor();
        public SharedFloat intensity = new SharedFloat();
        public SharedFloat shadowSpread = new SharedFloat();
        public SharedBool physicalFalloff = new SharedBool();
        public SharedFloat range = new SharedFloat();
        public SharedInt samples = new SharedInt();
        public SharedBool legacySampling = new SharedBool();
        public SharedFloat indirectIntensity = new SharedFloat();


        public override void Setup(Object type)
        {
            
         //   base.Setup(type);
            BakeryPointLight component = (BakeryPointLight)type;
            color.Setup(component.color);
            intensity.Setup(component.intensity);
            shadowSpread.Setup(component.shadowSpread);
            physicalFalloff.Setup(component.realisticFalloff);
            range.Setup(component.cutoff);
            samples.Setup(component.samples);
            legacySampling.Setup(component.legacySampling);
            indirectIntensity.Setup(component.indirectIntensity);

        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
           // base.Process(type, buildTarget);
            BakeryPointLight component = (BakeryPointLight)type;
            component.color = color.Get(buildTarget);
            component.intensity = intensity.Get(buildTarget);
            component.shadowSpread = shadowSpread.Get(buildTarget);
            component.realisticFalloff = physicalFalloff.Get(buildTarget);
            component.cutoff = range.Get(buildTarget);
            component.samples = samples.Get(buildTarget);
            component.legacySampling = legacySampling.Get(buildTarget);
            component.indirectIntensity = indirectIntensity.Get(buildTarget);


        }
    }
}
#endif
