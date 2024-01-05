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
        public static event Action<VisualElement> StatusBarCreated; 
        private static GUIView _statusBarInstance;
        
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update += OnUpdate;    
        }

        private static void OnUpdate()
        {
            if (_statusBarInstance != null)
                return;
            _statusBarInstance = Resources.FindObjectsOfTypeAll<AppStatusBar>().FirstOrDefault();
            if(_statusBarInstance == null)
                return; // todo: prevent FindObjectsOfTypeAll on every tick
                
            var visualTree = _statusBarInstance.windowBackend.visualTree as VisualElement;
            visualTree.styleSheets.Add(UIElementsEditorUtility.GetCommonDarkStyleSheet());

            var originalBar = visualTree.Q<IMGUIContainer>();
            originalBar.AddToClassList("status-bar");
            StatusBarCreated?.Invoke(visualTree);
        }
    }
}