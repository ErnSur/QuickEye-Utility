using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;
using Directory = UnityEditor.ShortcutManagement.Directory;

namespace QuickEye.Utility.Editor
{
    [EditorWindowTitle(title = "Rename Layout")]
    internal class RenameWindowLayout : EditorWindow
    {
        private const int Width = 200;
        private const int Height = 48;
        private const int HelpBoxHeight = 40;

        private static readonly string _InvalidChars = EditorUtility.GetInvalidFilenameChars();
        private static readonly string _InvalidCharsFormatString = L10n.Tr("Invalid characters: {0}");
        private string _currentInvalidChars = "";
        private bool _didFocus;
        private string _newLayoutName;
        private string _originalLayoutName;

        private void OnEnable()
        {
            titleContent = GetLocalizedTitleContent();
        }

        private void OnGUI()
        {
            GUILayout.Space(5);
            var evt = Event.current;
            var hitEnter = evt.type == EventType.KeyDown &&
                           (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter);
            var hitEscape = evt.type == EventType.KeyDown && evt.keyCode == KeyCode.Escape;
            if (hitEscape)
            {
                Close();
                GUIUtility.ExitGUI();
            }

            GUI.SetNextControlName("m_PreferencesName");
            EditorGUI.BeginChangeCheck();
            _newLayoutName = EditorGUILayout.TextField(_newLayoutName);
            _newLayoutName = _newLayoutName.TrimEnd();
            if (EditorGUI.EndChangeCheck())
                UpdateCurrentInvalidChars();

            if (!_didFocus)
            {
                _didFocus = true;
                EditorGUI.FocusTextInControl("m_PreferencesName");
            }

            if (_currentInvalidChars.Length != 0)
            {
                EditorGUILayout.HelpBox(string.Format(_InvalidCharsFormatString, _currentInvalidChars),
                    MessageType.Warning);
                minSize = new Vector2(Width, Height + HelpBoxHeight);
            }
            else
            {
                minSize = new Vector2(Width, Height);
            }

            var canSaveLayout = _newLayoutName.Length > 0 && _currentInvalidChars.Length == 0;
            EditorGUI.BeginDisabled(!canSaveLayout);

            if (GUILayout.Button("Rename") || (hitEnter && canSaveLayout))
            {
                Close();

                LayoutTabManager.TryRenameLayout(_originalLayoutName, _newLayoutName);
            }
            else
            {
                _didFocus = false;
            }

            EditorGUI.EndDisabled();
        }

        internal static RenameWindowLayout ShowWindow(string layoutName)
        {
            var w = GetWindowDontShow<RenameWindowLayout>();
            w._newLayoutName = w._originalLayoutName = layoutName;
            w.minSize = w.maxSize = new Vector2(Width, Height);
            w.ShowAuxWindow();
            return w;
        }

        private void UpdateCurrentInvalidChars()
        {
            _currentInvalidChars = new string(_newLayoutName.Intersect(_InvalidChars).Distinct().ToArray());
        }
    }
}