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

            var actual = OneAssetLoader.LoadOrCreateInstance<SoWithMultipleLoadPaths1>();

            Assert.AreEqual(asset, actual);
        }

        [Test]
        public void Should_LoadAssetInstanceWithHighestPriorityPath_When_TypeHasMultipleAttributes2()
        {
            var attributeWithHighestPriority = TestUtils.CreateLoadAttributeWithUniquePath("Resources");
            attributeWithHighestPriority.Priority = 2;
            var lowerPriorityAttribute = TestUtils.CreateLoadAttributeWithUniquePath("Resources");
            lowerPriorityAttribute.Priority = 1;

            using (new TestAssetScope(lowerPriorityAttribute.Path))
            using (var expectedAssetScope = new TestAssetScope(attributeWithHighestPriority.Path))
            {
                var asset = OneAssetLoader.LoadOrCreateInstance(typeof(SoWithAsset), new[]
                {
                    attributeWithHighestPriority,
                    lowerPriorityAttribute,
                }, null);

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

            var actual = OneAssetLoader.LoadOrCreateInstance<SoWithMultipleLoadPaths2>();

            Assert.AreEqual(assetFromSecondaryPath, actual);
        }
    }
}