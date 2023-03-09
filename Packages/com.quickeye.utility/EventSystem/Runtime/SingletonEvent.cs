using QuickEye.Utility;
using UnityEngine.Events;

namespace QuickEye.EventSystem
{
    public class SingletonEvent<T> : GameEvent where T : SingletonEvent<T>
    {
        private static T _instance;

        public static T Instance => _instance != null
            ? _instance
            : _instance = SingletonScriptableObjectFactory.LoadOrCreateInstance<T>();

        public new static void Subscribe(UnityAction callback)
        {
            ((GameEvent)Instance).Subscribe(callback);
        }

        public new static void Unsubscribe(UnityAction callback)
        {
            ((GameEvent)Instance).Unsubscribe(callback);
        }

        public new static void Invoke()
        {
            ((GameEvent)Instance).Invoke();
        }
    }

    public abstract class SingletonEvent<T, TArg> : GameEvent<TArg> where T : SingletonEvent<T, TArg>
    {
        private static T _instance;

        public static T Instance => _instance != null
            ? _instance
            : _instance = SingletonScriptableObjectFactory.LoadOrCreateInstance<T>();

        public new static bool WasInvoked => ((GameEvent<TArg>)Instance).WasInvoked;
        public new static TArg LastPayload => ((GameEvent<TArg>)Instance).LastPayload;
        public new static void Subscribe(UnityAction<TArg> callback)
        {
            ((GameEvent<TArg>)Instance).Subscribe(callback);
        }

        public new static void Unsubscribe(UnityAction<TArg> callback)
        {
            ((GameEvent<TArg>)Instance).Unsubscribe(callback);
        }

        public new static void Invoke(TArg payload)
        {
            ((GameEvent<TArg>)Instance).Invoke(payload);
        }
    }
}