#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using EasyQuestSwitch.Fields;
using UnityEngine.UI;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public class Type_CanvasScaler : Type_Behaviour
    {
        [System.NonSerialized]
        private CanvasScaler type;

        public SharedFloat ScaleFactor = new SharedFloat();
        public SharedFloat ReferencePixelsPerUnit = new SharedFloat();

        public override void Setup(Object type)
        {
            base.Setup(type);
            CanvasScaler component = (CanvasScaler)type;
            ScaleFactor.Setup(component.scaleFactor);
            ReferencePixelsPerUnit.Setup(component.referencePixelsPerUnit);
        }

        public override void Process(Object type, BuildTarget buildTarget)
        {
            base.Process(type, buildTarget);
            CanvasScaler component = (CanvasScaler)type;
            component.scaleFactor = ScaleFactor.Get(buildTarget);
            component.referencePixelsPerUnit = ReferencePixelsPerUnit.Get(buildTarget);
        }
    }
}
#endif

