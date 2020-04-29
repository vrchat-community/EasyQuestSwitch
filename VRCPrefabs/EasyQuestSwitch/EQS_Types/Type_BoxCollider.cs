#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;

namespace EasyQuestSwitch.Types
{
    public class Type_BoxCollider : Type_Collider
    {
        [System.NonSerialized]
        private BoxCollider type;

        public SharedVector3 Size = new SharedVector3();
        public SharedVector3 Center = new SharedVector3();

        public override void Setup(Object type)
        {
            base.Setup(type);
            BoxCollider component = (BoxCollider)type;
            Size.Setup(component.size);
            Center.Setup(component.center);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            BoxCollider component = (BoxCollider)type;
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    component.size = Size.PC;
                    component.center = Center.PC;
                    break;
                case BuildTarget.Android:
                    component.size = Size.Quest;
                    component.center = Center.Quest;
                    break;
                default:
                    break;
            }
        }
    }
}
#endif

