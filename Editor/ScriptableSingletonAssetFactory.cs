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
            SingletonScriptableObject.TryCreateAssetAction += TryCreateAsset;
        }

        // This can crash the editor if it would occur at editor startup (pre 2021.2.0a15)
        // https://fogbugz.unity3d.com/default.asp?1322299_jm6m9dvbph96nd5o
        private static bool TryCreateAsset(ScriptableObject obj)
        {
            if (!TryGetAssetPath(obj.GetType(), out var path))
                return false;

            var baseDir = Path.GetDirectoryName(path);
            if (baseDir != null)
                Directory.CreateDirectory(baseDir);
            AssetDatabase.CreateAsset(obj, path);
            AssetDatabase.SaveAssets();
            return true;
        }

        private static bool TryGetAssetPath(Type type, out string path)
        {
            var attr = type.GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            if (attr == null)
            {
                path = null;
                return false;
            }

            path = PathUtility.EnsurePathStartsWith("Assets", attr.FullAssetPath);
            if (!PathUtility.ContainsFolder("Resources", path))
                path = Path.Combine(Path.GetDirectoryName(path) ?? "", "Resources", Path.GetFileName(path));
            if (!path.EndsWith(".asset"))
                path += ".asset";
            return true;
        }
    }
}