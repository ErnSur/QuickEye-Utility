using QuickEye.Utility;
using UnityEngine;

namespace OneAsset.Tests.Editor
{
    [SingletonAsset("non/exising/path", Mandatory = false)]
    internal class SoWithNonMandatoryMissingAsset : ScriptableObject
    {
    }
}