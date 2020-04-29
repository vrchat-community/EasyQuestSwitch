#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_Material : Type_Base
    {
        [System.NonSerialized]
        private Material type;

        public SharedShader Shader = new SharedShader();

        public override void Setup(Object type)
        {
            Material material = (Material)type;
            Shader.Setup(material.shader);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            Material material = (Material)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    material.shader = Shader.PC;
                    break;
                case BuildTarget.Android:
                    material.shader = Shader.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

