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
            var asset = OneAssetLoader.LoadOrCreateScriptableObject<SoWithCreateAutomatically>();

            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(SoWithCreateAutomatically.AbsoluteAssetPath, assetPath);
        }

        [Test]
        public void Should_CreateNewAsset_When_AtPathFromTheAttributeWithHighestPriority()
        {
            var asset = OneAssetLoader.LoadOrCreateScriptableObject<SoWithCreateAutomatically2>();

            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(SoWithCreateAutomatically2.AbsoluteAssetPathNoExt, assetPath);
            FileAssert.DoesNotExist(SoWithCreateAutomatically2.SecondaryAbsoluteAssetPath);
        }

        [Test]
        public void Should_CreateNewAsset_When_PathHasNoFileExtension()
        {
            var options = new AssetLoadOptions($"{TestUtils.TempDir}Resources/test")
            {
                CreateAssetIfMissing = true
            };
           
            var asset = OneAssetLoader.LoadOrCreateScriptableObject(typeof(ScriptableObject), options);

            Assert.IsTrue(AssetDatabase.Contains(asset));
            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(options.Paths[0], assetPath);
        }

        [Test]
        public void Should_CreateNewAsset_When_PathHasFileExtension()
        {
            var options = new AssetLoadOptions($"{TestUtils.TempDir}Resources/test.asset")
            {
                CreateAssetIfMissing = true
            };
            var asset = OneAssetLoader.LoadOrCreateScriptableObject(typeof(ScriptableObject), options);

            Assert.IsTrue(AssetDatabase.Contains(asset));
            var assetPath = AssetDatabase.GetAssetPath(asset);
            StringAssert.Contains(options.Paths[0], assetPath);
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