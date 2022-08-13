using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    public static class LayoutTabManager
    {
        public const string LayoutsPath  = "UserSettings/Layouts/Tabs/";

        private static string currentLayout;

        static LayoutTabManager()
        {
            if (!Directory.Exists(LayoutsPath))
            {
                Directory.CreateDirectory(LayoutsPath);
            }
        }
        
        public static string GetLastLoadedLayout()
        {
            return Toolbar.lastLoadedLayoutName;
        }
        
        public static string[] GetTabLayouts()
        {
            return Directory.GetFiles(LayoutsPath).Select(Path.GetFileNameWithoutExtension).ToArray();
        }

        public static void RenameLayout(string oldName, string newName)
        {
            File.Move(NameToLayoutPath(oldName),NameToLayoutPath(newName));
        }

        public static void SaveLayout(string name)
        {
            Toolbar.lastLoadedLayoutName = name;
            WindowLayout.SaveWindowLayout(NameToLayoutPath(name));
        }
        
        public static void LoadLayout(string name)
        {
            Debug.Log($"Load layout, Last loaded:{GetLastLoadedLayout()}");

            if (IsTabLayout(GetLastLoadedLayout()))
                SaveLayout(GetLastLoadedLayout());

            WindowLayout.LoadWindowLayout(NameToLayoutPath(name),false,true,true);
            Debug.Log($"Loaded Layout: {GetLastLoadedLayout()}");
        }

        private static bool IsTabLayout(string layoutName)
        {
            return GetTabLayouts().Contains(layoutName);
        }
        
        public static void DeleteLayout(string name)
        {
            File.Delete(NameToLayoutPath(name));
        }

        private static string NameToLayoutPath(string name) => Path.Combine(LayoutsPath, $"{name}.wlt");
        
        private  static bool IsFileLocked(FileInfo file)
        {
            try
            {
                using(FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
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