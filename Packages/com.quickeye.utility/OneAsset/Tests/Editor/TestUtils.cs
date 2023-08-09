using System;
using System.IO;
using NUnit.Framework;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    public static class TestUtils
    {
        public const string TempDir = "Assets/one-asset-tests/";

        public static LoadFromAssetAttribute CreateLoadAttributeWithUniquePath(string pathWithoutFileName)
        {
            var fileName = Guid.NewGuid();
            return new LoadFromAssetAttribute($"{pathWithoutFileName}/{fileName}");
        }
        public static ScriptableObject CreateTestSoAsset(string path)
        {
            path = PathUtility.EnsurePathStartsWith("Assets", path);
            if (!path.EndsWith(".asset"))
                path = $"{path}.asset";
            var so = ScriptableObject.CreateInstance<SoWithAsset>();
            var dirName = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dirName))
                Directory.CreateDirectory(dirName);

            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return so;
        }

        public static void DeleteTestOnlyDirectory()
        {
            if (Directory.Exists(TempDir))
            {
                AssetDatabase.DeleteAsset(TempDir);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    internal class TestAssetScope : IDisposable
    {
        public ScriptableObject Asset { get; }

        public TestAssetScope(string path)
        {
            Asset = TestUtils.CreateTestSoAsset(path);
            
            Assert.IsTrue(AssetDatabase.Contains(Asset));
        }

        public void Dispose()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(Asset));
        }
    }
}