using UnityEngine;
using UnityEngine.Events;

namespace QuickEye.EventSystem
{
    public abstract class GameEvent<TArgs> : GameEventBase, IInvokable
    {
        public class Hub : MonoBehaviour
        {
            public TArgs val;
        }
        [SerializeField]
        TArgs _lastPayload;

        [SerializeField]
        UnityEvent<TArgs> _event = new UnityEvent<TArgs>();

        public UnityEvent<TArgs> Event => _event;
        public TArgs LastPayload => _lastPayload;

        public void Subscribe(UnityAction<TArgs> callback)
        {
            Event.AddListener(callback);
#if UNITY_EDITOR
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
#endif
        }

        public void Unsubscribe(UnityAction<TArgs> callback)
        {
            Event.RemoveListener(callback);
#if UNITY_EDITOR
            //UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
#endif
        }

        public void Invoke(TArgs payload)
        {
            Event?.Invoke(_lastPayload = payload);
            wasInvoked = true;
        }
        protected override void ResetValues()
        {
            base.ResetValues();
            _event = new UnityEvent<TArgs>();
            _lastPayload = default;
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
            Event.AddListener(callback);
#if UNITY_EDITOR
            //UnityEditor.Events.UnityEventTools.AddPersistentListener(Event, callback);
#else
#endif
        }

        public void Unsubscribe(UnityAction callback)
        {
            Event.RemoveListener(callback);
#if UNITY_EDITOR
            //UnityEditor.Events.UnityEventTools.RemovePersistentListener(Event, callback);
#else
#endif
        }

        public void Invoke()
        {
            Event?.Invoke();
            wasInvoked = true;
        }

        protected override void ResetValues()
        {
            base.ResetValues();
            _event = new UnityEvent();
        }

        void IInvokable.RepeatLastInvoke() => Invoke();
    }
}