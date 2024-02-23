#if UNITY_EDITOR
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEditor;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Light : Type_Behaviour
    {
        [System.NonSerialized]
        private Light type;

        public SharedLightType LightType = new SharedLightType();
        public SharedFloat Range = new SharedFloat();
        public SharedColor Color = new SharedColor();
        public SharedLightmapBakeType Mode = new SharedLightmapBakeType();
        public SharedFloat Intensity = new SharedFloat();
        public SharedFloat IndirectMultiplier = new SharedFloat();
        public SharedLightShadows ShadowType = new SharedLightShadows();
        public SharedLightRenderMode RenderMode = new SharedLightRenderMode();

        public override void Setup(Object type)
        {
            base.Setup(type);
            Light component = (Light)type;
            LightType.Setup(component.type);
            Range.Setup(component.range);
            Color.Setup(component.color);
            Mode.Setup(component.lightmapBakeType);
            Intensity.Setup(component.intensity);
            IndirectMultiplier.Setup(component.bounceIntensity);
            ShadowType.Setup(component.shadows);
            RenderMode.Setup(component.renderMode);
        }

        public override void Setup(Object type, int currentVersion)
        {
            Light component = (Light)type;
            if (currentVersion == 0) // 0 -> 131 upgrade
            {
                LightType.Setup(component.type);
                Range.Setup(component.range);
                RenderMode.Setup(component.renderMode);
            }
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            Light component = (Light)type;
            component.type = LightType.Get(buildTarget);
            component.range = Range.Get(buildTarget);
            component.color = Color.Get(buildTarget);
            component.lightmapBakeType = Mode.Get(buildTarget);
            component.intensity = Intensity.Get(buildTarget);
            component.bounceIntensity = IndirectMultiplier.Get(buildTarget);
            component.shadows = ShadowType.Get(buildTarget);
            component.renderMode = RenderMode.Get(buildTarget);
        }
    }
}
#endif