using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutButton : Button
    {
        public string path;
        private static string _currentLayoutPath;

        public LayoutButton(string path)
        {
            this.path = path;
            text = Path.GetFileName(path);
            AddToClassList("unity-toolbar-button");
            clicked += () =>
            {
                if (_currentLayoutPath != path)
                {
                    WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                    _currentLayoutPath = path;
                    if (!File.Exists(path))
                        WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                    WindowLayoutHelper.LoadLayout(_currentLayoutPath);
                }
            };
        }
    }
}