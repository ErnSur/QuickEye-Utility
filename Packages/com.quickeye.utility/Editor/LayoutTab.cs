using System.IO;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutTab : Tab
    {
        public string path;
        private static string _currentLayoutPath;

        public LayoutTab(string path)
        {
            this.path = path;
            Text = Path.GetFileNameWithoutExtension(path);
            //AddToClassList("unity-toolbar-button");
            this.RegisterValueChangedCallback(e =>
            {
                if (_currentLayoutPath != path)
                {
                    WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                    _currentLayoutPath = path;
                    if (!File.Exists(path))
                        WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                    WindowLayoutHelper.LoadLayout(_currentLayoutPath);
                }
            });
        }
    }
}