using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    internal static class MainToolbarExtender
    {
        private static ScriptableObject mainToolbar;

        //[InitializeOnLoadMethod]
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
            //EditorApplication.delayCall += () => left.Insert(2,new LayoutToolbar());
        }
    }
}