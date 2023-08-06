using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    public class UnsafeLoadingTests
    {
        private SoWithAsset _assetInstance;
        private SoWithAsset _unsafeInstance;

        [SetUp]
        public void SetUp()
        {
            _assetInstance = Resources.Load<SoWithAsset>(SoWithAsset.ResourcesPath);
            _unsafeInstance = InternalEditorUtility.LoadSerializedFileAndForget(SoWithAsset.AbsolutePath)[0] as SoWithAsset;
        }

        [Test]
        public void Should_NotBeSame()
        {
            Selection.activeObject = _unsafeInstance;
            Assert.NotNull(_unsafeInstance);
            Assert.NotNull(_assetInstance);
            Assert.AreNotEqual(_unsafeInstance,_assetInstance);
        }
    }
}