using System.IO;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace QuickEye.Utility.Editor
{
    public static class IMGUIUtility
    {
        public static Rect CalculateRectAfterLabelText(Rect rect, string projectWindowItemPath, bool hasIcon)
        {
            var content = new GUIContent(Path.GetFileNameWithoutExtension(projectWindowItemPath));
            if (projectWindowItemPath.StartsWith("Packages"))
            {
                var pi = PackageInfo.FindForAssetPath(projectWindowItemPath);
                if (!string.IsNullOrEmpty(pi.displayName))
                    content.text = pi.displayName;
            }

            return CalculateRectAfterLabelText(rect, content, hasIcon);
        }

        public static Rect CalculateRectAfterLabelText(Rect rect, GUIContent content, bool hasIcon)
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