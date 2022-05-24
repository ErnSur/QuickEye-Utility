using System.IO;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    internal static class GitUtility
    {
        public static bool TryFindRepositoryDirUpwards(string dirName, out string repoDirectory, int iterations = 2)
        {
            for (int i = 0; i < iterations; i++)
            {
                var gitDirName = $"{Path.GetDirectoryName(dirName)}/.git";
                if (Directory.Exists(gitDirName))
                {
                    Debug.Log($"Found: {gitDirName} | {dirName}");
                    repoDirectory = dirName;
                    return true;
                }

                dirName = Path.GetDirectoryName(dirName);
            }

            repoDirectory = default;
            return false;
        }
        
        public static bool TryGetBranchName(string repositoryDir, out string branchName)
        {
            if (string.IsNullOrEmpty(repositoryDir))
                repositoryDir = Path.GetDirectoryName(Application.dataPath);
            var gitDir = Path.Combine(repositoryDir ?? "", ".git");
            if (Directory.Exists(gitDir))
            {
                var headPath = Path.Combine(gitDir, "HEAD");
                var headRef = File.ReadAllText(headPath);
                //"ref: refs/heads/"
                //var pos = headRef.LastIndexOf("/", StringComparison.InvariantCulture) + 1;
                //TODO: Handle detached head case
                var pos = "ref: refs/heads/".Length;
                branchName = headRef.Substring(pos).TrimEnd();
                return true;
            }

            branchName = default;
            return false;
        }
    }
}