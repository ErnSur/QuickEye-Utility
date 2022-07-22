using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    internal class LayoutWindow
    {
        private static List<LayoutButton> _buttons = new List<LayoutButton>()
        {
            new LayoutButton("layouts/test1.wlt"),
            new LayoutButton("layouts/test2.wlt"),
            new LayoutButton("layouts/test3.wlt"),
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
            foreach (var button in _buttons)
            {
                left.Add(button);
            }
        }
    }
}