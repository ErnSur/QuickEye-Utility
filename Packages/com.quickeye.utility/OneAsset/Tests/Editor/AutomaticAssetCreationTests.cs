using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine;

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
            var asset = OneAssetLoader.LoadOrCreateInstance<SoWithCreateAutomatically>();

            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(SoWithCreateAutomatically.AbsoluteAssetPath,assetPath);
        }
        
        [Test]
        public void Should_CreateNewAsset_When_AtPathFromTheAttributeWithHighestPriority()
        {
            var asset = OneAssetLoader.LoadOrCreateInstance<SoWithCreateAutomatically2>();

            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(SoWithCreateAutomatically2.AbsoluteAssetPathNoExt,assetPath);
            FileAssert.DoesNotExist(SoWithCreateAutomatically2.SecondaryAbsoluteAssetPath);
        }
        
        [Test]
        public void Should_CreateNewAsset_When_PathHasNoFileExtension()
        {
            var attribute = new LoadFromAssetAttribute($"{TestUtils.TempDir}Resources/test")
            {
                CreateAssetAutomatically = true
            };
            var asset = OneAssetLoader.LoadOrCreateInstance(typeof(ScriptableObject),attribute);

            Assert.IsTrue(AssetDatabase.Contains(asset));
            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(attribute.Path,assetPath);
        }
        
        [Test]
        public void Should_CreateNewAsset_When_PathHasFileExtension()
        {
            var attribute = new LoadFromAssetAttribute($"{TestUtils.TempDir}Resources/test.asset")
            {
                CreateAssetAutomatically = true
            };
            var asset = OneAssetLoader.LoadOrCreateInstance(typeof(ScriptableObject),attribute);

            Assert.IsTrue(AssetDatabase.Contains(asset));
            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(attribute.Path,assetPath);
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