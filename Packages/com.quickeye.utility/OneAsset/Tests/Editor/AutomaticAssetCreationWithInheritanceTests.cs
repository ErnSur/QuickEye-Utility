using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(OneAssetLoader))]
    public class AutomaticAssetCreationWithInheritanceTests
    {
        [SetUp]
        public void Setup()
        {
            DeleteTestOnlyAssetsIfTheyExist();
        }

        [TearDown]
        public void Teardown()
        {
            DeleteTestOnlyAssetsIfTheyExist();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            AssetDatabase.Refresh();
        }

        [Test][Ignore("Feature not supported in this version")]
        public void Should_CreateNewAsset_When_TypeHasInheritedCreateAutomaticallyAttributeAndAssetIsMissing()
        {
            var asset = OneAssetLoader.LoadOrCreateScriptableObject<SoWithInheritedCreateAutomatically>();

            Assert.IsTrue(AssetDatabase.Contains(asset));
        }

        private static void DeleteTestOnlyAssetsIfTheyExist()
        {
            const string directoryName = TestUtils.TempDir;
            if (Directory.Exists(directoryName))
            {
                AssetDatabase.DeleteAsset(directoryName);
            }
        }
    }
}