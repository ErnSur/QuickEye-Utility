using QuickEye.EventSystem;
using QuickEye.Utility;

namespace Samples.GlobalEvents
{
    public class HpChangeEvent : ExampleEvent<HpChangeEvent,int> { }
    public class MpChangeEvent : SingletonEvent<MpChangeEvent,int> { }
}