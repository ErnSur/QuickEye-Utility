using System.Linq;
using System.Reflection;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;

namespace OneAsset.Editor.Tests
{
    [TestOf(typeof(LoadFromAssetUtils))]
    public class AttributeResolutionTests
    {
        [Test]
        public void Should_GetAttributeWithLowestOrder_When_LoadFromAssetUtils()
        {
            var type = typeof(SoWithMultipleLoadPaths1);
            var attributes = type.GetCustomAttributes<LoadFromAssetAttribute>().ToArray();
            var expected = attributes.Select(a => a.Order).Min();

            var attr = LoadFromAssetUtils.GetFirstAttribute(typeof(SoWithMultipleLoadPaths1));

            Assert.AreEqual(expected, attr.Order);
        }
    }
}