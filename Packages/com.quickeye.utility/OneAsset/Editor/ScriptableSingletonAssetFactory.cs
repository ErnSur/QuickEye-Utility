using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    internal static class ScriptableSingletonAssetFactory
    {
        [InitializeOnLoadMethod]
        private static void RegisterCallback()
        {
            SingletonScriptableObjectFactory.CreateAssetAction += CreateAsset;
        }

        // This can crash the editor if it would occur at editor startup (pre 2021.2.0a15)
        // https://fogbugz.unity3d.com/default.asp?1322299_jm6m9dvbph96nd5o
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
            var singletonAssetAtt = type.GetCustomAttribute<SingletonAssetAttribute>();
            if (singletonAssetAtt == null)
                throw new Exception($"{type.FullName} is missing {nameof(SingletonAssetAttribute)}.");

            var pathStart = PathUtility.EnsurePathStartsWith("Assets", createAssetAtt.ResourcesFolderPath);
            pathStart = PathUtility.EnsurePathEndsWith("Resources", pathStart);
            var pathEnd = singletonAssetAtt.GetResourcesPath(type) + ".asset";
            return $"{pathStart}/{pathEnd}";
        }
    }
}