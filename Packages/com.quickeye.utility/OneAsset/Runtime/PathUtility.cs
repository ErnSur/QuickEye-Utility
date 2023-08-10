using System;
using System.IO;
using System.Linq;

namespace OneAsset
{
    internal static class PathUtility
    {
        public static bool ContainsFolder(string folder, string path)
        {
            var dirName = Path.GetDirectoryName(path);
            if (dirName == null)
                return false;
            var folders = dirName.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return folders.Contains(folder);
        }

        // TODO: enforce forward slashes
        public static string EnsurePathStartsWith(string folderName, string path)
        {
            var cleanPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
            var folders = cleanPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            if (folders.FirstOrDefault() == folderName)
                return path;
            return Combine(folderName, path);
        }

        public static string EnsurePathEndsWith(string folderName, string path)
        {
            var cleanPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
            var folders = cleanPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return folders.Last() == folderName ? path : Path.Combine(path, folderName);
        }

        /// <param name="folderName">Name of the folder that the result path should start with</param>
        /// <param name="path">Must use forward slashes</param>
        public static string GetPathRelativeTo(string folderName, string path)
        {
            if (!path.StartsWith("/"))
                path = $"/{path}";

            folderName = $"/{folderName}/";
            // Find the last index of the folder name in the path
            int index = path.LastIndexOf(folderName, StringComparison.InvariantCulture);
            if (index == -1)
            {
                return string.Empty;
            }

            return path.Substring(index + folderName.Length);
        }

        public static string GetPathWithoutAssetOrPrefabExtension(string path)
        {
            const string assetExtension = ".asset";
            const string prefabExtension = ".prefab";
            if (path.EndsWith(assetExtension))
                return GetPathWithoutExtension(path, assetExtension);
            else
                return GetPathWithoutExtension(path, prefabExtension);
        }

        private static string GetPathWithoutExtension(string path, string extension)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;
            if (!path.EndsWith(extension))
                return path;
            return path.Substring(0, path.Length - extension.Length);
        }

        private static string Combine(params string[] pathSegments)
        {
            return string.Join("/", pathSegments.Select(p => p.Trim('/', '\\')));
        }

        public static string GetResourcesPath(string path)
        {
            path = GetPathRelativeTo("Resources", path);

            path = GetPathWithoutAssetOrPrefabExtension(path);
            return path.TrimStart('/');
        }
    }
}