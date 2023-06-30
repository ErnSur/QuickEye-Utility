using UnityEditor;
using UnityEngine;

namespace UnityOne.Editor.EditorGUIExtension
{
    internal static class SingletonGUI
    {
        private const string LinkedIcon =
            "Packages/com.quickeye.utility/Editor/Icons/Linked.png";

        private const string UnlinkedIcon =
            "Packages/com.quickeye.utility/Editor/Icons/Unlinked.png";
        
        public static GUIContent GetGuiContent(bool isCorrectPath, string resourcesPath)
        {
            var iconContent = isCorrectPath
                ? EditorGUIUtility.IconContent(LinkedIcon)
                : EditorGUIUtility.IconContent(UnlinkedIcon);
            iconContent.tooltip = isCorrectPath
                ? "Singleton Asset is loaded from this path"
                : $"Singleton Asset is not in correct path:\n\"Resources/{resourcesPath}\"";
            return iconContent;
        }
    }
}