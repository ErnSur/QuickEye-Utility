using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [CreateAssetAutomatically(SampleAssetsTempDirectory.TemporaryTestOnlyDirectory)]
    [LoadFromAsset(AbsoluteAssetPath, UnsafeLoad = true)]
    internal class UnsafeLoadedAsset : ScriptableObject
    {
        public const string AbsoluteAssetPath = 
            SampleAssetsTempDirectory.TemporaryTestOnlyDirectory 
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically) + ".asset";
    }
}