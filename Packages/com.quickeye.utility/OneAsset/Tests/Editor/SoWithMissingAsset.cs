using QuickEye.Utility;
using UnityEngine;

namespace OneAsset.Tests.Editor
{
    [SingletonAsset("non/exising/path", Mandatory = true)]
    internal class SoWithMissingAsset : ScriptableObject
    {
    }
}