using System.Collections.Generic;
using QuickEye.UIToolkit;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutToolbar : VisualElement
    {
        private List<LayoutTab> _tabs = new List<LayoutTab>();

        private Button _addTabButton;
        private TabGroup _group;
        public LayoutToolbar()
        {
            Debug.Log($"Recreating layout: {LayoutTabManager.GetLastLoadedLayout()}");
            Init();
            
        }

        private void Init()
        {
            var tree = Resources.Load<VisualTreeAsset>("com.quickeye.utility/LayoutTabView");
            tree.CloneTree(this);
            _addTabButton = this.Q<Button>("add-button");
            _addTabButton.clicked += () =>
            {
                var index =LayoutTabManager.GetTabLayouts().Length + 1;
                var layoutName = $"Layout {index}";
                LayoutTabManager.SaveLayout(layoutName);
                AddTab(layoutName);
            };
            _group = this.Q<TabGroup>();
            foreach (var layoutName in LayoutTabManager.GetTabLayouts())
            {
                AddTab(layoutName);
            }
        }

        private void AddTab(string layoutName)
        {
            var tab = new LayoutTab(layoutName);
            _group.Add(tab);
            if (layoutName == LayoutTabManager.GetLastLoadedLayout())
                tab.SetValueWithoutNotify(true);
        }
    }
}