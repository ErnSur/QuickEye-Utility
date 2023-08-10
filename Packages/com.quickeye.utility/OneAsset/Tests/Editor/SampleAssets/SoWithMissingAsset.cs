using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("non/exising/path", AssetIsMandatory = true)]
    internal class SoWithMissingAsset : ScriptableObject
    {
    }
}