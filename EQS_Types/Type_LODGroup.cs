#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEngine.UI;
using System.Collections.Generic;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_LODGroup : Type_Base
    {
        [System.NonSerialized]
        private LODGroup type;

        public SharedBool Enabled = new SharedBool();
        public SharedFadeMode FadeMode = new SharedFadeMode();
        public SharedBool AnimateCrossFading = new SharedBool();
        public List<SharedFloat> Percentage = new List<SharedFloat>();
        public List<SharedFloat> FadeTransitionWidth = new List<SharedFloat>();

        public override void Setup(Object type)
        {
            LODGroup component = (LODGroup)type;
            Enabled.Setup(component.enabled);
            FadeMode.Setup(component.fadeMode);
            AnimateCrossFading.Setup(component.animateCrossFading);
            LOD[] lods = component.GetLODs();
            Debug.Log(lods.Length);
            for(int i = 0; i < lods.Length; i++)
            {
                Percentage.Add(new SharedFloat());
                Percentage[i].Setup(lods[i].screenRelativeTransitionHeight * 100);
                FadeTransitionWidth.Add(new SharedFloat());
                FadeTransitionWidth[i].Setup(lods[i].fadeTransitionWidth);
            }
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            LODGroup component = (LODGroup)type;
            LOD[] lods = component.GetLODs();
            component.enabled = Enabled.Get(buildTarget);
            component.fadeMode = FadeMode.Get(buildTarget);
            component.animateCrossFading = AnimateCrossFading.Get(buildTarget);
            for (int i = 0; i < lods.Length; i++)
            {
                lods[i].screenRelativeTransitionHeight = Percentage[i].Get(buildTarget) / 100;
                lods[i].fadeTransitionWidth = FadeTransitionWidth[i].Get(buildTarget);
            }
            component.SetLODs(lods);
        }
    }
}
#endif

