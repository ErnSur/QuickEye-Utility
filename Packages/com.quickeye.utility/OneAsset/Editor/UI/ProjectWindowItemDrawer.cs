using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.UI
{
    using static LoadableAssetGUI;

    [InitializeOnLoad]
    internal static class ProjectWindowItemDrawer
    {
        private static Color WindowBackground => EditorGUIUtility.isProSkin
            ? new Color32(0x38, 0x38, 0x38, 255)
            : new Color32(0xC8, 0xC8, 0xC8, 255);

        private static GUIStyle IconLabelStyle => new GUIStyle(EditorStyles.label)
        {
            margin = new RectOffset(),
            padding = new RectOffset()
        };

        private static bool _enableGui = true;

        static ProjectWindowItemDrawer()
        {
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        }

        private static void ProjectWindowItemOnGUI(string guid, Rect rect)
        {
            if (!_enableGui)
                return;
            try
            {
                if (!LoadFromAssetCache.TryGetEntry(guid, out var metadata) ||
                    metadata.FirstLoadFromAssetAttribute == null)
                    return;
                if (rect.height > EditorGUIUtility.singleLineHeight)
                    DrawProjectGridItem(rect, metadata);
                else
                    DrawProjectItem(rect, AssetDatabase.GUIDToAssetPath(guid), metadata);
            }
            catch (Exception e)
            {
                _enableGui = false;
                Debug.LogError($"Project Browser extension disabled. Exception: {e}");
                throw;
            }
        }


        private static void DrawProjectItem(Rect rect, string path, AssetMetadata meta)
        {
            var projectItemLabelContent = new GUIContent(Path.GetFileNameWithoutExtension(path));
            var linkedIconRect = CalculateRectAfterLabelText(rect, projectItemLabelContent, true);
            var isInLoadablePath = meta.IsInLoadablePath(out _);
            var linkedIcon = GetGuiContent(isInLoadablePath, meta.FirstResourcesPath, meta.TypeName);
            using (new EditorGUIUtility.IconSizeScope(new Vector2(16, 16)))
                GUI.Label(linkedIconRect, linkedIcon, IconLabelStyle);
        }

        private static void DrawProjectGridItem(Rect rect, AssetMetadata meta)
        {
            var content = GetGuiContent(meta.IsInLoadablePath(out _), meta.FirstResourcesPath, meta.TypeName);
            var iconRect = new Rect(rect)
            {
                size = new Vector2(rect.size.y, rect.size.y) / 3
            };

            using (new EditorGUIUtility.IconSizeScope(iconRect.size))
            {
                GUI.color = WindowBackground;
                using (new EditorGUIUtility.IconSizeScope(iconRect.size + Vector2.one))
                    GUI.Label(iconRect, EditorGUIUtility.IconContent("Folder On Icon"), IconLabelStyle);
                GUI.color = Color.white;
                GUI.Label(iconRect, content, IconLabelStyle);
            }
        }

        private static Rect CalculateRectAfterLabelText(Rect rect, GUIContent content, bool hasIcon)
        {
            var labelTextSize = new GUIStyle("label").CalcSize(content);
            var itemLabel = new Rect(rect)
            {
                width = labelTextSize.x
            };
            if (hasIcon)
                itemLabel.width += 16;
            var linkedIconRect = new Rect(rect)
            {
                x = itemLabel.xMax + 2,
                xMax = rect.xMax
            };
            return linkedIconRect;
        }
    }
}