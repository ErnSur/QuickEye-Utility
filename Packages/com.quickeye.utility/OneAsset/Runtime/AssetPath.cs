namespace OneAsset
{
    internal class AssetPath
    {
        public string OriginalPath { get; }
        //public string PathWithExtension { get; }
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