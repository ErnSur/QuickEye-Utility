using System;
using System.Reflection;
using UnityEditor;

namespace QuickEye.Utility.Editor.WindowTitle
{
    internal static class MainWindowTitleUpdater
    {
        [InitializeOnLoadMethod]
        private static void UpdateWindowTitle()
        {
            EditorApplication.updateMainWindowTitle += descriptor =>
            {
                if (WindowTitleSettings.EnableCustomTitle)
                {
                    descriptor.title = WindowTitleSettings.WindowTitle;
                }
            };
        }
    }
}