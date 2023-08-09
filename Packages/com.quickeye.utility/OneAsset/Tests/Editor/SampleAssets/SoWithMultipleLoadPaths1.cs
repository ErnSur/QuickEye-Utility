using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("Resources/"+FirstResourcesPath, Priority = 3)]
    [LoadFromAsset("Resources/"+SecondResourcesPath, Priority = 2)]
    internal class SoWithMultipleLoadPaths1 : ScriptableObject
    {
        public const string FirstResourcesPath = "one-asset-tests/SoWithMultipleLoadPaths 1.1";
        public const string SecondResourcesPath = "one-asset-tests/SoWithMultipleLoadPaths 1.2";
    }
}