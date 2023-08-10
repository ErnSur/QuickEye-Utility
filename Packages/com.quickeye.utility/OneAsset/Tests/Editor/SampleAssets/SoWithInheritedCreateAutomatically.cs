using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    internal class SoWithInheritedCreateAutomatically : SoWithInheritedCreateAutomaticallyBase
    {
    }

    [LoadFromAsset("Resources/"+ResourcesDirectory, CreateAssetAutomatically = true)]
    internal abstract class SoWithInheritedCreateAutomaticallyBase : ScriptableObject
    {
        private const string ResourcesDirectory = "one-asset-tests/";
    }
}