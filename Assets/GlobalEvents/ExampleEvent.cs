using QuickEye.Utility;
using UnityEditor;
using UnityEngine;

namespace Samples.GlobalEvents
{
    [CreateAssetAutomatically("Assets/Resources")]
    [SingletonAsset("Events", UseTypeNameAsFileName = true, Mandatory = true)]
    public abstract class ExampleEvent<T> : SingletonEvent<T> where T : ExampleEvent<T> { }

    
    [CreateAssetAutomatically("Assets/Resources")]
    [SingletonAsset("Events", UseTypeNameAsFileName = true, Mandatory = true)]
    public abstract class ExampleEvent<T,TArg> : SingletonEvent<T,TArg> where T : ExampleEvent<T,TArg> { }
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