using QuickEye.Utility;
using UnityEngine;

namespace OneAsset.Tests.Editor
{
    [CreateAssetMenu]
    [SingletonAsset(ResourcesPath)]
    internal class SoWithAsset : ScriptableObject
    {
        public const string ResourcesPath = "com.quickeye.utility.tests/Sso With Asset";
    }
}