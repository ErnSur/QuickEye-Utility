using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    internal class FloatingWindowWindow : EditorWindow
    {
        [MenuItem("Test/ShowModal")]
        private static void OpenModal()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowModal();
        }
        
        [MenuItem("Test/ShowUtility")]
        private static void OpenUtility()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowUtility();
        }
        
        [MenuItem("Test/Show")]
        private static void Open()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.Show();
        }
        
        [MenuItem("Test/ShowPopup")]
        private static void OpenPopup()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowPopup();
        }
        
        [MenuItem("Test/ShowAsDropDown")]
        private static void OpenAsDropDown()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowAsDropDown(new Rect(0, 0, 100, 100), new Vector2(100, 100));
        }
        
        [MenuItem("Test/ShowAuxWindow")]
        private static void OpenAuxWindow()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowAuxWindow();
        }
        
        [MenuItem("Test/ShowTab")]
        private static void OpenTab()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowTab();
        }
        
        [MenuItem("Test/ModalUtility")]
        private static void OpenModalUtility()
        {
            var so = CreateInstance<FloatingWindowWindow>();
            so.ShowModalUtility();
        }
        
        

        private void OnGUI()
        {
            if (GUILayout.Button("close"))
                Close();
        }
    }
}