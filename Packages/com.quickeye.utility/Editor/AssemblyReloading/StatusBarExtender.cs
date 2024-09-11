using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
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
                
                if (guiViewType == null)
                    throw new Exception("Failed to get GUIView type");
                
                
                _windowBackendProperty =
                    guiViewType.GetProperty("windowBackend", BindingFlags.Instance | BindingFlags.NonPublic);
                
                if (_windowBackendProperty == null)
                    throw new Exception("Failed to get windowBackend property");
                
                var windowBackendType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IWindowBackend");
                
                if(windowBackendType == null)
                    throw new Exception("Failed to get windowBackendType property");
                
                
                _visualTreeProperty =
                    windowBackendType.GetProperty("visualTree", BindingFlags.Instance | BindingFlags.Public);
                
                if(_visualTreeProperty == null)
                    throw new Exception("Failed to get visualTree property");
                
                
                _appStatusBarType = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.AppStatusBar");
                
                if(_appStatusBarType == null)
                    throw new Exception("Failed to get appStatusBarType property");
                
                
                //UnityEditor.UIElements.UIElementsEditorUtility.AddDefaultEditorStyleSheets();
                var uIElementsEditorUtilityType =
                    typeof(UnityEditor.UIElements.ObjectField).Assembly.GetType("UnityEditor.UIElements.UIElementsEditorUtility");
                
                if(uIElementsEditorUtilityType == null)
                    throw new Exception("Failed to get uIElementsEditorUtilityType property");
                
                
                _addDefaultEditorStyleSheetsMethod = GetMethod(uIElementsEditorUtilityType,
                    "AddDefaultEditorStyleSheets", true,
                    typeof(VisualElement));
                
                if(_addDefaultEditorStyleSheetsMethod == null)
                    throw new Exception("Failed to get addDefaultEditorStyleSheetsMethod property");
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to initialize status bar extender: {e}");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Returns a method with the specified name and argument types. It tires to find a public method, if it fails, it tries to find a non-public method.
        /// </summary>
        /// <param name="type"> The type to search for the method in.</param>
        /// <param name="methodName"> The name of the method to search for.</param>
        /// <param name="argTypes"> The types of the arguments of the method to search for.</param>
        /// <param name="isStatic"> Whether the method is static or not. defaults to true if the type is a static class.</param>
        /// <returns></returns>
        private static MethodInfo GetMethod(Type type, string methodName, bool isStatic, params Type[] argTypes)
        {
            var method = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance,
                null, argTypes, null);
            if (method != null)
                return method;
            method = type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance,
                null, argTypes, null);
            return method;
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
                throw;
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