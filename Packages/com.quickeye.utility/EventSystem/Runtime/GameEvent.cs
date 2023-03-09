using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace QuickEye.Utility
{
    public abstract class GameEvent<TArgs> : GameEventBase
    {
        [SerializeField]
        TArgs _lastPayload;

        [SerializeField]
        UnityEvent<TArgs> _event = new UnityEvent<TArgs>();

        public UnityEvent<TArgs> Event => _event;
        public TArgs LastPayload => _lastPayload;

        public void Register(UnityAction<TArgs> callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
            Event.AddListener(callback);
#endif
        }

        public void Unregister(UnityAction<TArgs> callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
            Event.RemoveListener(callback);
#endif
        }

        public void Trigger(TArgs payload)
        {
            Event?.Invoke(_lastPayload = payload);
            WasRaised = true;
        }
    }

    public abstract class GameEvent : GameEventBase
    {
        [SerializeField]
        UnityEvent _event = new UnityEvent();
        public UnityEvent Event => _event;

        public void Register(UnityAction callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
            Event.AddListener(callback);
#endif
        }

        public void Unregister(UnityAction callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
            Event.RemoveListener(callback);
#endif
        }

        public void Trigger()
        {
            Event?.Invoke();
            WasRaised = true;
        }
    }
}