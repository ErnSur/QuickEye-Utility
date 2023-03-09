using QuickEye.Utility;
using UnityEditor;
using UnityEngine;

namespace Samples.GlobalEvents
{
    [CreateAssetAutomatically("Assets/Resources")]
    [SingletonAsset("Events", true, Mandatory = true)]
    public abstract class ExampleEvent<T> : SingletonEvent<T> where T : ExampleEvent<T> { }

    class UI
    {
        [MenuItem("DEBUG/Check")]
        static void Test()
        {
            var obj = DiedEvent.Instance;
            Debug.Log(obj);
        }
    }
}