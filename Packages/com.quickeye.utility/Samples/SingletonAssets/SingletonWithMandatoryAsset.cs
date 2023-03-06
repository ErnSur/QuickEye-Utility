using UnityEngine;

namespace QuickEye.Utility.Samples.SingletonAssets
{
    /// <summary>
    /// Load Asset from Resources folder Example
    /// Singleton with path from which it should be loaded from, and one that creates the asset automatically if it does not exists.
    /// </summary>
    [SingletonAsset(ResourcesPath, Mandatory = true)]
    // CreateAssetAutomatically Attribute turns on a system that will create scriptable object file at specific path
    // if it cannot be loaded from path specified in `SingletonAsset` attribute
    [CreateAssetAutomatically(AutoCreatePath)]
    public class SingletonWithMandatoryAsset : SingletonScriptableObject<SingletonWithMandatoryAsset>
    {
        private const string ResourcesPath = nameof(SingletonWithMandatoryAsset);
        private const string AutoCreatePath = "Assets/Samples/Settings/Resources/" + ResourcesPath;

        [TextArea(10,30)]
        public string Description =
            $"Call to `{nameof(SingletonWithMandatoryAsset)}.Instance` will try to load an asset from Resources folder at path specified in `{nameof(SingletonAssetAttribute)}.`" +
            $"If there is no asset at that path a new Asset will be created at: {AutoCreatePath}";
    }
}