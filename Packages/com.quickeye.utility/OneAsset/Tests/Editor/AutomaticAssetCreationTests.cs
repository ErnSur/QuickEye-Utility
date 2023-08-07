using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(ScriptableObjectFactory))]
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
            ScriptableObjectFactory.LoadOrCreateInstance<SoWithCreateAutomatically>();

            FileAssert.Exists(SoWithCreateAutomatically.AbsoluteAssetPath);
        }
        
        [Test]
        public void Should_CreateNewAsset_When_AtPathFromTheAttributeWithHighestPriority()
        {
            ScriptableObjectFactory.LoadOrCreateInstance<SoWithCreateAutomatically2>();

            FileAssert.Exists(SoWithCreateAutomatically2.AbsoluteAssetPath);
            FileAssert.DoesNotExist(SoWithCreateAutomatically2.SecondaryAbsoluteAssetPath);
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