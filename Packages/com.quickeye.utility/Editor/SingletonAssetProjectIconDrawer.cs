using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility.Editor
{
    [InitializeOnLoad]
    internal static class SingletonAssetProjectIconDrawer
    {
        private static readonly Dictionary<string, SingletonAssetAttribute> cachedAssets =
            new Dictionary<string, SingletonAssetAttribute>();

        private static GUIStyle IconLabelStyle => new GUIStyle(EditorStyles.label)
        {
            alignment = TextAnchor.MiddleRight
        };

        private const string LinkedIcon = "Packages/com.quickeye.utility/Editor/Resources/com.quickeye.utility/Linked.png";
        private const string UnlinkedIcon = "Packages/com.quickeye.utility/Editor/Resources/com.quickeye.utility/Unlinked.png";

        static SingletonAssetProjectIconDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
        {
            if (!cachedAssets.TryGetValue(guid, out var attr))
                CacheItem(guid);
            if (attr == null)
                return;
            DrawItem(selectionRect, AssetDatabase.GUIDToAssetPath(guid), attr);
        }

        private static GUIContent GetGuiContent(bool isCorrectPath, string resourcesPath)
        {
            var iconContent = isCorrectPath
                ? EditorGUIUtility.IconContent(LinkedIcon)
                : EditorGUIUtility.IconContent(UnlinkedIcon);
            iconContent.tooltip = isCorrectPath
                ? "Singleton Asset is loaded from this path"
                : $"Singleton Asset is not in correct path:\n\"Resources/{resourcesPath}\"";
            return iconContent;
        }

        private static void DrawItem(Rect rect, string path, SingletonAssetAttribute attribute)
        {
            var resPath = GetResPath(path);
            var hasCorrectPath = resPath == attribute.ResourcesPath;
            var content = GetGuiContent(hasCorrectPath, attribute.ResourcesPath);
            var bRect = new Rect(rect)
            {
                xMin = rect.xMax - 30,
                xMax = rect.xMax
            };
            GUI.Label(bRect, content, IconLabelStyle);
        }

        private static void CacheItem(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            Object singleton = AssetDatabase.LoadAssetAtPath<ScriptableSingleton>(path);
            if (singleton == null)
                singleton = AssetDatabase.LoadAssetAtPath<Singleton>(path);

            SingletonAssetAttribute attribute = null;
            if (singleton != null)
            {
                attribute = singleton.GetType().GetCustomAttribute<SingletonAssetAttribute>();
            }

            cachedAssets[guid] = attribute;
        }

        private static string GetResPath(string path)
        {
            path = Path.GetFullPath(path);
            path = Path.Combine(Path.GetDirectoryName(path) ?? "", Path.GetFileNameWithoutExtension(path));

            var resourcesFolder = $"{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}";
            var index = path.IndexOf(resourcesFolder, StringComparison.InvariantCulture);
            return index == -1 ? null : path.Substring(index + resourcesFolder.Length);
        }
    }
}