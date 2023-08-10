using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("Assets/Resources/"+ResourcesPath)]
    internal class SoWithAbsoluteResourcesPath : ScriptableObject
    {
        private const string ResourcesPath = "one-asset-tests/SoWithAbsoluteResourcesPath";
    }
}