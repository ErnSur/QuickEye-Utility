using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(ResourcesPath)]
    internal class SoWithAsset : ScriptableObject
    {
        public const string ResourcesPath = "com.quickeye.utility.tests/So With Asset";
        public const string AbsolutePath = "Packages/com.quickeye.utility/OneAsset/Tests/Editor/Resources/com.quickeye.utility.tests/So With Asset.asset";
    }
}