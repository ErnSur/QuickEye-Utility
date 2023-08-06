using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(ResourcesPath1, Order = 1)]
    [LoadFromAsset(ResourcesPath2, Order = 2)]
    internal class SoWithMultipleLoadPaths2 : ScriptableObject
    {
        public const string ResourcesPath1 = "no-path";
        public const string ResourcesPath2 = "com.quickeye.utility.tests/SoWithMultipleLoadPaths2 2";
    }
}