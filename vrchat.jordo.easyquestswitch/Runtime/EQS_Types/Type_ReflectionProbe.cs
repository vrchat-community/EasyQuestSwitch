#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEngine.UI;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_ReflectionProbe : Type_Behaviour
    {
        [System.NonSerialized]
        private ReflectionProbe type;

        public SharedReflectionProbeMode ReflectionProbeType = new SharedReflectionProbeMode();
        public SharedTexture Cubemap = new SharedTexture();
        public SharedInt Importance = new SharedInt();
        public SharedFloat Intensity = new SharedFloat();
        public SharedBool BoxProjection = new SharedBool();
        public SharedReflectionProbeResolution Resolution = new SharedReflectionProbeResolution();

        public override void Setup(Object type)
        {
            base.Setup(type);
            ReflectionProbe component = (ReflectionProbe)type;
            ReflectionProbeType.Setup(component.mode);
            Cubemap.Setup(component.customBakedTexture);
            Importance.Setup(component.importance);
            Intensity.Setup(component.intensity);
            BoxProjection.Setup(component.boxProjection);
            Resolution.Setup((ReflectionProbeResolution)component.resolution);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            ReflectionProbe component = (ReflectionProbe)type;
            component.mode = ReflectionProbeType.Get(buildTarget);
            component.customBakedTexture = Cubemap.Get(buildTarget);
            component.importance = Importance.Get(buildTarget);
            component.intensity = Intensity.Get(buildTarget);
            component.boxProjection = BoxProjection.Get(buildTarget);
            component.resolution = (int)Resolution.Get(buildTarget);
        }
    }
}
#endif

