#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEngine.UI;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_RawImage : Type_Behaviour
    {
        [System.NonSerialized]
        private RawImage type;

        public SharedTexture Texture = new SharedTexture();
        public SharedColor Color = new SharedColor();
        public SharedMaterial Material = new SharedMaterial();

        public override void Setup(Object type)
        {
            base.Setup(type);
            RawImage component = (RawImage)type;
            Texture.Setup(component.texture);
            Color.Setup(component.color);
            Material.Setup(component.material);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            RawImage component = (RawImage)type;
            component.texture = Texture.Get(buildTarget);
            component.color = Color.Get(buildTarget);
            component.material = Material.Get(buildTarget);
        }
    }
}
#endif

