using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(ScriptableObjectFactory))]
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

        [Test]
        public void Should_CreateNewAsset_When_TypeHasInheritedCreateAutomaticallyAttributeAndAssetIsMissing()
        {
            var asset = ScriptableObjectFactory.LoadOrCreateInstance<SoWithInheritedCreateAutomatically>();

            Assert.IsTrue(AssetDatabase.Contains(asset));
        }

        private static void DeleteTestOnlyAssetsIfTheyExist()
        {
            const string directoryName = SampleAssetsTempDirectory.TemporaryTestOnlyDirectory;
            if (Directory.Exists(directoryName))
            {
                AssetDatabase.DeleteAsset(directoryName);
            }
        }
    }
}