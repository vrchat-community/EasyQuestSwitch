#if UNITY_EDITOR && BAKERY_INCLUDED
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Bakery_LightMesh : Type_Base
    {
        [System.NonSerialized]
        private BakeryLightMesh type;

        public SharedFloat intensity = new SharedFloat();
        public SharedColor color = new SharedColor();
        public SharedInt samplesNear = new SharedInt();
        public SharedInt samplesFar = new SharedInt();
        public SharedBool selfShadow = new SharedBool();
        public SharedFloat indirectIntensity = new SharedFloat();

        public override void Setup(Object type)
        {
            
         //   base.Setup(type);
            BakeryLightMesh component = (BakeryLightMesh)type;
            intensity.Setup(component.intensity);
            color.Setup(component.color);
            samplesNear.Setup(component.samples);
            samplesFar.Setup(component.samples2);
            selfShadow.Setup(component.selfShadow);
            indirectIntensity.Setup(component.indirectIntensity);

        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
           // base.Process(type, buildTarget);
            BakeryLightMesh component = (BakeryLightMesh)type;
            component.intensity = intensity.Get(buildTarget);
            component.color = color.Get(buildTarget);
            component.samples = samplesNear.Get(buildTarget);
            component.samples2 = samplesFar.Get(buildTarget);
            component.selfShadow = selfShadow.Get(buildTarget);
            component.indirectIntensity = indirectIntensity.Get(buildTarget);

        }
    }
}
#endif
