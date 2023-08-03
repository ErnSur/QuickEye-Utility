using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [LoadFromAsset("non/exising/path", Mandatory = false)]
    internal class SoWithNonMandatoryMissingAsset : ScriptableObject
    {
    }
}