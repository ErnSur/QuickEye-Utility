using System;
using UnityEditor;
using UnityEditor.ShortcutManagement;
using UnityEditor.Toolbars;
using UnityEditor.UIElements;
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

        public CustomEditorUIService(IEditorUIService original)
        {
            _original = original;
        }

        public Type GetDefaultToolbarType()
        {
            return typeof(CustomMainToolbar);
        }


        public IWindowBackend GetDefaultWindowBackend(IWindowModel model)
        {
            return _original.GetDefaultWindowBackend(model);
        }

        public void AddSubToolbar(SubToolbar subToolbar)
        {
            _original.AddSubToolbar(subToolbar);
        }

        public IEditorElement CreateEditorElement(int editorIndex, IPropertyView iw, string title)
        {
            return _original.CreateEditorElement(editorIndex, iw, title);
        }

        public IEditorElement CreateCulledEditorElement(int editorIndex, IPropertyView iw, string title)
        {
            return _original.CreateCulledEditorElement(editorIndex, iw, title);
        }

        public void PackageManagerOpen()
        {
            _original.PackageManagerOpen();
        }

        public IShortcutManagerWindowView CreateShortcutManagerWindowView(
            IShortcutManagerWindowViewController viewController, IKeyBindingStateProvider bindingStateProvider)
        {
            return _original.CreateShortcutManagerWindowView(viewController, bindingStateProvider);
        }

        public void ProgressWindowShowDetails(bool shouldReposition)
        {
            _original.ProgressWindowShowDetails(shouldReposition);
        }

        public void ProgressWindowHideDetails()
        {
            _original.ProgressWindowHideDetails();
        }

        public bool ProgressWindowCanHideDetails()
        {
            return _original.ProgressWindowCanHideDetails();
        }

        public void AddDefaultEditorStyleSheets(VisualElement ve)
        {
            _original.AddDefaultEditorStyleSheets(ve);
        }

        public string GetUIToolkitDefaultCommonDarkStyleSheetPath()
        {
            return _original.GetUIToolkitDefaultCommonDarkStyleSheetPath();
        }

        public string GetUIToolkitDefaultCommonLightStyleSheetPath()
        {
            return _original.GetUIToolkitDefaultCommonLightStyleSheetPath();
        }

        public StyleSheet GetUIToolkitDefaultCommonDarkStyleSheet()
        {
            return _original.GetUIToolkitDefaultCommonDarkStyleSheet();
        }

        public StyleSheet GetUIToolkitDefaultCommonLightStyleSheet()
        {
            return _original.GetUIToolkitDefaultCommonLightStyleSheet();
        }

        public StyleSheet CompileStyleSheetContent(string styleSheetContent, bool disableValidation, bool reportErrors)
        {
            return _original.CompileStyleSheetContent(styleSheetContent, disableValidation, reportErrors);
        }
    }

    internal class CustomMainToolbar : DefaultMainToolbar
    {
        // TODO: find all types with EditorToolbarElement attribute that target "ToolbarType" and add them here
        protected override VisualElement CreateRoot()
        {
            var rootVe = base.CreateRoot();
            var leftContainer = rootVe.Q("ToolbarZoneLeftAlign");
            UIElementsEditorUtility.AddDefaultEditorStyleSheets(rootVe);
            var leftToolbar = new EditorToolbar(null, leftContainer,
                LayoutToolbarElement.id
            );
            return rootVe;
        }
    }
}