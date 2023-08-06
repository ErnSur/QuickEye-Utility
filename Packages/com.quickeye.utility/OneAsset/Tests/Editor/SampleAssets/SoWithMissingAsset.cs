using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("non/exising/path", Mandatory = true)]
    internal class SoWithMissingAsset : ScriptableObject
    {
    }
}