using UnityEditor;
using UnityEngine;

namespace OneAsset.Editor.UI
{
    internal static class LoadableAssetGUI
    {
        private const string LinkedIcon =
            "Packages/com.quickeye.utility/OneAsset/Editor/UI/Icons/Linked.png";

        private const string UnlinkedIcon =
            "Packages/com.quickeye.utility/OneAsset/Editor/UI/Icons/Unlinked.png";
        
        public static GUIContent GetGuiContent(bool isCorrectPath, string resourcesPath, string typeName)
        {
            var iconContent = isCorrectPath
                ? EditorGUIUtility.IconContent(LinkedIcon)
                : EditorGUIUtility.IconContent(UnlinkedIcon);
            iconContent.tooltip = isCorrectPath
                ? $"{typeName} can be loaded from this path"
                : $"{typeName} won't load from this path. Load path:\n\"Resources/{resourcesPath}\"";
            return iconContent;
        }
    }
}