using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    public class UnsafeLoadingTests
    {
        private const string FilePath = "OneAsset_LoadAndForget_Test";

        [SetUp]
        public void SetUp()
        {
            var so = ScriptableObject.CreateInstance<SoWithAsset>();
            UnityEditorInternal.InternalEditorUtility.SaveToSerializedFileAndForget(new Object[] { so }, FilePath,
                true);
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }

        [Test]
        public void LoadUnsafe()
        {
            var options = new AssetLoadOptions(FilePath)
            {
                LoadAndForget = true
            };

            var instance = OneAssetLoader.LoadOrCreateScriptableObject(typeof(SoWithAsset), options);

            Assert.NotNull(instance);
            Assert.AreEqual(typeof(SoWithAsset), instance.GetType());
        }
    }
}