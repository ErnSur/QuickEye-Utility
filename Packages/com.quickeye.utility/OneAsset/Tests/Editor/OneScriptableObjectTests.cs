using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(OneScriptableObject<>))]
    public class OneScriptableObjectTests
    {
        private SsoWithNoAsset _instance;

        [SetUp]
        public void Setup()
        {
            _instance = SsoWithNoAsset.Instance;
        }

        [TearDown]
        public void Teardown()
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
            var actual = SsoWithNoAsset.Instance;
            Assert.AreEqual(_instance, actual);
        }
        
        [Test]
        public void Should_CreateNewInstance_When_ExistingOneWasDestroyed()
        {
            Object.DestroyImmediate(_instance);

            Assert.NotNull(SsoWithNoAsset.Instance);
        }
    }
}