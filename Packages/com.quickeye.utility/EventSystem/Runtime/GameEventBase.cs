using UnityEngine;

namespace QuickEye.EventSystem
{
    public abstract class GameEventBase : ScriptableObject
    {
#if UNITY_EDITOR
        [TextArea]
        [SerializeField]
        protected internal string developerDescription;
#endif
        protected bool wasInvoked;
        public bool WasInvoked => wasInvoked;
    }
    
    // just for polymorphic method invocation in Editor 
    internal interface IInvokable
    {
        void RepeatLastInvoke();
    }
}