using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(FirstResourcesPath, Priority = 99)]
    [LoadFromAsset("Resources/"+SecondaryResourcesPath, Priority = -2)]
    internal class SoWithMultipleLoadPaths2 : ScriptableObject
    {
        public const string FirstResourcesPath = "no-path";
        public const string SecondaryResourcesPath = "one-asset-tests/SoWithMultipleLoadPaths 2";
    }
}