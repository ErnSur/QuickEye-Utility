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

        private static void CreateAsset(ScriptableObject obj)
        {
            if (!OneAssetLoader.TryGetAbsoluteAssetPath(obj.GetType(), out var fullAssetPath))
            {
                throw new InvalidOperationException($"Could not get full assetPath for object {obj}");
            }
            
            var baseDir = Path.GetDirectoryName(fullAssetPath);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            AssetDatabase.CreateAsset(obj, fullAssetPath);
            AssetDatabase.SaveAssets();
        }
    }
}