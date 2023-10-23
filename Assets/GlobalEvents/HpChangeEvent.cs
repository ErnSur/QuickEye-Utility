using QuickEye.EventSystem;
using QuickEye.Utility;
using UnityEngine;

namespace Samples.GlobalEvents
{
    public class HpChangeEvent : ExampleEvent<HpChangeEvent, int>
    {
        [ContextMenu("Create Component")]
        void CreateComponent()
        {
            var go = new GameObject();
            go.name = "HP CHange Event";
            go.AddComponent<Hub>();
        }
    }
    public class MpChangeEvent : SingletonEvent<MpChangeEvent,int> { }
}