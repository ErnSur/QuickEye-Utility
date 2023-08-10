using System.IO;

namespace OneAsset
{
    internal class AssetPath
    {
        public string OriginalPath { get; }
        public bool IsInResourcesFolder { get; }
        public string ResourcesPath { get; }
        public string Extension { get; }

        public AssetPath(string path)
        {
            OriginalPath = path;
            Extension = Path.GetExtension(path);
            if (PathUtility.ContainsFolder("Resources", path))
            {
                IsInResourcesFolder = true;
                ResourcesPath = PathUtility.GetResourcesPath(path);
            }
        }

        public override string ToString()
        {
            return IsInResourcesFolder ? $"*/Resources/{ResourcesPath}" : OriginalPath;
        }
    }
}