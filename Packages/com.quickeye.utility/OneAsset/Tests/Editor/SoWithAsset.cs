using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [LoadFromAsset(ResourcesPath)]
    internal class SoWithAsset : ScriptableObject
    {
        public const string ResourcesPath = "com.quickeye.utility.tests/So With Asset";
    }
}