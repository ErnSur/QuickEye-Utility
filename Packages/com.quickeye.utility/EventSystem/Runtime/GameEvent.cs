using UnityEngine;
using UnityEngine.Events;

namespace QuickEye.EventSystem
{
    public abstract class GameEvent<TArgs> : GameEventBase, IInvokable
    {
        [SerializeField]
        TArgs _lastPayload;

        [SerializeField]
        UnityEvent<TArgs> _event = new UnityEvent<TArgs>();

        public UnityEvent<TArgs> Event => _event;
        public TArgs LastPayload => _lastPayload;

        public void Subscribe(UnityAction<TArgs> callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
            Event.AddListener(callback);
#endif
        }

        public void Unsubscribe(UnityAction<TArgs> callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
            Event.RemoveListener(callback);
#endif
        }

        public void Invoke(TArgs payload)
        {
            Event?.Invoke(_lastPayload = payload);
            wasInvoked = true;
        }

        void IInvokable.RepeatLastInvoke() => Invoke(_lastPayload);
    }

    public abstract class GameEvent : GameEventBase, IInvokable
    {
        [SerializeField]
        UnityEvent _event = new UnityEvent();

        public UnityEvent Event => _event;

        public void Subscribe(UnityAction callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
            Event.AddListener(callback);
#endif
        }

        public void Unsubscribe(UnityAction callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
            Event.RemoveListener(callback);
#endif
        }

        public void Invoke()
        {
            Event?.Invoke();
            wasInvoked = true;
        }

        void IInvokable.RepeatLastInvoke() => Invoke();
    }
}