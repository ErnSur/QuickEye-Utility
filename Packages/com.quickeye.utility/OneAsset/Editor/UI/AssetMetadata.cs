using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.UI
{
    internal class AssetMetadata
    {
        public readonly Object Asset;

        public readonly LoadFromAssetAttribute LoadFromAssetAttribute;
        public string ResourcesPath { get; }
        public bool IsInLoadablePath => LoadFromAssetCache.IsInResourcesPath(AssetDatabase.GetAssetPath(Asset), ResourcesPath);

        public AssetMetadata(Object asset)
        {
            Asset = asset;
            var type = asset.GetType();
            LoadFromAssetAttribute = LoadFromAssetUtils.GetAttribute(type);
            ResourcesPath = LoadFromAssetAttribute.GetResourcesPath(type);
        }
    }
}