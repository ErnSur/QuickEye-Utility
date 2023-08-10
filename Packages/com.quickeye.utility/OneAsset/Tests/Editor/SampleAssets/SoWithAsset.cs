using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset("Resources/"+ResourcesPath)]
    internal class SoWithAsset : ScriptableObject
    {
        public const string ResourcesPath = "one-asset-tests/So With Asset.usrExtension";
    }
}