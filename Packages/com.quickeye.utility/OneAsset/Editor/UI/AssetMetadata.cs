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

        public readonly LoadFromAssetAttribute[] LoadFromAssetAttributes;
        public string[] ResourcesPaths { get; }
        public string FirstResourcesPath => ResourcesPaths[0];

        public LoadFromAssetAttribute FirstLoadFromAssetAttribute =>
            ResourcesPaths.Length == 0 ? null : LoadFromAssetAttributes[0];

        /// <summary>
        /// Metadata about the object that has <see cref="LoadFromAssetAttribute"/>
        /// </summary>
        /// <param name="asset">Type of the object has to have the <see cref="LoadFromAssetAttribute"/></param>
        public AssetMetadata(Object asset)
        {
            Asset = asset;
            var type = asset.GetType();
            TypeName = type.Name;
            LoadFromAssetAttributes = LoadFromAssetUtils.GetAttributesInOrder(type);
            ResourcesPaths = LoadFromAssetAttributes.Select(a => a.TryGetResourcesPath(type)).ToArray();
        }

        public bool IsInLoadablePath(out string attributeLoadPath)
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
    }
}