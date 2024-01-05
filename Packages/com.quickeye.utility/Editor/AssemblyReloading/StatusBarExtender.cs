using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor.AssemblyReloading
{
    internal static class StatusBarExtender
    {
        private const int MaxRetries = 5;
        public static event Action<VisualElement> StatusBarCreated; 
        private static GUIView _statusBarInstance;
        private static int _retries;
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update += OnUpdate;    
        }

        private static void OnUpdate()
        {
            if (_statusBarInstance != null || _retries >= MaxRetries)
                return;
            _statusBarInstance = Resources.FindObjectsOfTypeAll<AppStatusBar>().FirstOrDefault();
            if(_statusBarInstance == null)
            {
                _retries++;
                return;
            }

            try
            {
                var visualTree = _statusBarInstance.windowBackend.visualTree as VisualElement;
                visualTree.styleSheets.Add(UIElementsEditorUtility.GetCommonDarkStyleSheet());

                var originalBar = visualTree.Q<IMGUIContainer>();
                originalBar.AddToClassList("status-bar");
                StatusBarCreated?.Invoke(visualTree);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to extend status bar: {e}");
                _retries = MaxRetries;
            }
        }
    }
}