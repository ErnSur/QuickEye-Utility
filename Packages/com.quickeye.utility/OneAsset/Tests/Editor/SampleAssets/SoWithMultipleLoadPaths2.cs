using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [CreateAssetMenu]
    [LoadFromAsset(FirstResourcesPath, Order = 1)]
    [LoadFromAsset(SecondaryResourcesPath, Order = 2)]
    internal class SoWithMultipleLoadPaths2 : ScriptableObject
    {
        public const string FirstResourcesPath = "no-path";
        public const string SecondaryResourcesPath = "com.quickeye.utility.tests/SoWithMultipleLoadPaths 2";
    }
}