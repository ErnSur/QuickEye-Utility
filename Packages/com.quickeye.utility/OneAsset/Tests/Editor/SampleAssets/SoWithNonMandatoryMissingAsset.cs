using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("non/exising/path", Mandatory = false)]
    internal class SoWithNonMandatoryMissingAsset : ScriptableObject
    {
    }
}