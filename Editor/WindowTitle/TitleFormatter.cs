using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace QuickEye.Utility.Editor.WindowTitle
{
    internal static class TitleFormatter
    {
        private static readonly Dictionary<string, string> _Tags = new Dictionary<string, string>();
        private const string BranchKey = "<Branch>";
        private const string ProjectNameKey = "<ProjectName>";
        private const string ProjectPathKey = "<ProjectPath>";
        private const string RepoDirNameKey = "<RepoDirName>";
        private const string RepoPathKey = "<RepoPath>";
        private const string SceneNameKey = "<SceneName>";
        private const string EditorVersionKey = "<EditorVersion>";
        private const string TargetPlatformKey = "<TargetPlatform>";
        private static string ProjectPath = Path.GetDirectoryName(Application.dataPath);

        static TitleFormatter()
        {
            _Tags.Add(BranchKey, GetBranchName());
            _Tags.Add(ProjectNameKey, Application.productName);
            _Tags.Add(ProjectPathKey, ProjectPath);
            _Tags.Add(RepoPathKey, "RPath");
            _Tags.Add(RepoDirNameKey, GetRepoDirName());
            _Tags.Add(SceneNameKey, GetSceneName());
            _Tags.Add(EditorVersionKey, Application.unityVersion);
            _Tags.Add(TargetPlatformKey, GetTargetPlatform());
        }

        private static string GetRepoDirName()
        {
            return Path.GetFileName(_Tags[RepoPathKey]);
        }

        private static string GetTargetPlatform()
        {
            return EditorUserBuildSettings.selectedBuildTargetGroup.ToString();
        }

        private static string GetBranchName()
        {
            return GitUtility.TryGetBranchName(WindowTitleSettings.RepositoryPath, out var branchName)
                ? branchName
                : "Repository not found";
        }

        private static string GetSceneName()
        {
            var n = SceneManager.GetActiveScene().name;
            if (string.IsNullOrEmpty(n))
                n = "Untitled";
            return n;
        }

        public static string Format(string text)
        {
            var customRepoPath = WindowTitleSettings.RepositoryPath;
            _Tags[BranchKey] = GetBranchName();
            if (string.IsNullOrEmpty(customRepoPath))
            {
                GitUtility.TryFindRepositoryDirUpwards(ProjectPath, out customRepoPath);
            }

            if (!string.IsNullOrEmpty(customRepoPath))
                _Tags[RepoPathKey] = Path.GetFullPath(customRepoPath).TrimEnd(Path.DirectorySeparatorChar);
            _Tags[RepoDirNameKey] = GetRepoDirName();
            foreach (var kvp in _Tags)
            {
                text = text.Replace(kvp.Key, kvp.Value);
            }

            return text;
        }
    }
}