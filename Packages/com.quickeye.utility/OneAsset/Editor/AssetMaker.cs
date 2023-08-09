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

        private static void CreateAsset(ScriptableObject obj,LoadFromAssetAttribute attribute)
        {
            var baseDir = Path.GetDirectoryName(attribute.Path);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            AssetDatabase.CreateAsset(obj, attribute.Path);
            AssetDatabase.SaveAssets();
        }
    }
}