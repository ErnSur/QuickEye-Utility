using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    // todo: support createassetautoma
    [LoadFromAsset(FirstResourcesPath, Order = -3)]
    [LoadFromAsset(SecondResourcesPath, Order = 2)]
    internal class SoWithMultipleLoadPaths1 : ScriptableObject
    {
        public const string FirstResourcesPath = "com.quickeye.utility.tests/SoWithMultipleLoadPaths 1.1";
        public const string SecondResourcesPath = "com.quickeye.utility.tests/SoWithMultipleLoadPaths 1.2";
    }
}