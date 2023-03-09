using System.IO;
using System.Linq;

namespace QuickEye.Utility
{
    internal static class PathUtility
    {
        public static bool ContainsFolder(string folder,string path)
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
    }
}