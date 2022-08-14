using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using UnityEditor;

namespace QuickEye.Utility.Editor
{
    // TODO: Create tab files with guid as filename

    public static class LayoutTabManager
    {
        public const string LayoutsPath = "UserSettings/Layouts/Tabs/";
        public const string LayoutsConfigPath = LayoutsPath + "config.json";
        public static event Action LayoutRenamed;
        public static event Action LayoutDeleted;

        private static LayoutToolbarModel _config;

        static LayoutTabManager()
        {
            if (!Directory.Exists(LayoutsPath))
            {
                Directory.CreateDirectory(LayoutsPath);
            }

            if (!File.Exists(LayoutsConfigPath))
            {
                File.WriteAllText(LayoutsConfigPath, new LayoutToolbarModel().ToJson(), Encoding.UTF8);
            }

            _config = LayoutToolbarModel.FromJson(File.ReadAllText(LayoutsConfigPath, Encoding.UTF8));
        }

        public static string GetLastLoadedLayoutName()
        {
            return Toolbar.lastLoadedLayoutName;
        }

        public static string[] GetTabLayouts()
        {
            return _config.tabNames.ToArray();
        }

        public static string[] GetTabLayoutFiles()
        {
            return Directory.GetFiles(LayoutsPath).Where(p => p.EndsWith("wlt"))
                .Select(Path.GetFileNameWithoutExtension).ToArray();
        }

        public static void SaveToJson()
        {
            var json = _config.ToJson();
            File.WriteAllText(LayoutsConfigPath, json);
        }

        public static void SaveLayout(string name)
        {
            //                 WindowLayout.ReloadWindowLayoutMenu();
            Toolbar.lastLoadedLayoutName = name;
            WindowLayout.SaveWindowLayout(NameToLayoutPath(name));
            _config.Add(name);
            SaveToJson();
        }

        public static void LoadLayout(string name)
        {
            TrySaveCurrentTabLayout();
            WindowLayout.LoadWindowLayout(NameToLayoutPath(name), false, true, true);
        }

        private static bool IsTabLayout(string layoutName)
        {
            return _config.tabNames.Contains(layoutName);
        }

        public static void DeleteLayout(string name)
        {
            _config.tabNames.Remove(name);
            File.Delete(NameToLayoutPath(name));
            SaveToJson();
            LayoutDeleted?.Invoke();
        }

        public static bool TryRenameLayout(string oldName, string newName)
        {
            try
            {
                File.Move(NameToLayoutPath(oldName), NameToLayoutPath(newName));
                _config.tabNames[_config.tabNames.IndexOf(oldName)] = newName;
                SaveToJson();
                if (GetLastLoadedLayoutName() == oldName)
                {
                    SaveLayout(newName);
                }
                LayoutRenamed?.Invoke();
                return true;
            }
            catch (IOException e)
            {
                return false;
            }
        }

        public static void TrySaveCurrentTabLayout()
        {
            if (IsTabLayout(GetLastLoadedLayoutName()))
                SaveLayout(GetLastLoadedLayoutName());
        }

        public static void UpdateTabOrder(List<string> tabNames)
        {
            CollectionAssert.AreEquivalent(_config.tabNames,tabNames);
            _config.tabNames = tabNames;
            SaveToJson();
        }

        private static string NameToLayoutPath(string name) => Path.Combine(LayoutsPath, $"{name}.wlt");

        private static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}