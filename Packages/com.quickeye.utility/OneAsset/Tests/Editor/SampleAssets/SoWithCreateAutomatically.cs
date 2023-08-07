using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [CreateAssetAutomatically(SampleAssetsTempDirectory.TemporaryTestOnlyDirectory)]
    [LoadFromAsset(PathToAssetInResourcesDirectory)]
    internal class SoWithCreateAutomatically : ScriptableObject
    {
        private const string PathToAssetInResourcesDirectory = "one-asset-tests/"+ nameof(SoWithCreateAutomatically);
        public const string AbsoluteAssetPath = 
            SampleAssetsTempDirectory.TemporaryTestOnlyDirectory 
            + "Resources/" 
            + PathToAssetInResourcesDirectory 
            + ".asset";
    }
}