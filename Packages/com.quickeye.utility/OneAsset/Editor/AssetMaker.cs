using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor
{
    internal static class AssetMaker
    {
        [InitializeOnLoadMethod]
        private static void RegisterCallback()
        {
            OneAssetLoader.CreateAssetAction += CreateAsset;
            Console.WriteLine("[AssetLoadTest] AssetMaker RegisterCallback");
        }

        private static void CreateAsset(ScriptableObject obj, AssetLoadOptions options)
        {
            var path = options.Paths[0];
            var baseDir = Path.GetDirectoryName(path);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            var assetPath = path;
            if (!assetPath.EndsWith(".asset"))
                assetPath = $"{assetPath}.asset";
            AssetDatabase.CreateAsset(obj, assetPath);
            AssetDatabase.SaveAssets();
        }
    }
}