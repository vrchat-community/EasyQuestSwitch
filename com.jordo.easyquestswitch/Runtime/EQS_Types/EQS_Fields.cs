#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;


namespace EasyQuestSwitch.Fields
{
    [Serializable]
    public class SharedObject<T>
    {
        public T PC;
        public T Quest;

        public void Setup(T obj)
        {
            PC = Quest = obj;
        }

        public T Get(BuildTarget buildTarget)
        {
            switch(buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    return PC;
                case BuildTarget.Android:
                    return Quest;
                default:
                    return default(T);
            }
        }
    }

    [Serializable]
    public class SharedBool : SharedObject<bool> {}
    [Serializable]
    public class SharedInt : SharedObject<int> {}
    [Serializable]
    public class SharedVector2 : SharedObject<Vector2> {}
    [Serializable]
    public class SharedVector2Int : SharedObject<Vector2Int> {}
    [Serializable]
    public class SharedVector3 : SharedObject<Vector3> {}
    [Serializable]
    public class SharedVector3Int : SharedObject<Vector3Int> {}
    [Serializable]
    public class SharedVector4 : SharedObject<Vector4> {}
    [Serializable]
    public class SharedQuaternion : SharedObject<Quaternion> {}
    [Serializable]
    public class SharedString : SharedObject<string> {}
    [Serializable]
    public class SharedFloat : SharedObject<float> {}
    [Serializable]
    public class SharedShader : SharedObject<Shader> {}
    [Serializable]
    public class SharedMaterial : SharedObject<Material> {}
    [Serializable]
    public class SharedMesh : SharedObject<Mesh> {}
    [Serializable]
    public class SharedTransform : SharedObject<Transform> {}
    [Serializable]
    public class SharedLayerMask : SharedObject<LayerMask> {}
    [Serializable]
    public class SharedRenderer : SharedObject<Renderer> { }

    #region Graphic
    [Serializable]
    public class SharedSprite : SharedObject<Sprite> {}
    [Serializable]
    public class SharedColor : SharedObject<Color> {}
    [Serializable]
    public class SharedRenderTexture : SharedObject<RenderTexture> {}
    [Serializable]
    public class SharedTexture : SharedObject<Texture> {}
    #endregion

    #region GameObject
    [Serializable]
    public class SharedTag : SharedObject<string> {}
    [Serializable]
    public class SharedLayer : SharedObject<int> {}
    [Serializable]
    public class SharedStaticEditorFlags : SharedObject<StaticEditorFlags> {}
    #endregion

    #region Animator
    [Serializable]
    public class SharedRuntimeAnimatorController : SharedObject<RuntimeAnimatorController> {}
    [Serializable]
    public class SharedAnimatorCullingMode : SharedObject<AnimatorCullingMode> {}
    #endregion

    #region Renderer
    [Serializable]
    public class SharedLightProbeUsage : SharedObject<LightProbeUsage> {}
    [Serializable]
    public class SharedReflectionProbeUsage : SharedObject<ReflectionProbeUsage> {}
    [Serializable]
    public class SharedShadowCastingMode : SharedObject<ShadowCastingMode> {}
    #endregion

    #region LODGroup
    [Serializable]
    public class SharedFadeMode : SharedObject<LODFadeMode> {}
    #endregion

    #region ReflectionProbe
    [Serializable]
    public class SharedReflectionProbeMode : SharedObject<ReflectionProbeMode> {}
    [Serializable]
    public class SharedReflectionProbeResolution : SharedObject<ReflectionProbeResolution> {}
    public enum ReflectionProbeResolution : int
    {
        _16 = 16,
        _32 = 32,
        _64 = 64,
        _128 = 128,
        _256 = 256,
        _512 = 512,
        _1024 = 1024,
        _2048 = 2048
    }
    #endregion

    #region AudioClip
    [Serializable]
    public class SharedAudioClip : SharedObject<AudioClip> {}
    #endregion

    #region RenderTexture
    [Serializable]
    public class SharedDepthBuffer : SharedObject<DepthBuffer> {}
    public enum DepthBuffer : int
    {
        _0 = 0,
        _16 = 16,
        _24 = 24,
        _32 = 32
    }
    [Serializable]
    public class SharedAntiAliasing : SharedObject<AntiAliasing> {}
    public enum AntiAliasing : int
    {
        None = 1,
        _2Samples = 2,
        _4Samples = 4,
        _8Samples = 8
    }
    [Serializable]
    public class SharedFilterMode : SharedObject<FilterMode> {}
    #endregion

    #region Light
    [Serializable]
    public class SharedLightmapBakeType : SharedObject<LightmapBakeType> {}
    [Serializable]
    public class SharedLightShadows : SharedObject<LightShadows> {}
    #endregion
}
#endif
