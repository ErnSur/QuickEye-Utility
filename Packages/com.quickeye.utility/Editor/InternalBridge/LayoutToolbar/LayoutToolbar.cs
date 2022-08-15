using System.Linq;
using QuickEye.UIToolkit;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    public class LayoutToolbar : VisualElement
    {
        private Button _addTabButton;
        private TabGroup _group;

        public LayoutToolbar()
        {
            Init();
        }

        private void Init()
        {
            var tree = Resources.Load<VisualTreeAsset>("com.quickeye.utility/LayoutTabView");
            tree.CloneTree(this);
            _addTabButton = this.Q<Button>("add-button");
            _addTabButton.clicked += () =>
            {
                var layoutName = GetNewTabName();
                LayoutTabManager.SaveLayout(layoutName);
                AddTab(layoutName);
            };
            _group = this.Q<TabGroup>();
            _group.name = "layout-tab-group";
            RecreateTabs();
            LayoutTabManager.LayoutDeleted += RecreateTabs;
            LayoutTabManager.LayoutRenamed += RecreateTabs;

            InjectSavingTabLayoutBeforeRegularLayoutLoading();
            _group.RegisterCallback<ChildOrderChangedEvent>(_ =>
            {
                var tabNames = _group.Children().OfType<LayoutTab>().Select(t => t.LayoutTabName).ToList();
                LayoutTabManager.UpdateTabOrder(tabNames);
            });
        }

        private void InjectSavingTabLayoutBeforeRegularLayoutLoading()
        {
            RegisterCallback<AttachToPanelEvent>(evt =>
            {
                var layoutDropdown = panel.visualTree.Q<LayoutDropdown>();
                if (layoutDropdown == null)
                    return;

                layoutDropdown.RegisterCallback<PointerDownEvent>(
                    evt => { LayoutTabManager.TrySaveCurrentTabLayout(); });
                var clickable = layoutDropdown.clickable;
                layoutDropdown.RemoveManipulator(clickable);
                layoutDropdown.AddManipulator(clickable);
            });
        }

        private static string GetNewTabName()
        {
            var names = LayoutTabManager.GetTabLayouts();
            var newName = "New Tab";
            var index = 0;
            while (names.Contains(newName))
                newName = $"New Tab {++index}";

            return newName;
        }

        private void RecreateTabs()
        {
            _group.Clear();
            foreach (var layoutName in LayoutTabManager.GetTabLayouts())
                AddTab(layoutName);
        }

        private void AddTab(string layoutName)
        {
            var tab = new LayoutTab(layoutName);
            tab.IsReorderable = true;
            _group.Add(tab);
            if (layoutName == LayoutTabManager.GetLastLoadedLayoutName())
                tab.SetValueWithoutNotify(true);
        }
    }
}