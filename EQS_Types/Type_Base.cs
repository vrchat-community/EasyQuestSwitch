#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EasyQuestSwitch.Types
{
    [AddComponentMenu("")]
    public abstract class Type_Base : MonoBehaviour
    {
        public abstract void Setup(Object type);
        
        // Override without calling base to setup custom migration
        public virtual void Setup(Object type, int currentVersion) => Setup(type);
        
        public abstract void Process(Object type, BuildTarget buildTarget);
    }
}
#endif