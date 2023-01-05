#if UNITY_SETTINGS_MANAGER
using UnityEditor;
using UnityEditor.SettingsManagement;

namespace QuickEye.Utility.Editor.WindowTitle
{
    internal static class WindowTitleSettingsProvider
    {
        [SettingsProvider]
        private static SettingsProvider CreateSettingsProvider()
        {
            var provider = new UserSettingsProvider("Preferences/Window Title",
                WindowTitleSettings.Instance,
                new[] { typeof(WindowTitleSettingsProvider).Assembly });

            return provider;
        }
    }
}
#endif