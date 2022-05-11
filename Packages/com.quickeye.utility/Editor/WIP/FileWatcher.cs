using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    class FileWatcher
    {
        private static FileSystemWatcher fileWatcher;
        [InitializeOnLoadMethod]
        static void Main()
        {
            fileWatcher = new FileSystemWatcher(@"C:\Users\Work\Repos\ErnSur\QuickEye-Utility");
            fileWatcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Security
                                   | NotifyFilters.Size;

            fileWatcher.Changed += OnChanged;
            fileWatcher.Created += OnCreated;
            fileWatcher.Deleted += OnDeleted;
            fileWatcher.Renamed += OnRenamed;
            fileWatcher.Error += OnError;

            fileWatcher.Filter = "*.txt";
            fileWatcher.IncludeSubdirectories = true;
            fileWatcher.EnableRaisingEvents = true;
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }

            Debug.Log($"Changed: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created: {e.FullPath}";
            Debug.Log(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            Debug.Log($"Deleted: {e.FullPath}");

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Debug.Log($"Renamed:");
            Debug.Log($"    Old: {e.OldFullPath}");
            Debug.Log($"    New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e) =>
            PrintException(e.GetException());

        private static void PrintException(Exception ex)
        {
            if (ex != null)
            {
                Debug.Log($"Message: {ex.Message}");
                Debug.Log("Stacktrace:");
                Debug.Log(ex.StackTrace);
                PrintException(ex.InnerException);
            }
        }
    }
}