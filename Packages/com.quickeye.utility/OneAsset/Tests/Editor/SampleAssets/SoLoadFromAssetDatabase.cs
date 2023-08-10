using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [LoadFromAsset(ProjectPath)]
    internal class SoLoadFromAssetDatabase : ScriptableObject
    {
        public const string ProjectPath = "Assets/SoLoadFromAssetDatabase";
    }
}