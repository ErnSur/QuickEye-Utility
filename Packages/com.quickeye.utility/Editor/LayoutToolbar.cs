using System.Collections.Generic;
using QuickEye.UIToolkit;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutToolbar : VisualElement
    {
        private List<LayoutTab> _tabs = new List<LayoutTab>()
        {
            new LayoutTab("layouts/Layout 1.wlt"),
            new LayoutTab("layouts/Layout 2.wlt"),
            new LayoutTab("layouts/Layout 3.wlt"),
        };

        public LayoutToolbar()
        {
            Init();
        }

        private void Init()
        {
            var tree = Resources.Load<VisualTreeAsset>("com.quickeye.utility/LayoutTabView");
            tree.CloneTree(this);
            var group = this.Q<TabGroup>();
            foreach (var tab in _tabs)
            {
                group.Add(tab);
            }
        }
    }
}