using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [CreateAssetAutomatically(SampleAssetsTempDirectory.TemporaryTestOnlyDirectory)]
    [LoadFromAsset(PathToAssetInResourcesDirectory, Order = 1)]
    [LoadFromAsset(SecondaryPathToAssetInResourcesDirectory, Order = 2)]
    internal class SoWithCreateAutomatically2 : ScriptableObject
    {
        private const string PathToAssetInResourcesDirectory = "one-asset-tests/"+ nameof(SoWithCreateAutomatically2);
        private const string SecondaryPathToAssetInResourcesDirectory = PathToAssetInResourcesDirectory + "2";
        
        public const string AbsoluteAssetPath = 
            SampleAssetsTempDirectory.TemporaryTestOnlyDirectory 
            + "Resources/" 
            + PathToAssetInResourcesDirectory 
            + ".asset";
        
        public const string SecondaryAbsoluteAssetPath = 
            SampleAssetsTempDirectory.TemporaryTestOnlyDirectory 
            + "Resources/" 
            + SecondaryPathToAssetInResourcesDirectory 
            + ".asset";
    }
}