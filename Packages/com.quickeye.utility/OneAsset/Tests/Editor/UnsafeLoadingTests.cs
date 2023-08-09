using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    public class UnsafeLoadingTests
    {
        [Test]
        public void LoadUnsafe()
        {
            var attribute = new LoadFromAssetAttribute("UNSAFE_TEST")
            {
                LoadAndForget = true
            };
            var so = ScriptableObject.CreateInstance<SoWithAsset>();
            UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { so }, attribute.Path,
                true);

            var instance = OneAssetLoader.LoadOrCreateInstance(typeof(SoWithAsset), attribute);

            Assert.NotNull(instance);
            Assert.AreEqual(typeof(SoWithAsset), instance.GetType());
        }
    }
}