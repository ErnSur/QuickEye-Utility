using NUnit.Framework;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(SingletonScriptableObject<>))]
    public class SingletonScriptableObjectTests
    {
        private SsoWithNoAsset _ssoWithNoAsset;

        [SetUp]
        public void Setup()
        {
            _ssoWithNoAsset = SsoWithNoAsset.Instance;
        }

        [TearDown]
        public void Teardown()
        {
            Object.DestroyImmediate(_ssoWithNoAsset);
        }

        [Test]
        public void Should_CreateNewInstance_When_NoInstanceExists()
        {
            Assert.NotNull(_ssoWithNoAsset);
        }

        [Test]
        public void Should_ReturnSameInstance_When_OneInstanceExists()
        {
            var actual = SsoWithNoAsset.Instance;
            Assert.AreEqual(_ssoWithNoAsset, actual);
        }
        
        [Test]
        public void Should_CreateNewInstance_When_ExistingOneWasDestroyed()
        {
            Object.DestroyImmediate(_ssoWithNoAsset);

            Assert.NotNull(SsoWithNoAsset.Instance);
        }
    }
}