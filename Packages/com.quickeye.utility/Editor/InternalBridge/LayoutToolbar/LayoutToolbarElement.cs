using UnityEditor;
using UnityEditor.Toolbars;

namespace QuickEye.Utility.Editor.WindowTitle
{
    [EditorToolbarElement(id, typeof(DefaultMainToolbar))]
    internal class LayoutToolbarElement : LayoutToolbar
    {
        public const string id = "QuickEyeToolbar/LayoutTabs";
    }
}