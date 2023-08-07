using NUnit.Framework;
using OneAsset.Tests.SampleAssets;
using UnityEngine;

namespace OneAsset.Tests
{
    public class MultipleLoadPathsTests
    {
        [Test]
        public void Should_LoadAssetInstanceWithHighestPriorityPath_When_TypeHasMultipleAttributes()
        {
            var asset = Resources.Load<GameObjectWithMultiplePaths>(GameObjectWithMultiplePaths.SecondaryResourcesPath);
            Assert.NotNull(asset);
            
            var instance = GameObjectWithMultiplePaths.Instance;

            GameObjectAssert.IsPrefabInstance(instance.gameObject);
        }
        
        [Test]
        public void Should_LoadAssetFromFirstPathThatHasIt_When_AssetIsMissingFromFirstPath()
        {
            var assetFromFirstPath = Resources.Load<GameObjectWithMultiplePaths>(GameObjectWithMultiplePaths.FirstResourcesPathNotValid);
            var assetFromSecondaryPath = Resources.Load<GameObjectWithMultiplePaths>(GameObjectWithMultiplePaths.SecondaryResourcesPath);
            Assert.IsNull(assetFromFirstPath);
            Assert.NotNull(assetFromSecondaryPath);

            var instance = GameObjectWithMultiplePaths.Instance;

            GameObjectAssert.IsPrefabInstance(instance.gameObject);
        }
    }
}