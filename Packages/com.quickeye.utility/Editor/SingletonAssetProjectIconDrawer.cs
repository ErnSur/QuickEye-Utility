using System.IO;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    using UnityEditor;

    [InitializeOnLoad]
    internal static class SingletonAssetProjectIconDrawer
    {
        private const string LinkedIcon =
            "Packages/com.quickeye.utility/Editor/Icons/Linked.png";

        private const string UnlinkedIcon =
            "Packages/com.quickeye.utility/Editor/Icons/Unlinked.png";

        private static GUIStyle IconLabelStyle => new GUIStyle(EditorStyles.label)
        {
            margin = new RectOffset(),
            padding = new RectOffset()
        };

        static SingletonAssetProjectIconDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }

        private static void OnPostHeaderGUI(Editor editor)
        {
            if (editor.targets.Length > 1 ||
                !EditorUtility.IsPersistent(editor.target) ||
                !SingletonAssetCache.TryGetEntry(editor.serializedObject.targetObject, out var metadata) ||
                metadata.SingletonAssetAttribute == null)
                return;

            var resPath = metadata.ResourcesPath;
            var hasCorrectPath = metadata.IsInLoadablePath;
            using (new GUILayout.HorizontalScope())
            {
                var iconContent = new GUIContent(GetGuiContent(hasCorrectPath, resPath));
                iconContent.text = "Singleton Asset";
                using (new EditorGUIUtility.IconSizeScope(new Vector2(17, 17)))
                    GUILayout.Label(iconContent, GUILayout.ExpandWidth(false));
                GUI.enabled = false;
                EditorGUILayout.TextField($"*/Resources/{resPath}");
                GUI.enabled = true;
            }
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

        private static void DrawProjectItem(Rect rect, string path, SingletonAssetCache.AssetMetadata meta)
        {
            const int projWindowIconWidth = 16;
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
            var linkedIcon = GetGuiContent(meta.IsInLoadablePath, meta.ResourcesPath);
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