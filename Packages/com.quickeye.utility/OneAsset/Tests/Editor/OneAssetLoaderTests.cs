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

        [TestCase("Resources/one-asset/test.asset")]
        [TestCase("Assets/Resources/one-asset/test.asset")]
        [TestCase("Assets/Resources/one-asset/test")]
        [TestCase("Assets/Resources/test.asset")]
        [TestCase("Assets/Resources/test")]
        public void Should_LoadAsset_When_AssetExists(string loadPath)
        {
            
            using (new TestAssetScope(loadPath))
            {
                var loadAttribute = new LoadFromAssetAttribute(loadPath);
                var actual = OneAssetLoader.LoadOrCreateInstance(typeof(SoWithAsset),
                    new[] { loadAttribute }, null);

                Assert.NotNull(actual);
                Assert.IsTrue(AssetDatabase.Contains(actual));
            }
        }

        /// <summary>
        /// Mandatory asset is defined by <see cref="LoadFromAssetAttribute.Mandatory"/>
        /// </summary>
        [Test]
        public void Should_Throw_When_TypeHasMandatoryAssetButAssetIsMissing()
        {
            Assert.Throws<AssetIsMissingException>(() =>
            {
                OneAssetLoader.LoadOrCreateInstance<SoWithMissingAsset>();
            });
        }

        [Test]
        public void Should_CreateNewInstance_When_TypeHasNonMandatoryAssetAndAssetIsMissing()
        {
            var result = OneAssetLoader.LoadOrCreateInstance<SoWithNonMandatoryMissingAsset>();

            Assert.NotNull(result);
        }
    }
}