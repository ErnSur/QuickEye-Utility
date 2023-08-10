using UnityEngine;

namespace OneAsset.Editor.Tests.SampleAssets
{
    [CreateAssetMenu]
    [LoadFromAsset(ProjectPath)]
    internal class SoLoadFromAssetDatabase : ScriptableObject
    {
        public const string ProjectPath = "Assets/SoLoadFromAssetDatabase";
    }
}