using System.IO;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutTab : TabDropdown
    {
        public string path;
        private static string _currentLayoutPath;

        public LayoutTab(string path)
        {
            this.path = path;
            Text = Path.GetFileNameWithoutExtension(path);

            BeforeMenuShow += menu =>
            {
                menu.AddItem("Rename",false,()=>{ Debug.Log($"MES: Rename");});
                menu.AddSeparator("");
                menu.AddItem("Delete",false,()=>{});
            };

            this.RegisterValueChangedCallback(e =>
            {
                if (_currentLayoutPath != path)
                {
                    if (IsFileLocked(new FileInfo(path)))
                    {
                        Debug.Log($"File is Locked {path}");
                        return;
                    }
                    if (!string.IsNullOrEmpty(_currentLayoutPath) && IsFileLocked(new FileInfo(_currentLayoutPath)))
                    {
                        Debug.Log($"File is Locked {_currentLayoutPath}");
                        return;
                    }
                    WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                    _currentLayoutPath = path;
                    if (!File.Exists(path))
                        WindowLayoutHelper.SaveLayout(_currentLayoutPath);
                    WindowLayoutHelper.LoadLayout(_currentLayoutPath);
                }
            });
        }
        
        private  bool IsFileLocked(FileInfo file)
        {
            try
            {
                using(FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}