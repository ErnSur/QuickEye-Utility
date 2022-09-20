#if UNITY_SETTINGS_MANAGER
using System.Reflection;
using UnityEditor;
using UnityEditor.SettingsManagement;
using UnityEngine;

namespace QuickEye.Utility.Editor.WindowTitle
{
    internal static class WindowTitleSettings
    {
        private static Settings _instance;

        public static Settings Instance =>
            _instance ?? (_instance = new Settings(new[] { new UserSettingsRepository() }));

        private static readonly string _DisabledTextColorTag =
            $"<color=#{ColorUtility.ToHtmlStringRGB(EditorColorPalette.Current.DefaultText)}{128:X2}>";

        [UserSetting]
        private static readonly UserSetting<bool> _EnableCustomTitle =
            new UserSetting<bool>(Instance, "qe.window-title.enable", false, SettingsScope.User);

        [UserSetting]
        private static readonly UserSetting<string> _FormatString =
            new UserSetting<string>(Instance, "qe.window-title.format-string", "<RepoDirName> | <Branch>",
                SettingsScope.User);

        [UserSetting]
        private static readonly UserSetting<string> _RepositoryPath =
            new UserSetting<string>(Instance, "qe.window-title.repo-path", "./", SettingsScope.User);

        public static bool EnableCustomTitle => _EnableCustomTitle.value;
        public static string WindowTitle => TitleFormatter.Format(_FormatString.value);
        public static string RepositoryPath => _RepositoryPath.value;

        [UserSettingBlock(" ")]
        private static void OnGUI(string searchContext)
        {
            var style = new GUIStyle(EditorStyles.helpBox);
            style.fontSize = 13;
            style.richText = true;
            EditorGUI.BeginChangeCheck();

            _EnableCustomTitle.value =
                SettingsGUILayout.SettingsToggle("Enable Custom Window Title", _EnableCustomTitle, searchContext);
            using (new EditorGUI.DisabledScope(!_EnableCustomTitle.value))
            {
                _FormatString.value =
                    SettingsGUILayout.SettingsTextField("Window Title Format String", _FormatString, searchContext);
                _RepositoryPath.value =
                    SettingsGUILayout.SettingsTextField(
                        new GUIContent("Git Repository Path", "Git repository root directory"), _RepositoryPath,
                        searchContext);
                var parametersInfoBox = $@"Available title parameters:
    • <Branch> {_DisabledTextColorTag}{TitleFormatter.Format("<Branch>")}</color>
    • <SceneName> {_DisabledTextColorTag}{TitleFormatter.Format("<SceneName>")}</color>
    • <ProjectName> {_DisabledTextColorTag}{TitleFormatter.Format("<ProjectName>")}</color>
    • <RepoDirName> {_DisabledTextColorTag}{TitleFormatter.Format("<RepoDirName>")}</color>
    • <ProjectPath> {_DisabledTextColorTag}{TitleFormatter.Format("<ProjectPath>")}</color>
    • <RepoPath> {_DisabledTextColorTag}{TitleFormatter.Format("<RepoPath>")}</color>
    • <EditorVersion> {_DisabledTextColorTag}{TitleFormatter.Format("<EditorVersion>")}</color>
    • <TargetPlatform> {_DisabledTextColorTag}{TitleFormatter.Format("<TargetPlatform>")}</color>";
                GUILayout.Label(parametersInfoBox, style);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Instance.Save();
                UpdateWindowTitle();
            }
        }

        private static void UpdateWindowTitle()
        {
            try
            {
                var type = typeof(EditorApplication);
                var method = type.GetMethod("UpdateMainWindowTitle", BindingFlags.Static | BindingFlags.NonPublic);
                method.Invoke(null, null);
            }
            catch
            {
            }
        }
    }
}
#endif