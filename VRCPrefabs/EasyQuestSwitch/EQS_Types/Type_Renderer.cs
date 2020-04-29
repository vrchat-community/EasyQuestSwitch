#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_Renderer : Type_Base
    {
        [System.NonSerialized]
        private Renderer type;

        public SharedBool Enabled = new SharedBool();
        public SharedLightProbeUsage LightProbes = new SharedLightProbeUsage();
        public SharedTransform AnchorOverride = new SharedTransform();
        public SharedReflectionProbeUsage ReflectionProbes = new SharedReflectionProbeUsage();
        public SharedShadowCastingMode CastShadows = new SharedShadowCastingMode();
        public SharedBool ReceiveShadows = new SharedBool();
        public SharedBool DynamicOccluded = new SharedBool();
        public List<SharedMaterial> Materials = new List<SharedMaterial>();

        public override void Setup(Object type)
        {
            Renderer component = (Renderer)type;
            Enabled.Setup(component.enabled);
            LightProbes.Setup(component.lightProbeUsage);
            AnchorOverride.Setup(component.probeAnchor);
            ReflectionProbes.Setup(component.reflectionProbeUsage);
            CastShadows.Setup(component.shadowCastingMode);
            ReceiveShadows.Setup(component.receiveShadows);
            DynamicOccluded.Setup(component.allowOcclusionWhenDynamic);
            for (int i = 0; i < component.sharedMaterials.Length; i++)
            {
                Materials.Add(new SharedMaterial());
                Materials[i].Setup(component.sharedMaterials[i]);
            }
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {

            Renderer component = (Renderer)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.enabled = Enabled.PC;
                    component.lightProbeUsage = LightProbes.PC;
                    component.probeAnchor = AnchorOverride.PC;
                    component.reflectionProbeUsage = ReflectionProbes.PC;
                    component.shadowCastingMode = CastShadows.PC;
                    component.receiveShadows = ReceiveShadows.PC;
                    component.allowOcclusionWhenDynamic = DynamicOccluded.PC;
                    for (int i = 0; i < component.sharedMaterials.Length; i++)
                    {
                        component.sharedMaterials = Materials.Select(x => x.PC).ToArray();
                    }
                    break;
                case BuildTarget.Android:
                    component.enabled = Enabled.Quest;
                    component.lightProbeUsage = LightProbes.Quest;
                    component.probeAnchor = AnchorOverride.Quest;
                    component.reflectionProbeUsage = ReflectionProbes.Quest;
                    component.shadowCastingMode = CastShadows.Quest;
                    component.receiveShadows = ReceiveShadows.Quest;
                    component.allowOcclusionWhenDynamic = DynamicOccluded.Quest;
                    for (int i = 0; i < component.sharedMaterials.Length; i++)
                    {
                        component.sharedMaterials = Materials.Select(x => x.Quest).ToArray();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

