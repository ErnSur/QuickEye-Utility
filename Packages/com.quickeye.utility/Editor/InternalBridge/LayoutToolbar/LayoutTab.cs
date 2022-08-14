using System.IO;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutTab : TabDropdown
    {
        public readonly string LayoutTabName;
        private static string _currentLayoutLayoutName;

        public LayoutTab(string layoutTabName)
        {
            LayoutTabName = layoutTabName;
            Text = layoutTabName;

            BeforeMenuShow += menu =>
            {
                menu.AddItem("Rename", false, OnRename);
                menu.AddSeparator("");
                menu.AddItem("Delete", false, OnDelete);
            };

            this.RegisterValueChangedCallback(e =>
            {
                if (LayoutTabManager.GetLastLoadedLayoutName() != LayoutTabName)
                {
                    LayoutTabManager.LoadLayout(LayoutTabName);
                }
            });
        }

        private void OnDelete()
        {
            LayoutTabManager.DeleteLayout(LayoutTabName);
        }

        private void OnRename()
        {
            RenameWindowLayout.ShowWindow(LayoutTabName);
        }
    }
}