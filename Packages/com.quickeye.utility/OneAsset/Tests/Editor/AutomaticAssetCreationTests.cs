using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(OneAssetLoader))]
    public class AutomaticAssetCreationTests
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
        public void Should_CreateNewAsset_When_TypeHasCreateAutomaticallyAttributeAndAssetIsMissing()
        {
            var asset =OneAssetLoader.LoadOrCreateInstance<SoWithCreateAutomatically>();

            FileAssert.Exists(SoWithCreateAutomatically.AbsoluteAssetPath);
        }
        
        [Test]
        public void Should_CreateNewAsset_When_AtPathFromTheAttributeWithHighestPriority()
        {
            OneAssetLoader.LoadOrCreateInstance<SoWithCreateAutomatically2>();

            FileAssert.Exists(SoWithCreateAutomatically2.AbsoluteAssetPath);
            FileAssert.DoesNotExist(SoWithCreateAutomatically2.SecondaryAbsoluteAssetPath);
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