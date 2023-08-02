using OneAsset;
using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    /// <summary>
    /// Load Asset from Resources folder Example
    /// Singleton with path from which it should be loaded from.
    /// </summary>
    [LoadFromAsset(ResourcesPath, Mandatory = false)]
    public class SingletonWithOptionalAsset : SingletonScriptableObject<SingletonWithOptionalAsset>
    {
        private const string ResourcesPath = "SingletonAsset2";

        [TextArea]
        public string Description =
            $"Call to `{nameof(SingletonWithOptionalAsset)}.Instance` will try to load an asset from Resources folder at path specified in `{nameof(LoadFromAssetAttribute)}.`" +
            $"If there is no asset at that path and `{nameof(LoadFromAssetAttribute)}.{nameof(LoadFromAssetAttribute.Mandatory)}` is set to `true` it will throw an exception, otherwise it will create a new non-asset instance.";
    }
}