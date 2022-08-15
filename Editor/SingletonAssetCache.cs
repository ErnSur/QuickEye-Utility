using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using Object = UnityEngine.Object;

namespace QuickEye.Utility.Editor
{
    internal static class SingletonAssetCache
    {
        private static readonly Dictionary<string, AssetMetadata> CachedGuidResults =
            new Dictionary<string, AssetMetadata>();

        private static readonly Dictionary<Object, AssetMetadata> CachedEditorTargets =
            new Dictionary<Object, AssetMetadata>();

        private static readonly HashSet<Object> NullTargetResults = new HashSet<Object>();
        private static Object _lastNullTargetResult;
        private const int NullTargetResultsCapacity = 100;

        public static bool TryGetEntry(string guid, out AssetMetadata assetMetadata)
        {
            if (!CachedGuidResults.TryGetValue(guid, out assetMetadata))
                CacheResult(guid);
            return assetMetadata != null;
        }

        public static bool TryGetEntry(Object editorTarget, out AssetMetadata assetMetadata)
        {
            if (NullTargetResults.Contains(editorTarget))
            {
                assetMetadata = null;
                return false;
            }

            if (CachedEditorTargets.TryGetValue(editorTarget, out assetMetadata) ||
                TryGetAndCacheTargetEntry(editorTarget, out assetMetadata))
                return true;

            if (NullTargetResults.Count == NullTargetResultsCapacity)
            {
                NullTargetResults.Remove(_lastNullTargetResult);
            }

            NullTargetResults.Add(editorTarget);
            _lastNullTargetResult = editorTarget;
            return false;
        }

        private static bool TryGetAndCacheTargetEntry(Object target, out AssetMetadata assetMetadata)
        {
            var singletonAsset = GetSingleton(target);
            if (singletonAsset == null)
            {
                assetMetadata = null;
                return false;
            }

            assetMetadata = CachedEditorTargets[target] = new AssetMetadata(singletonAsset);
            return true;
        }

        private static void CacheResult(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Object singleton = AssetDatabase.LoadAssetAtPath<SingletonScriptableObject>(path);
            if (singleton == null)
                singleton = AssetDatabase.LoadAssetAtPath<SingletonMonoBehaviour>(path);

            AssetMetadata assetMetadata = null;
            if (singleton != null)
            {
                assetMetadata = new AssetMetadata(singleton);
            }

            CachedGuidResults[guid] = assetMetadata;
        }

        private static Object GetSingleton(Object obj)
        {
            if (obj is SingletonScriptableObject)
                return obj;
            if (obj is AssetImporter)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                obj = AssetDatabase.LoadAssetAtPath<SingletonMonoBehaviour>(path);
                return obj;
            }

            return null;
        }

        public class AssetMetadata
        {
            public readonly Object Asset;
            public readonly SingletonAssetAttribute SingletonAssetAttribute;
            public readonly CreateAssetAutomaticallyAttribute CreateAssetAutomaticallyAttribute;

            public bool IsInLoadablePath => ToResourcesRelativePath(AssetDatabase.GetAssetPath(Asset)) == ResourcesPath;
            public string ResourcesPath => SingletonAssetAttribute?.ResourcesPath;
            public string FullAssetPath => CreateAssetAutomaticallyAttribute?.FullAssetPath;

            public AssetMetadata(Object asset)
            {
                Asset = asset;
                var type = asset.GetType();
                SingletonAssetAttribute = type.GetCustomAttribute<SingletonAssetAttribute>();
                CreateAssetAutomaticallyAttribute = type.GetCustomAttribute<CreateAssetAutomaticallyAttribute>();
            }
        }

        private static string ToResourcesRelativePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;
            path = Path.GetFullPath(path);
            path = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileNameWithoutExtension(path));

            var resourcesFolder = $"{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}";
            var index = path.IndexOf(resourcesFolder, StringComparison.InvariantCulture);
            return index == -1 ? null : path.Substring(index + resourcesFolder.Length);
        }
    }
}