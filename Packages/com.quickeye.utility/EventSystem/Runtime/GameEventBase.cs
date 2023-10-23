using System;
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
        [SerializeField]
        protected bool wasInvoked;

        public bool WasInvoked => wasInvoked;
        
        protected virtual void OnEnable() => ResetValues();
        protected virtual void OnDisable() => ResetValues();

        protected virtual void ResetValues()
        {
            wasInvoked = false;
        }
    }

    // just for polymorphic method invocation in Editor 
    internal interface IInvokable
    {
        void RepeatLastInvoke();
    }
    
    
}