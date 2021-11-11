#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_SpriteRenderer : Type_Base
    {
        [System.NonSerialized]
        private SpriteRenderer type;

        public SharedSprite Sprite = new SharedSprite();
        public SharedColor Color = new SharedColor();
        public SharedMaterial Material = new SharedMaterial();

        public override void Setup(Object type)
        {
            SpriteRenderer component = (SpriteRenderer)type;
            Sprite.Setup(component.sprite);
            Color.Setup(component.color);
            Material.Setup(component.material);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            SpriteRenderer component = (SpriteRenderer)type;
            component.sprite = Sprite.Get(buildTarget);
            component.color = Color.Get(buildTarget);
            component.material = Material.Get(buildTarget);
        }
    }
}
#endif

