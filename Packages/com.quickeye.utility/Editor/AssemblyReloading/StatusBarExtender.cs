using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    [InitializeOnLoad]
    internal static class StatusBarExtender
    {
        private const int MaxRetries = 5;

        private static Type _appStatusBarType;
        private static PropertyInfo _windowBackendProperty;
        private static PropertyInfo _visualTreeProperty;
        private static MethodInfo _addDefaultEditorStyleSheetsMethod;
        public static event Action<VisualElement> StatusBarCreated;
        private static object _statusBarGuiView;
        private static int _retries;

        static StatusBarExtender()
        {
            if (TryInitializeTypeMembers())
                // Status bar instance can be destroyed and recreated at any time (i.e., when changing window layout) That's why we need to try to invoke the event every frame.
                EditorApplication.update += TryInvokeStatusBarCreatedEvent;
        }

        private static bool TryInitializeTypeMembers()
        {
            try
            {
                var guiViewType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.GUIView");
                _windowBackendProperty =
                    guiViewType.GetProperty("windowBackend", BindingFlags.Instance | BindingFlags.NonPublic);
                var windowBackendType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IWindowBackend");
                _visualTreeProperty =
                    windowBackendType.GetProperty("visualTree", BindingFlags.Instance | BindingFlags.Public);
                _appStatusBarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.AppStatusBar");
                var uIElementsEditorUtilityType =
                    typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.UIElements.UIElementsEditorUtility");
                _addDefaultEditorStyleSheetsMethod = uIElementsEditorUtilityType.GetMethod(
                    "AddDefaultEditorStyleSheets",
                    BindingFlags.Static | BindingFlags.NonPublic);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize status bar extender: {e}");
                return false;
            }

            return true;
        }

        private static void TryInvokeStatusBarCreatedEvent()
        {
            if (_statusBarGuiView != null)
                return;
            if (!TryGetAppStatusBarGuiView(out _statusBarGuiView))
            {
                if (_retries++ >= MaxRetries)
                    EditorApplication.update -= TryInvokeStatusBarCreatedEvent;
                return;
            }


            try
            {
                var visualElement = GetStatusBarVisualElement();
                StatusBarCreated?.Invoke(visualElement);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to extend status bar: {e}");
                EditorApplication.update -= TryInvokeStatusBarCreatedEvent;
            }
        }

        private static VisualElement GetStatusBarVisualElement()
        {
            var windowBackend = _windowBackendProperty.GetValue(_statusBarGuiView);
            var visualTree = (VisualElement)_visualTreeProperty.GetValue(windowBackend);
            _addDefaultEditorStyleSheetsMethod.Invoke(null, new object[] { visualTree });
            var originalBar = visualTree.Q<IMGUIContainer>();
            originalBar.AddToClassList("status-bar");
            return visualTree;
        }

        private static bool TryGetAppStatusBarGuiView(out object assStatusbar)
        {
            assStatusbar = Resources.FindObjectsOfTypeAll(_appStatusBarType).FirstOrDefault();
            return assStatusbar != null;
        }
    }
}