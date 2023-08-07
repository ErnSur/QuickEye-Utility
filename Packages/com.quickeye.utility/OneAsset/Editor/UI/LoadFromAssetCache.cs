using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

namespace OneAsset.Editor.UI
{
    internal static class LoadFromAssetCache
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
            InvalidateCacheEntry(guid, assetMetadata);
            return assetMetadata != null;
        }

        static void InvalidateCacheEntry(string guid, AssetMetadata assetMetadata)
        {
            if (assetMetadata != null && assetMetadata.Asset == null)
            {
                CachedGuidResults.Remove(guid);
                CacheResult(guid);
            }
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
            var loadableAsset = GetLoadableAssetOrNull(target);
            if (loadableAsset == null)
            {
                assetMetadata = null;
                return false;
            }

            assetMetadata = CachedEditorTargets[target] = new AssetMetadata(loadableAsset);
            return true;
        }
        
        private static void CacheResult(string guid)
        {
            if (guid == string.Empty)
                return;
            var path = AssetDatabase.GUIDToAssetPath(guid);

            var isLoadableAsset = TryLoadLoadableScriptableObject(path, out var asset) 
                                  || TryGetOneGameObjectPrefab(path, out asset);

            var assetMetadata = isLoadableAsset ? new AssetMetadata(asset) : null;
            // Should set null value if guid does not point to a loadable asset
            CachedGuidResults[guid] = assetMetadata;
        }

        private static bool TryLoadLoadableScriptableObject(string path, out Object asset)
        {
            asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            return asset != null && HasLoadFromAssetAttribute(asset);
        }

        private static bool TryGetOneGameObjectPrefab(string path, out Object component)
        {
            component = AssetDatabase.LoadAssetAtPath<OneGameObject>(path);
            return component != null && HasLoadFromAssetAttribute(component);
        }

        private static bool HasLoadFromAssetAttribute(Object asset)
        {
            return LoadFromAssetUtils.HasAttribute(asset.GetType());
        }

        private static Object GetLoadableAssetOrNull(Object obj)
        {
            if (HasLoadFromAssetAttribute(obj))
                return obj;
            if (obj is AssetImporter)
            {
                var path = AssetDatabase.GetAssetPath(obj);
                obj = AssetDatabase.LoadAssetAtPath<OneGameObject>(path);
                return obj;
            }

            return null;
        }
    }
}