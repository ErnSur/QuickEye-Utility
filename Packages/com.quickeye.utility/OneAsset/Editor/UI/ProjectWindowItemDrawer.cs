using System.IO;
using QuickEye.Utility.Editor;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.EditorGUIExtension
{
    using static SingletonGUI;

    [InitializeOnLoad]
    internal static class ProjectWindowItemDrawer
    {
        private static GUIStyle IconLabelStyle => new GUIStyle(EditorStyles.label)
        {
            margin = new RectOffset(),
            padding = new RectOffset()
        };

        static ProjectWindowItemDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect rect)
        {
            if (!SingletonAssetCache.TryGetEntry(guid, out var metadata) ||
                metadata.SingletonAssetAttribute == null)
                return;
            if (rect.height > EditorGUIUtility.singleLineHeight)
                DrawProjectGridItem(rect, metadata);
            else
                DrawProjectItem(rect, AssetDatabase.GUIDToAssetPath(guid), metadata);
        }


        private static void DrawProjectItem(Rect rect, string path, SingletonAssetCache.AssetMetadata meta)
        {
            var projectItemLabelContent = new GUIContent(Path.GetFileNameWithoutExtension(path));
            var linkedIconRect = IMGUIUtility.CalculateRectAfterLabelText(rect, projectItemLabelContent, true);
            var linkedIcon = GetGuiContent(meta.IsInLoadablePath, meta.ResourcesPath);
            using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
                GUI.Label(linkedIconRect, linkedIcon, IconLabelStyle);
        }

        private static void DrawProjectGridItem(Rect rect, SingletonAssetCache.AssetMetadata meta)
        {
            var content = GetGuiContent(meta.IsInLoadablePath, meta.ResourcesPath);
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
    }
}