#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_Material : Type_Base
    {
        [System.NonSerialized]
        private Material type;

        public SharedShader Shader = new SharedShader();
        private SharedString ShaderPath = new SharedString();

        public override void Setup(Object type)
        {
            Material material = (Material)type;
            Shader.Setup(material.shader);
            ShaderPath.Setup(material.shader.name);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            Material material = (Material)type;
            if(Shader.Get(buildTarget) != null)
            {
                switch (buildTarget)
                {
                    case BuildTarget.StandaloneWindows:
                        ShaderPath.PC = Shader.Get(buildTarget).name;
                        break;
                    case BuildTarget.Android:
                        ShaderPath.Quest = Shader.Get(buildTarget).name;
                        break;
                }
            }
            else if(Shader.Get(buildTarget) == null && !string.IsNullOrEmpty(ShaderPath.Get(buildTarget)))
            {
                switch(buildTarget)
                {
                    case BuildTarget.StandaloneWindows:
                        Shader.PC = UnityEngine.Shader.Find(ShaderPath.PC);
                        break;
                    case BuildTarget.Android:
                        Shader.Quest = UnityEngine.Shader.Find(ShaderPath.Quest);
                        break;
                }
            }

            if(Shader.Get(buildTarget) == null)
            {
                throw new MissingReferenceException();
            }
            material.shader = Shader.Get(buildTarget);
        }
    }
}
#endif

