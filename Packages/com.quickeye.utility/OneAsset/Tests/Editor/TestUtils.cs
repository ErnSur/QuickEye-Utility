using System;
using System.IO;
using OneAsset.Editor.Tests.SampleAssets;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.Tests
{
    public static class TestUtils
    {
        public const string TempDir = "Assets/one-asset-tests/";

        public static ScriptableObject CreateTestSoAsset(string path)
        {
            path = PathUtility.EnsurePathStartsWith("Assets", path);
            if (!path.EndsWith(".asset"))
                path = $"{path}.asset";
            var so = ScriptableObject.CreateInstance<SoWithAsset>();
            var dirName = Path.GetDirectoryName(path);
            if (!string.IsNullOrWhiteSpace(dirName))
                Directory.CreateDirectory(dirName);

            Debug.Log($"Created asset at: {path}");
            AssetDatabase.CreateAsset(so, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return so;
        }

        public static void DeleteTestOnlyDirectory()
        {
            if (Directory.Exists(TempDir))
            {
                Debug.Log("DELETE");
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
        }

        public void Dispose()
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(Asset));
        }
    }
}