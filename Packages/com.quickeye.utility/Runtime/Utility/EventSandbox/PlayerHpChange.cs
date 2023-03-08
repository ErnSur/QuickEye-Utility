using UnityEngine;

namespace QuickEye.Utility
{
    [CreateAssetMenu]
    public class PlayerHpChange : SingletonEvent<PlayerHpChange,float>
    {
        //public new static void Register(Action<float> callback)=>ISingleton<PlayerHpChange>.Instance.Register(default);

        // If I want to have all
        // empty body of PlayerHpChange
        // non singleton base class
        // static API on PlayerHpChange
        // but having all of them at once is impossible
        // I need to have a compromise somewhere
        // so what is more valueaable
        // preetier API
        // more functionality
    }
}