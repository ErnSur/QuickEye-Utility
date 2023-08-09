using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(AbsoluteAssetPath, Priority = 2, CreateAssetAutomatically = true)]
    [LoadFromAsset(SecondaryAbsoluteAssetPath, Priority = 1)]
    internal class SoWithCreateAutomatically2 : ScriptableObject
    {
        public const string AbsoluteAssetPath = 
            TestUtils.TempDir 
            + "Resources/" 
            + "one-asset-tests/"+ nameof(SoWithCreateAutomatically2) 
            + ".asset";
        
        public const string SecondaryAbsoluteAssetPath = 
            TestUtils.TempDir 
            + "Resources/" 
            + "one-asset-tests/"+ nameof(SoWithCreateAutomatically2) +"2" 
            + ".asset";
    }
}