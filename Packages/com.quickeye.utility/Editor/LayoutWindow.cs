using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace QuickEye.Utility.Editor
{
    internal class LayoutWindow : EditorWindow
    {
        [SerializeField]
        private string layoutPath;

        [SerializeField]
        private List<LayoutButton> _buttons = new List<LayoutButton>()
        {
            new LayoutButton("layouts/test1.wlt"),
            new LayoutButton("layouts/test2.wlt"),
            new LayoutButton("layouts/test3.wlt"),
        };

        
        
        [MenuItem("Window/Layout Palette")]
        public static void OpenWindow()
        {
            var wnd =GetWindow<LayoutWindow>();
            
        }

        private void OnGUI()
        {
            using (new HorizontalScope())
            {
                foreach (var button in _buttons)
                {
                    button.GUI();
                }
            }
        }
    }

    public class LayoutButton
    {
        public string path;
        private static string _currentLayoutPath;
        public LayoutButton(string path)
        {
            this.path = path;
        }

        public void GUI()
        {
            
            if (Button(Path.GetFileName(path)) && _currentLayoutPath != path)
            {
                WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                _currentLayoutPath = path;
                if(!File.Exists(path))
                    WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                WindowLayoutHelper.LoadLayout(_currentLayoutPath);
            }
        }
    }

    public static class WindowLayoutHelper
    {
        public static void LoadLayout(string path) => EditorUtility.LoadWindowLayout(path);

        public static void SaveLayout(string path)
        {
            var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.WindowLayout");
            Debug.Log($"$MES$: {type}");
            var method = type.GetMethod("SaveWindowLayout", BindingFlags.Public | BindingFlags.Static);
            Debug.Log($"$MES$: {method}");
            method.Invoke(null, new object[] { path });
        }
    }
}