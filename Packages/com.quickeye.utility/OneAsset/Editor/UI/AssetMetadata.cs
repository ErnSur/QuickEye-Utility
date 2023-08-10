using System;
using System.IO;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace OneAsset.Editor.UI
{
    internal class AssetMetadata
    {
        public readonly Object Asset;

        public readonly string TypeName;

        public readonly AssetLoadOptions LoadOptions;
        public string[] ResourcesPaths { get; }

        public string FirstResourcesPath =>
            ResourcesPaths.Length == 0 ? null : ResourcesPaths[0];

        public string FirstLoadPath =>
            LoadOptions.Paths.Length == 0 ? null : LoadOptions.Paths[0];

        /// <summary>
        /// Metadata about the object that has <see cref="LoadFromAssetAttribute"/>
        /// </summary>
        /// <param name="asset">Type of the object has to have the <see cref="LoadFromAssetAttribute"/></param>
        public AssetMetadata(Object asset)
        {
            Asset = asset;
            var type = asset.GetType();
            TypeName = type.Name;
            LoadOptions = AssetLoadOptionsUtility.GetLoadOptions(type);
            ResourcesPaths = LoadOptions.AssetPaths.Where(p => p.IsInResourcesFolder).Select(p => p.ResourcesPath)
                .ToArray();
        }

        public bool IsInLoadablePath2(out string attributeLoadPath)
        {
            var assetPath = AssetDatabase.GetAssetPath(Asset);
            if (string.IsNullOrEmpty(assetPath))
            {
                attributeLoadPath = null;
                return false;
            }

            var extension = Path.GetExtension(assetPath);
            // the ResourcesPaths need to use forward slashes
            foreach (var resourcesPath in ResourcesPaths)
            {
                var pathEnding = $"Resources/{resourcesPath}{extension}";
                if (assetPath.EndsWith(pathEnding, StringComparison.InvariantCulture))
                {
                    attributeLoadPath = resourcesPath;
                    return true;
                }
            }

            attributeLoadPath = null;
            return false;
        }

        public bool IsInLoadablePath(out AssetPath loadPath)
        {
            var assetPath = AssetDatabase.GetAssetPath(Asset);
            if (string.IsNullOrEmpty(assetPath))
            {
                loadPath = null;
                return false;
            }

            foreach (var loadablePath in LoadOptions.AssetPaths)
            {
                if (loadablePath.IsInResourcesFolder &&
                    assetPath.EndsWith(loadablePath.ResourcesPath + loadablePath.Extension))
                {
                    loadPath = loadablePath;
                    return true;
                }

                if (loadablePath.OriginalPath == assetPath)
                {
                    loadPath = loadablePath;
                    return true;
                }
            }

            loadPath = null;
            return false;
        }
    }
}