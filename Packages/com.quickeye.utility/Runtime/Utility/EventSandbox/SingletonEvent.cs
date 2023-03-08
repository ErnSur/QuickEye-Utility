using System;

namespace QuickEye.Utility
{
    public class SingletonEvent<T, TArg> : GameEvent<TArg> where T : SingletonEvent<T, TArg> 
    {
        private static T _instance; 
        public static T Instance => _instance != null
            ? _instance
            : _instance = SingletonScriptableObjectManager.LoadOrCreateInstance<T>();
        public new static void Register(Action<TArg> callback)
        {
            ((GameEvent<TArg>)Instance).Register(callback);
        }
    }
}