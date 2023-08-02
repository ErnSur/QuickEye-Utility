using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor
{
    internal static class ScriptableSingletonAssetFactory
    {
        [InitializeOnLoadMethod]
        private static void RegisterCallback()
        {
            ScriptableObjectFactory.CreateAssetAction += CreateAsset;
        }

        private static void CreateAsset(ScriptableObject obj)
        {
            var fullAssetPath = GetFullAssetPath(obj.GetType());
            var baseDir = Path.GetDirectoryName(fullAssetPath);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            AssetDatabase.CreateAsset(obj, fullAssetPath);
            AssetDatabase.SaveAssets();
        }

        private static string GetFullAssetPath(Type type)
        {
            var createAssetAtt = type.GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            if (createAssetAtt == null)
                throw new Exception($"{type.FullName} is missing {nameof(CreateAssetAutomaticallyAttribute)}.");
            var singletonAssetAtt = type.GetCustomAttribute<LoadFromAssetAttribute>();
            if (singletonAssetAtt == null)
                throw new Exception($"{type.FullName} is missing {nameof(LoadFromAssetAttribute)}.");

            var pathStart = PathUtility.EnsurePathStartsWith("Assets", createAssetAtt.ResourcesFolderPath);
            pathStart = PathUtility.EnsurePathEndsWith("Resources", pathStart);
            var pathEnd = singletonAssetAtt.GetResourcesPath(type) + ".asset";
            return $"{pathStart}/{pathEnd}";
        }
    }
}