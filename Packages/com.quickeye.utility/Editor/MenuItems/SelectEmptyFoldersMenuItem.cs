using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility.Editor
{
    internal static class SelectEmptyFoldersMenuItem
    {
        private const string ContextMenuPath = "Assets/Select Empty Folders";
        
        [MenuItem(ContextMenuPath, priority = 35)]
        private static void SelectEmptyFolders()
        {
            var directories = GetSelectedDirectories();
            var paths = FindEmptyFolders(directories).ToArray();
            Selection.objects = paths.Select(AssetDatabase.LoadAssetAtPath<Object>).ToArray();
            foreach (var o in Selection.objects)
            {
                InternalUtility.FrameObjectInProjectWindow(o.GetInstanceID());
            }
        }

        private static List<string> GetSelectedDirectories()
        {
            var paths = Selection.instanceIDs
                .Where(ProjectWindowUtil.IsFolder)
                .Select(AssetDatabase.GetAssetPath)
                .Where(p => !p.StartsWith("Packages/"))
                .ToList();
            if (Selection.assetGUIDs.Length == 0)
                paths.Add(Application.dataPath);
            return paths;
        }

        private static IEnumerable<string> FindEmptyFolders(IReadOnlyCollection<string> directories)
        {
            return directories
                .SelectMany(d => Directory.GetDirectories(d, "*", SearchOption.AllDirectories))
                .Where(IsDirectoryEmptyRecursive);
        }

        private static bool IsDirectoryEmptyRecursive(string directoryName)
        {
            var dirs = Directory.GetDirectories(directoryName, "*", SearchOption.AllDirectories);
            var hasNoDirs = dirs.All(IsDirectoryEmptyRecursive);
            var hasNoFiles = Directory.GetFiles(directoryName).All(IsMetaFile);
            return hasNoDirs && hasNoFiles;

            bool IsMetaFile(string path) => path.EndsWith(".meta") || path == ".DS_Store";
        }
    }
}