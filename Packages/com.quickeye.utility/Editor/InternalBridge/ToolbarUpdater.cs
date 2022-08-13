using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditor.Toolbars;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor.WindowTitle
{
    internal static class ToolbarUpdater
    {
        [InitializeOnLoadMethod]
        private static void UpdateToolbar()
        {
            EditorUIService.instance = new CustomEditorUIService(EditorUIService.instance);
        }
    }

    internal class CustomEditorUIService : IEditorUIService
    {
        private readonly IEditorUIService _original;
        public CustomEditorUIService(IEditorUIService original) => _original = original;
        
        public Type GetDefaultToolbarType() => typeof(CustomMainToolbar);

        
        public IWindowBackend GetDefaultWindowBackend(IWindowModel model) => _original.GetDefaultWindowBackend(model);
        public void AddSubToolbar(SubToolbar subToolbar) => _original.AddSubToolbar(subToolbar);
        public IEditorElement CreateEditorElement(int editorIndex, IPropertyView iw, string title) =>
            _original.CreateEditorElement(editorIndex, iw, title);
        public IEditorElement CreateCulledEditorElement(int editorIndex, IPropertyView iw, string title) =>
            _original.CreateCulledEditorElement(editorIndex, iw, title);
        public void PackageManagerOpen() => _original.PackageManagerOpen();
        public IShortcutManagerWindowView CreateShortcutManagerWindowView(
            IShortcutManagerWindowViewController viewController, IKeyBindingStateProvider bindingStateProvider) =>
            _original.CreateShortcutManagerWindowView(viewController, bindingStateProvider);
        public void ProgressWindowShowDetails(bool shouldReposition) => _original.ProgressWindowShowDetails(shouldReposition);
        public void ProgressWindowHideDetails() => _original.ProgressWindowHideDetails();
        public bool ProgressWindowCanHideDetails() => _original.ProgressWindowCanHideDetails();
        public void AddDefaultEditorStyleSheets(VisualElement ve) => _original.AddDefaultEditorStyleSheets(ve);
        public string GetUIToolkitDefaultCommonDarkStyleSheetPath() => _original.GetUIToolkitDefaultCommonDarkStyleSheetPath();
        public string GetUIToolkitDefaultCommonLightStyleSheetPath() => _original.GetUIToolkitDefaultCommonLightStyleSheetPath();
        public StyleSheet GetUIToolkitDefaultCommonDarkStyleSheet() => _original.GetUIToolkitDefaultCommonDarkStyleSheet();
        public StyleSheet GetUIToolkitDefaultCommonLightStyleSheet() => _original.GetUIToolkitDefaultCommonLightStyleSheet();
        public StyleSheet CompileStyleSheetContent(string styleSheetContent, bool disableValidation, bool reportErrors) =>
            _original.CompileStyleSheetContent(styleSheetContent, disableValidation, reportErrors);
    }
    
    class CustomMainToolbar : DefaultMainToolbar
    {
        protected override VisualElement CreateRoot()
        {
            var rootVe = base.CreateRoot();
            var leftContainer = rootVe.Q("ToolbarZoneLeftAlign");
            var leftToolbar = new EditorToolbar(null, leftContainer,
                LayoutToolbarElement.id);
            return rootVe;
        }
    }
}