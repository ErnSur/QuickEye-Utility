using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [CreateAssetAutomatically()]
    [LoadFromAsset(AbsoluteAssetPath, UnsafeLoad = true)]
    internal class UnsafeLoadedAsset : ScriptableObject
    {
        public const string AbsoluteAssetPath = 
            TestUtils.TempDir 
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically) + ".asset";
    }
    
    [CreateAssetAutomatically()]
    [LoadFromAsset(AbsoluteAssetPath, UseTypeNameAsFileName = true)]
    internal class UnsafeLoadedAsset2 : ScriptableObject
    {
        public const string AbsoluteAssetPath = 
            TestUtils.TempDir 
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically) + ".asset";
    }
}