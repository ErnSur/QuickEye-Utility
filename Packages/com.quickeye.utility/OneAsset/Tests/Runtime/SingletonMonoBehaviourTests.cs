using NUnit.Framework;
using UnityEngine;

namespace OneAsset.Tests
{
    [TestOf(typeof(SingletonMonoBehaviour<>))]
    public class SingletonMonoBehaviourTests
    {
        private SampleSingletonWithoutPrefab _instance;

        [SetUp]
        public void SetUp()
        {
            _instance = SampleSingletonWithoutPrefab.Instance;
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(_instance);
        }

        [Test]
        public void Should_CreateNewInstance_When_NoInstanceExists()
        {
            Assert.NotNull(_instance);
        }
        
        [Test]
        public void Should_ReturnSameInstance_When_OneInstanceExists()
        {
            var actual = SampleSingletonWithoutPrefab.Instance;
            Assert.AreEqual(_instance, actual);
        }

        [Test]
        public void Should_CreateNewInstance_When_ExistingOneWasDestroyed()
        {
            Object.DestroyImmediate(_instance);

            Assert.NotNull(SampleSingletonWithoutPrefab.Instance);
        }
        
        [Test]
        public void Should_CreateNewInstanceFromPrefab_When_HasLoadFromAssetAttribute()
        {
            // Arrange
            var prefab = Resources.Load<SampleSingletonWithPrefab>(SampleSingletonWithPrefab.ResourcesPath);
            Assert.NotNull(prefab);

            // Act
            var instance = SampleSingletonWithPrefab.Instance;
            
            // Assert
            Assert.AreNotEqual(prefab, instance);
            // Mesh filter reference was serialized in prefab
            Assert.NotNull(instance.MeshFilter);
        }
    }
}