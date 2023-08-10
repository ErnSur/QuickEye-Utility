using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(OneAssetLoader))]
    public class OneAssetLoaderTests
    {
        private SoWithAsset _assetFromResourcesFolder;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _assetFromResourcesFolder = Resources.Load<SoWithAsset>(SoWithAsset.ResourcesPath);
            Assert.NotNull(_assetFromResourcesFolder);
        }

        [SetUp]
        public void SetUp()
        {
            TestUtils.DeleteTestOnlyDirectory();
        }

        [TearDown]
        public void TearDown()
        {
            TestUtils.DeleteTestOnlyDirectory();
        }

        [TestCase("Resources/test")]
        [TestCase("/Resources/test")]
        [TestCase("Resources/test.asset")]
        [TestCase("Resources/test/test")]
        [TestCase("Assets/Resources/test")]
        [TestCase("/Assets/Resources/test")]
        [TestCase("Resources/test.userExtension")]
        public void Should_LoadAsset_When_AssetExists(string path)
        {
            var options = new AssetLoadOptions(path);
            using (new TestAssetScope(options.Paths[0]))
            {
                var actual = OneAssetLoader.LoadOrCreateScriptableObject(typeof(SoWithAsset), options);

                Assert.NotNull(actual);
                Assert.IsTrue(AssetDatabase.Contains(actual));
            }
        }
        
        [TestCase("Assets/one-asset-test-asset")]
        [TestCase("Assets/one-asset-test/asset")]
        [TestCase("/Assets/one-asset-test/asset")]
        [TestCase("Assets/one-asset-test-asset.asset")]
        [TestCase("Assets/one-asset-test-asset.userExtension")]
        public void Should_LoadAsset_When_AssetInNotInResources(string path)
        {
            var options = new AssetLoadOptions(path);
            using (new TestAssetScope(options.Paths[0]))
            {
                var actual = OneAssetLoader.LoadOrCreateScriptableObject(typeof(SoWithAsset), options);

                Assert.NotNull(actual);
                Assert.IsTrue(AssetDatabase.Contains(actual));
            }
        }

        /// <summary>
        /// Mandatory asset is defined by <see cref="LoadFromAssetAttribute.AssetIsMandatory"/>
        /// </summary>
        [Test]
        public void Should_Throw_When_TypeHasMandatoryAssetButAssetIsMissing()
        {
            Assert.Throws<AssetIsMissingException>(() =>
            {
                OneAssetLoader.LoadOrCreateScriptableObject<SoWithMissingAsset>();
            });
        }

        [Test]
        public void Should_CreateNewInstance_When_TypeHasNonMandatoryAssetAndAssetIsMissing()
        {
            var result = OneAssetLoader.LoadOrCreateScriptableObject<SoWithNonMandatoryMissingAsset>();

            Assert.NotNull(result);
        }
    }
}