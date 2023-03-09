using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using QuickEye.Utility;
using UnityEditor;
using Object = UnityEngine.Object;

namespace UnityOne.Editor.EditorGUIExtension
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
            if (editorTarget == null || NullTargetResults.Contains(editorTarget))
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
            if (guid == string.Empty)
                return;
            var path = AssetDatabase.GUIDToAssetPath(guid);

            var isSingletonAsset =
                TryLoadSingletonScriptableObject(path, out var asset) || TryGetSingletonPrefab(path, out asset);

            var assetMetadata = isSingletonAsset ? new AssetMetadata(asset) : null;
            // Should set null value if guid does not point to a singleton asset
            CachedGuidResults[guid] = assetMetadata;
        }

        static bool TryLoadSingletonScriptableObject(string path, out Object asset)
        {
            asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            // Checking for SingletonAssetAttribute directly on type because there can be singleton assets that do not derive from SingletonScriptableObject
            // User can use ScriptableObjectSingletonFactory alone
            return asset != null && asset.GetType().GetCustomAttribute<SingletonAssetAttribute>() != null;
        }

        static bool TryGetSingletonPrefab(string path, out Object prefab)
        {
            prefab = AssetDatabase.LoadAssetAtPath<SingletonMonoBehaviour>(path);
            return prefab != null && prefab.GetType().GetCustomAttribute<SingletonAssetAttribute>() != null;
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
            public string ResourcesPath { get; }
            public bool IsInLoadablePath => IsInResourcesPath(AssetDatabase.GetAssetPath(Asset), ResourcesPath);

            public AssetMetadata(Object asset)
            {
                Asset = asset;
                var type = asset.GetType();
                SingletonAssetAttribute = type.GetCustomAttribute<SingletonAssetAttribute>();
                ResourcesPath = SingletonAssetAttribute.GetResourcesPath(type);
            }
        }

        /// <summary>
        /// both arguments need to use forward slashes
        /// </summary>
        private static bool IsInResourcesPath(string assetPath, string resourcesRelativePath)
        {
            if (string.IsNullOrEmpty(assetPath))
                return false;
            var extension = Path.GetExtension(assetPath);
            return assetPath.EndsWith("Resources/" + resourcesRelativePath + extension,
                StringComparison.InvariantCulture);
        }
    }
}