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
            var options = new AssetLoadOptions("UNSAFE_TEST")
            {
                LoadAndForget = true
            };
            var so = ScriptableObject.CreateInstance<SoWithAsset>();
            UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { so }, options.Paths[0],
                true);

            var instance = OneAssetLoader.LoadOrCreateInstance(typeof(SoWithAsset), options);

            Assert.NotNull(instance);
            Assert.AreEqual(typeof(SoWithAsset), instance.GetType());
        }
    }
}