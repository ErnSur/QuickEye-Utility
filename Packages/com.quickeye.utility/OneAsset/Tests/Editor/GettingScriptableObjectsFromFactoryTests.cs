using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(ScriptableObjectFactory))]
    public class GettingScriptableObjectsFromFactoryTests
    {
        private SoWithAsset _assetFromResourcesFolder;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _assetFromResourcesFolder = Resources.Load<SoWithAsset>(SoWithAsset.ResourcesPath);
            Assert.NotNull(_assetFromResourcesFolder);
        }

        [Test]
        public void Should_LoadAsset_When_AssetExists()
        {
            var actual = ScriptableObjectFactory.LoadOrCreateInstance<SoWithAsset>();

            Assert.NotNull(actual);
            Assert.AreEqual(_assetFromResourcesFolder, actual);
        }
        
        
        /// <summary>
        /// Mandatory asset is defined by <see cref="LoadFromAssetAttribute.Mandatory"/>
        /// </summary>
        [Test]
        public void Should_Throw_When_TypeHasMandatoryAssetButAssetIsMissing()
        {
            Assert.Throws<AssetIsMissingException>(() =>
            {
                ScriptableObjectFactory.LoadOrCreateInstance<SoWithMissingAsset>();
            });
        }

        [Test]
        public void Should_CreateNewInstance_When_TypeHasNonMandatoryAssetAndAssetIsMissing()
        {
            var result = ScriptableObjectFactory.LoadOrCreateInstance<SoWithNonMandatoryMissingAsset>();

            Assert.NotNull(result);
        }
    }
}