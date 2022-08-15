using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NUnit.Framework;
using UnityEditor;

namespace QuickEye.Utility.Editor
{
    public static class LayoutTabManager
    {
        public const string LayoutsPath = "UserSettings/Layouts/Tabs/";
        public const string LayoutsConfigPath = LayoutsPath + "config.json";

        private static readonly LayoutToolbarModel _Config;

        static LayoutTabManager()
        {
            if (!Directory.Exists(LayoutsPath))
                Directory.CreateDirectory(LayoutsPath);

            if (!File.Exists(LayoutsConfigPath))
                File.WriteAllText(LayoutsConfigPath, new LayoutToolbarModel().ToJson(), Encoding.UTF8);

            _Config = LayoutToolbarModel.FromJson(File.ReadAllText(LayoutsConfigPath, Encoding.UTF8));
        }

        public static event Action LayoutRenamed;
        public static event Action LayoutDeleted;

        public static string GetLastLoadedLayoutName()
        {
            return Toolbar.lastLoadedLayoutName;
        }

        public static string[] GetTabLayouts()
        {
            return _Config.tabNames.ToArray();
        }

        private static void SaveToJson()
        {
            var json = _Config.ToJson();
            File.WriteAllText(LayoutsConfigPath, json);
        }

        public static void SaveLayout(string name)
        {
            Toolbar.lastLoadedLayoutName = name;
            WindowLayout.SaveWindowLayout(NameToLayoutPath(name));
            _Config.Add(name);
            SaveToJson();
        }

        public static void LoadLayout(string name)
        {
            TrySaveCurrentTabLayout();
            WindowLayout.LoadWindowLayout(NameToLayoutPath(name), false, true, true);
        }

        private static bool IsTabLayout(string layoutName)
        {
            return _Config.tabNames.Contains(layoutName);
        }

        public static void DeleteLayout(string name)
        {
            _Config.tabNames.Remove(name);
            File.Delete(NameToLayoutPath(name));
            SaveToJson();
            LayoutDeleted?.Invoke();
        }

        public static bool TryRenameLayout(string oldName, string newName)
        {
            try
            {
                File.Move(NameToLayoutPath(oldName), NameToLayoutPath(newName));
                _Config.tabNames[_Config.tabNames.IndexOf(oldName)] = newName;
                SaveToJson();
                if (GetLastLoadedLayoutName() == oldName)
                    SaveLayout(newName);
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
            CollectionAssert.AreEquivalent(_Config.tabNames, tabNames);
            _Config.tabNames = tabNames;
            SaveToJson();
        }

        private static string NameToLayoutPath(string name)
        {
            return Path.Combine(LayoutsPath, $"{name}.wlt");
        }
    }
}