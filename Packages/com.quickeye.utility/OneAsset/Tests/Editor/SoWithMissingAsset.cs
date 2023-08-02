using OneAsset;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [LoadFromAsset("non/exising/path", Mandatory = true)]
    internal class SoWithMissingAsset : ScriptableObject
    {
    }
}