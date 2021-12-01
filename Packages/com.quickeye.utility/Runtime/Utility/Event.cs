using System;

namespace QuickEye.Utility
{
    public abstract class Event<T>  where T : Event<T>
    {
        public static event Action<T> Ev;
        public static void Register(Action<T> callback) => Ev += callback;
        public static void Unregister(Action<T> callback) => Ev -= callback;
        public static void Trigger(T payload) => Ev?.Invoke(payload);
    }

    public abstract class Event<T,TArgs>  where T : Event<T,TArgs>
    {
        public static event Action<TArgs> Ev;
        public static void Register(Action<TArgs> callback) => Ev += callback;
        public static void Unregister(Action<TArgs> callback) => Ev -= callback;
        public static void Trigger(TArgs payload) => Ev?.Invoke(payload);
    }
}