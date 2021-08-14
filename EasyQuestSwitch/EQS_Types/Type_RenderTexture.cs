#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEngine.Experimental.Rendering;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_RenderTexture : Type_Base
    {
        [System.NonSerialized]
        private RenderTexture type;

        public SharedVector2Int Size = new SharedVector2Int();
        public SharedAntiAliasing AntiAliasing = new SharedAntiAliasing();
        public SharedDepthBuffer DepthBuffer = new SharedDepthBuffer();
        public SharedBool EnableMipMaps = new SharedBool();
        public SharedFilterMode FilterMode = new SharedFilterMode();

        public override void Setup(Object type)
        {
            RenderTexture component = (RenderTexture)type;
            Size.Setup(new Vector2Int(component.width, component.height));
            AntiAliasing.Setup((AntiAliasing)component.antiAliasing);
            DepthBuffer.Setup((DepthBuffer)component.depth);
            EnableMipMaps.Setup(component.useMipMap);
            FilterMode.Setup(component.filterMode);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            RenderTexture component = (RenderTexture)type;
            component.Release();
            component.width = Size.Get(buildTarget).x;
            component.height = Size.Get(buildTarget).y;
            component.antiAliasing = (int)AntiAliasing.Get(buildTarget);
            component.depth = (int)DepthBuffer.Get(buildTarget);
            component.useMipMap = EnableMipMaps.Get(buildTarget);
            component.filterMode = FilterMode.Get(buildTarget);
        }
    }
}
#endif

