using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("non/exising/path", AssetIsMandatory = false)]
    internal class SoWithNonMandatoryMissingAsset : ScriptableObject
    {
    }
}