using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(AbsoluteAssetPathNoExt, Priority = 2, CreateAssetAutomatically = true)]
    [LoadFromAsset(SecondaryAbsoluteAssetPath, Priority = 1)]
    internal class SoWithCreateAutomatically2 : ScriptableObject
    {
        public const string AbsoluteAssetPathNoExt = 
            TestUtils.TempDir 
            + "Resources/" 
            + "one-asset-tests/"+ nameof(SoWithCreateAutomatically2);

        public const string SecondaryAbsoluteAssetPath =
            TestUtils.TempDir
            + "Resources/"
            + "one-asset-tests/" + nameof(SoWithCreateAutomatically2) + "2";
    }
}