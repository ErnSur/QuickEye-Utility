using System.Collections.Generic;
using System.Linq;

namespace OneAsset
{
    public class AssetLoadOptions
    {
        public string[] Paths;

        internal Dictionary<string, AssetPath> AssetPaths;
        public bool CreateAssetAutomatically { get; set; }
        public bool AssetIsMandatory { get; set; }
        public bool LoadAndForget { get; set; }

        public AssetLoadOptions(string path) : this(new[] { path })
        {
        }

        public AssetLoadOptions(string[] paths)
        {
            Paths = paths.Select(CleanPath).ToArray();
            AssetPaths = Paths.ToDictionary(p => p, p => new AssetPath(p));
        }

        private string CleanPath(string path)
        {
            return path.TrimStart('/');
        }
    }

    internal class AssetPath
    {
        public string OriginalPath { get; }
        public bool IsInResourcesFolder { get; }
        public string ResourcesPath { get; }

        public AssetPath(string path)
        {
            OriginalPath = path;
            if (PathUtility.ContainsFolder("Resources", path))
            {
                IsInResourcesFolder = true;
                ResourcesPath = PathUtility.GetResourcesPath(path);
            }
        }
    }
}