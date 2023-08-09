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

        public static string EnsurePathStartsWith(string folderName, string path)
        {
            var cleanPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
            var folders = cleanPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return folders.First() == folderName ? path : Path.Combine(folderName, path);
        }

        // TODO: enforce forward slashes
        public static string EnsurePathEndsWith(string folderName, string path)
        {
            var cleanPath = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileName(path));
            var folders = cleanPath.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            return folders.Last() == folderName ? path : Path.Combine(path, folderName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folderName"></param>
        /// <param name="path">Must use forward slashes</param>
        /// <returns></returns>
        public static string GetPathRelativeTo(string folderName, string path)
        {
            if (!path.StartsWith("/"))
                path = $"/{path}";
            // TODO: chack if is case sensitive
            folderName = $"/{folderName}/";
            // Find the last index of the folder name in the path
            int index = path.LastIndexOf(folderName, StringComparison.InvariantCulture);
            if (index == -1)
            {
                return string.Empty;
            }

            return path.Substring(index + folderName.Length);
        }

        // TODO: test on windows
        public static string GetPathWithoutExtension(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return path;
            var dirName = Path.GetDirectoryName(path);
            var fileName = Path.GetFileNameWithoutExtension(path);
            if (string.IsNullOrWhiteSpace(dirName))
                return fileName;
            return $"{dirName}/{fileName}";
        }
    }
}