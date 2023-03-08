using System;
using UnityEngine;

namespace QuickEye.Utility
{
    public class GameEvent :ScriptableObject
    {
        public void Register(Action callback)
        {
        }
    }

    public class GameEvent<T> : ScriptableObject
    {
        public event Action<T> ev;

        public void Register(Action<T> callback)
        {
            ev += callback;
        }

        public static void Register2(Action<T> callback)
        {
        }
    }
}