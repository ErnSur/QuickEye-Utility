using OneAsset;
using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    /// <summary>
    /// No Asset Example
    /// </summary>
    public class SingletonWithoutAnAsset : SingletonScriptableObject<SingletonWithoutAnAsset>
    {
        [TextArea]
        public string Description =
            "Call to `SingletonAsset1.Instance` will create a new object instance if it wasn't created before." +
            "It will just create an object instance without creating an Scriptable Object Asset inside the project.";
    }
}