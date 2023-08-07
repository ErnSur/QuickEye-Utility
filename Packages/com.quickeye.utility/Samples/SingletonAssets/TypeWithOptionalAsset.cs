using OneAsset;
using UnityEngine;

namespace QuickEye.Samples.SingletonAssets
{
    /// <summary>
    /// Load Asset from Resources folder Example
    /// Singleton with path from which it should be loaded from.
    /// </summary>
    [LoadFromAsset(ResourcesPath, Mandatory = false)]
    public class TypeWithOptionalAsset : OneScriptableObject<TypeWithOptionalAsset>
    {
        private const string ResourcesPath = "no path (example of optional asset)";

        [TextArea]
        public string Description =
            $"Call to `{nameof(TypeWithOptionalAsset)}.Instance` will try to load an asset from Resources folder at path specified in `{nameof(LoadFromAssetAttribute)}.`" +
            $"If there is no asset at that path and `{nameof(LoadFromAssetAttribute)}.{nameof(LoadFromAssetAttribute.Mandatory)}` is set to `true` it will throw an exception, otherwise it will create a new non-asset instance.";
    }
}