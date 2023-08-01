using QuickEye.Utility;
using UnityEngine;

namespace OneAsset.Tests.Editor
{
    [CreateAssetAutomatically(PathToResourcesDirectory)]
    [SingletonAsset(PathToAssetInResourcesDirectory)]
    internal class SoWithCreateAutomatically : ScriptableObject
    {
        public const string RootTestAssetsDirectory = "Assets/com.quickeye.one-asset.tests/";
        public const string PathToResourcesDirectory = RootTestAssetsDirectory + "Resources/";
        public const string PathToAssetInResourcesDirectory = "com.quickeye.utility.tests/TestAsset";
        public const string AbsoluteAssetPath = PathToResourcesDirectory + PathToAssetInResourcesDirectory + ".asset";
    }
}