using System;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(OneAssetLoader))]
    public class MultipleLoadPathsTests
    {
        [Test]
        public void Should_LoadAssetInstanceWithHighestPriorityPath_When_TypeHasMultipleAttributes()
        {
            var asset = Resources.Load<SoWithMultipleLoadPaths1>(SoWithMultipleLoadPaths1.FirstResourcesPath);
            Assert.NotNull(asset);

            var actual = OneAssetLoader.LoadOrCreateScriptableObject<SoWithMultipleLoadPaths1>();

            Assert.AreEqual(asset, actual);
        }

        [Test]
        public void Should_LoadAssetInstanceWithHighestPriorityPath_When_TypeHasMultipleAttributes2()
        {
            var options = TestUtils.CreateLoadOptionsWithUniquePath("Resources","Resources");

            using (var expectedAssetScope = new TestAssetScope(options.Paths[0]))
            using (new TestAssetScope(options.Paths[1]))
            {
                var asset = OneAssetLoader.LoadOrCreateScriptableObject(typeof(SoWithAsset),options);

                Assert.NotNull(asset);
                Assert.AreEqual(expectedAssetScope.Asset, asset);
            }
        }


        [Test]
        public void Should_LoadAssetFromFirstPathThatHasIt_When_AssetIsMissingFromFirstPath()
        {
            var assetFromFirstPath =
                Resources.Load<SoWithMultipleLoadPaths2>(SoWithMultipleLoadPaths2.FirstResourcesPath);
            var assetFromSecondaryPath =
                Resources.Load<SoWithMultipleLoadPaths2>(SoWithMultipleLoadPaths2.SecondaryResourcesPath);
            Assert.IsNull(assetFromFirstPath);
            Assert.NotNull(assetFromSecondaryPath);

            var actual = OneAssetLoader.LoadOrCreateScriptableObject<SoWithMultipleLoadPaths2>();

            Assert.AreEqual(assetFromSecondaryPath, actual);
        }
    }
}