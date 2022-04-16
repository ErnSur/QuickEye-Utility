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
        private static readonly Dictionary<string, SingletonAssetAttribute> CachedAssets =
            new Dictionary<string, SingletonAssetAttribute>();

        private static GUIStyle IconLabelStyle => new GUIStyle(EditorStyles.label)
        {
            margin = new RectOffset(),
            padding = new RectOffset()
        };

        private const string LinkedIcon =
            "Packages/com.quickeye.utility/Editor/Resources/com.quickeye.utility/Linked.png";

        private const string UnlinkedIcon =
            "Packages/com.quickeye.utility/Editor/Resources/com.quickeye.utility/Unlinked.png";

        static SingletonAssetProjectIconDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect rect)
        {
            if (!CachedAssets.TryGetValue(guid, out var attr))
                CacheItem(guid);
            if (attr == null)
                return;
            if (rect.height > EditorGUIUtility.singleLineHeight)
                DrawItemOnGrid(rect, AssetDatabase.GUIDToAssetPath(guid), attr);
            else
                DrawItem(rect, AssetDatabase.GUIDToAssetPath(guid), attr);
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
            const int projWindowIconWidth = 16;
            var resPath = GetResPath(path);
            var hasCorrectPath = resPath == attribute.ResourcesPath;

            var content = new GUIContent(Path.GetFileNameWithoutExtension(path));
            var labelTextSize = new GUIStyle("label").CalcSize(content);
            var itemLabel = new Rect(rect)
            {
                width = labelTextSize.x + projWindowIconWidth
            };
            var linkedIconRect = new Rect(rect)
            {
                x = itemLabel.xMax + 2,
                xMax = rect.xMax
            };
            var linkedIcon = GetGuiContent(hasCorrectPath, attribute.ResourcesPath);
            GUI.Label(linkedIconRect, linkedIcon, IconLabelStyle);
        }

        private static void DrawItemOnGrid(Rect rect, string path, SingletonAssetAttribute attribute)
        {
            var resPath = GetResPath(path);
            var hasCorrectPath = resPath == attribute.ResourcesPath;
            var content = GetGuiContent(hasCorrectPath, attribute.ResourcesPath);
            var iconRect = new Rect(rect)
            {
                size = new Vector2(rect.size.y, rect.size.y) / 3
            };

            using (new EditorGUIUtility.IconSizeScope(iconRect.size))
            {
                GUI.color = EditorColorPalette.Current.WindowBackground;
                using (new EditorGUIUtility.IconSizeScope(iconRect.size + Vector2.one))
                    GUI.Label(iconRect, EditorGUIUtility.IconContent("Folder On Icon"), IconLabelStyle);
                GUI.color = Color.white;
                GUI.Label(iconRect, content, IconLabelStyle);
            }
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

            CachedAssets[guid] = attribute;
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