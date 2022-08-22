#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEngine.UI;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Image : Type_Behaviour
    {
        [System.NonSerialized]
        private Image type;

        public SharedSprite Sprite = new SharedSprite();
        public SharedColor Color = new SharedColor();
        public SharedMaterial Material = new SharedMaterial();

        public override void Setup(Object type)
        {
            base.Setup(type);
            Image component = (Image)type;
            Sprite.Setup(component.sprite);
            Color.Setup(component.color);
            Material.Setup(component.material);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            Image component = (Image)type;
            component.sprite = Sprite.Get(buildTarget);
            component.color = Color.Get(buildTarget);
            component.material = Material.Get(buildTarget);
        }
    }
}
#endif

