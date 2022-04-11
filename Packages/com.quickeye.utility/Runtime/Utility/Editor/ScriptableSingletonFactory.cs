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
            ScriptableSingleton.SingletonInstantiated += TryCreateAsset;
        }

        // This can crash the editor if it would occur at editor startup
        // https://fogbugz.unity3d.com/default.asp?1322299_jm6m9dvbph96nd5o
        private static void TryCreateAsset(ScriptableObject obj)
        {
            if (!TryGetAssetPath(obj.GetType(), out var path))
                return;

            var baseDir = Path.GetDirectoryName(path);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SaveAssets();
        }

        private static bool TryGetAssetPath(Type type, out string path)
        {
            var attr = type.GetCustomAttribute<SingletonAssetAttribute>();
            if (attr == null)
            {
                path = null;
                return false;
            }

            path = attr.ResourcesPath;
            path = path.TrimStart('/');
            path = path.TrimStart('\\');
            if (!path.StartsWith("Assets/") && !path.StartsWith("Assets\\"))
                path = $"Assets/{path}";
            if (!path.EndsWith(".asset"))
                path += ".asset";
            return true;
        }

        private static string GetAssetPath(Type type)
        {
            const string baseDir = "Assets/Singleton Assets/Resources";
            var attr = type.GetCustomAttribute<SingletonAssetAttribute>();
            var resourcesPath = attr?.ResourcesPath ?? $"{type.Name}";
            var runtimeDir = Path.Combine(baseDir, $"{resourcesPath}.asset");
            return runtimeDir;
        }
    }
}