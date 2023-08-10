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