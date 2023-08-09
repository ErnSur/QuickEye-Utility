using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    internal class SoWithInheritedCreateAutomatically : SoWithInheritedCreateAutomaticallyBase
    {
    }
    
    [CreateAssetAutomatically()]
    [LoadFromAsset(ResourcesDirectory, UseTypeNameAsFileName = true)]
    internal abstract class SoWithInheritedCreateAutomaticallyBase : ScriptableObject
    {
        private const string ResourcesDirectory = "one-asset-tests/";
        // public const string AbsoluteAssetPath = 
        //     SampleAssetsTempDirectory.TemporaryTestOnlyDirectory 
        //     + "Resources/" 
        //     + ResourcesDirectory 
        //     + ".asset";
    }
}