using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(LoadFromAssetUtils))]
    public class AttributeResolutionTests
    {
        [TestCase(typeof(SoWithMultipleLoadPaths1))]
        [TestCase(typeof(SoWithMultipleLoadPaths2))]
        public void Should_GetAttributeWithHighestPriority_When_LoadFromAssetUtils(Type type)
        {
            var attributes = type.GetCustomAttributes<LoadFromAssetAttribute>().ToArray();
            var expected = attributes.Select(a => a.Priority).Max();

            var attr = LoadFromAssetUtils.GetFirstAttribute(type);

            Assert.AreEqual(expected, attr.Priority);
        }
    }
}