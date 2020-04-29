#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
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
    }

    [Serializable]
    public class SharedBool : SharedObject<bool> {}
    [Serializable]
    public class SharedInt : SharedObject<int> { }
    [Serializable]
    public class SharedVector3 : SharedObject<Vector3> { }
    [Serializable]
    public class SharedString : SharedObject<string> { }
    [Serializable]
    public class SharedFloat : SharedObject<float> { }
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

    #region GameObject
    [Serializable]
    public class SharedTag : SharedObject<string> { }
    [Serializable]
    public class SharedLayer : SharedObject<int> { }
    [Serializable]
    public class SharedStaticEditorFlags : SharedObject<StaticEditorFlags> { }
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

}
#endif
