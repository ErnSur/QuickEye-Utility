using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [CreateAssetAutomatically(PathToResourcesDirectory)]
    [LoadFromAsset(PathToAssetInResourcesDirectory)]
    internal class SoWithCreateAutomatically : ScriptableObject
    {
        public const string RootTestAssetsDirectory = "Assets/com.quickeye.one-asset.tests/";
        public const string PathToResourcesDirectory = RootTestAssetsDirectory + "Resources/";
        public const string PathToAssetInResourcesDirectory = "com.quickeye.utility.tests/TestAsset";
        public const string AbsoluteAssetPath = PathToResourcesDirectory + PathToAssetInResourcesDirectory + ".asset";
    }
}