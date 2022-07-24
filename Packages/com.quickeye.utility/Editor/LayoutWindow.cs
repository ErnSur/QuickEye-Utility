using System;
using System.Collections.Generic;
using System.Reflection;
using QuickEye.UIToolkit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    internal class LayoutWindow
    {
        private static List<LayoutTab> _tabs = new List<LayoutTab>()
        {
            new LayoutTab("layouts/test1.wlt"),
            new LayoutTab("layouts/test2.wlt"),
            new LayoutTab("layouts/test3.wlt"),
        };

        private static ScriptableObject mainToolbar;

        [InitializeOnLoadMethod]
        public static void Reg()
        {
            EditorApplication.update += () =>
            {
                if (mainToolbar == null)
                    SetupToolbar();
            };
        }

        private static void SetupToolbar()
        {
            mainToolbar = ReflectionHelper.GetMainToolbar();
            var root = ReflectionHelper.GetToolbarRoot();
            var left = root.Q("ToolbarZoneLeftAlign");
            CreateTabGroup(left);
        }

        private static void CreateTabGroup(VisualElement root)
        {
            var group = new TabGroup();
            group.AddToClassList("unity-toolbar-button");
            foreach (var tab in _tabs)
            {
                group.Add(tab);
            }
            root.Add(group);
        }
    }
}