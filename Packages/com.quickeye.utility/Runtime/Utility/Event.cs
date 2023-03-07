using System;
using UnityEngine;
using UnityEngine.Events;

namespace QuickEye.Utility
{
    public abstract class Event<TEvent> where TEvent : Event<TEvent>
    {
        public static event Action Ev;
        // ReSharper disable once StaticMemberInGenericType
        public static bool WasTriggered { get; private set; }
        public static void Register(Action callback) => Ev += callback;
        public static void Unregister(Action callback) => Ev -= callback;
        public static void Trigger()
        {
            Ev?.Invoke();
            WasTriggered = true;
        }
    }

    public abstract class Event<TEvent, TArgs> where TEvent : Event<TEvent, TArgs>
    {
        public static event Action<TArgs> Ev;
        public static TArgs LastPayload { get; private set; }
        // ReSharper disable once StaticMemberInGenericType
        public static bool WasTriggered { get; private set; }

        public static void Register(Action<TArgs> callback) => Ev += callback;
        public static void Unregister(Action<TArgs> callback) => Ev -= callback;
        public static void Trigger(TArgs payload)
        {
            Ev?.Invoke(LastPayload = payload);
            WasTriggered = true;
        }
    }
    
    // use static version of the class
    // editor to show all events can be created without the use of so
    // get all events with reflection
    public abstract class GlobalUnityEvent<T, TArgs> : SingletonScriptableObject<T>, IEvent where T : GlobalUnityEvent<T, TArgs>
    {
        //For easie
        [SerializeField]
        // If payload would be SO itself we would have to create new so for each event
        private TArgs _lastPayload;

        [SerializeField]
        private UnityEvent<TArgs> _event = new UnityEvent<TArgs>();

        public static UnityEvent<TArgs> Event => Instance._event;
        public TArgs LastPayload => _lastPayload;

        public static void Register(UnityAction<TArgs> callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
            Event.AddListener(callback);
#endif
        }

        public static void Unregister(UnityAction<TArgs> callback)
        {
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
//in Runtime SingletonScriptableObject can be removed completely
            Event.RemoveListener(callback);
#endif
        }

        public static void Trigger(TArgs payload) => Event?.Invoke(Instance._lastPayload = payload);
    }
    
    interface IEvent{}
}