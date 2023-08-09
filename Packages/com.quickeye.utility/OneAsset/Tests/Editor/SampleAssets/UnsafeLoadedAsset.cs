using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(AbsoluteAssetPath, UnsafeLoad = true, CreateAssetAutomatically = true)]
    internal class UnsafeLoadedAsset : ScriptableObject
    {
        public const string AbsoluteAssetPath =
            TestUtils.TempDir
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically) + ".asset";
    }

    [LoadFromAsset(AbsoluteAssetPath, UseTypeNameAsFileName = true, CreateAssetAutomatically = true)]
    internal class UnsafeLoadedAsset2 : ScriptableObject
    {
        public const string AbsoluteAssetPath =
            TestUtils.TempDir
            + "Resources/one-asset-tests/" + nameof(SoWithCreateAutomatically) + ".asset";
    }
}