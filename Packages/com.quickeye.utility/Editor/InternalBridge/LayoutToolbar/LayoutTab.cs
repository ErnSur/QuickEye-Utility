using System.IO;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutTab : TabDropdown
    {
        public readonly string LayoutName;
        private static string _currentLayoutLayoutName;

        public LayoutTab(string layoutName)
        {
            LayoutName = layoutName;
            Text = layoutName;

            BeforeMenuShow += menu =>
            {
                menu.AddItem("Rename",false,()=>{ Debug.Log($"MES: Rename");});
                menu.AddSeparator("");
                menu.AddItem("Delete",false,()=>{LayoutTabManager.DeleteLayout(LayoutName);});
            };

            this.RegisterValueChangedCallback(e =>
            {
                if (LayoutTabManager.GetLastLoadedLayout() != LayoutName)
                {
                    
                    LayoutTabManager.LoadLayout(LayoutName);
                    // TabLayoutManager.SaveLayout(_currentLayoutLayoutName);
                    // _currentLayoutLayoutName = layoutName;
                    // if (!File.Exists(layoutName))
                    //     WindowLayoutHelper.SaveLayout(_currentLayoutLayoutName);
                    // WindowLayoutHelper.LoadLayout(_currentLayoutLayoutName);
                }
            });
        }
    }
}