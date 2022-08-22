#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public abstract class Type_Base : MonoBehaviour
    {
        public abstract void Setup(Object type);
        public abstract void Process(Object type, BuildTarget buildTarget);
    }
}
#endif