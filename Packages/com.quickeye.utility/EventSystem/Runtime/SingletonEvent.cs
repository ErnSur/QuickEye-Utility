using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace QuickEye.Utility
{
    public class SingletonEvent<T> : GameEvent where T : SingletonEvent<T>
    {
        private static T _instance;

        public static T Instance => _instance != null
            ? _instance
            : _instance = SingletonScriptableObjectFactory.LoadOrCreateInstance<T>();

        public new static void Register(UnityAction callback)
        {
            ((GameEvent)Instance).Register(callback);
        }

        public new static void Unregister(UnityAction callback)
        {
            ((GameEvent)Instance).Unregister(callback);
        }

        public new static void Trigger()
        {
            ((GameEvent)Instance).Trigger();
        }
    }

    public abstract class SingletonEvent<T, TArg> : GameEvent<TArg> where T : SingletonEvent<T, TArg>
    {
        private static T _instance;

        public static T Instance => _instance != null
            ? _instance
            : _instance = SingletonScriptableObjectFactory.LoadOrCreateInstance<T>();

        public new static bool WasRaised => ((GameEvent<TArg>)Instance).WasRaised;
        public new static TArg LastPayload => ((GameEvent<TArg>)Instance).LastPayload;
        public new static void Register(UnityAction<TArg> callback)
        {
            ((GameEvent<TArg>)Instance).Register(callback);
        }

        public new static void Unregister(UnityAction<TArg> callback)
        {
            ((GameEvent<TArg>)Instance).Unregister(callback);
        }

        public new static void Trigger(TArg payload)
        {
            ((GameEvent<TArg>)Instance).Trigger(payload);
        }
    }
}