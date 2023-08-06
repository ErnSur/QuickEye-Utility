using UnityEngine;

namespace OneAsset.Tests
{
    [LoadFromAsset(ResourcesPath)]
    internal class SampleSingletonWithPrefab : SingletonMonoBehaviour<SampleSingletonWithPrefab>
    {
        public const string ResourcesPath = "com.quickeye.utility.tests/SampleSingletonWithPrefab";
        
        [SerializeField]
        private MeshFilter meshFilter;

        public MeshFilter MeshFilter => meshFilter;
    }
}