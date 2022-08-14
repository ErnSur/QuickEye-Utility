using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.Toolbars;
using UnityEngine;

namespace QuickEye.Utility.Editor.WindowTitle
{
    [EditorToolbarElement(id, typeof(DefaultMainToolbar))]
    class LayoutToolbarElement : LayoutToolbar
    {
        public const string id = "QuickEyeToolbar/LayoutTabs";
    }
    
    [EditorToolbarElement(id, typeof(DefaultMainToolbar))]
    class DropDownElement : EditorToolbarDropdown
    {
        public const string id = "QuickEyeToolbar/LayoutTabs2";
        
        
        static string dropChoice = null;

        public DropDownElement()
        {
            text = "Axis";
            clicked += ShowDropdown;
        }

        void ShowDropdown()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("X"), dropChoice == "X", () => { text = "X"; dropChoice = "X"; });
            menu.AddItem(new GUIContent("Y"), dropChoice == "Y", () => { text = "Y"; dropChoice = "Y"; });
            menu.AddItem(new GUIContent("Z"), dropChoice == "Z", () => { text = "Z"; dropChoice = "Z"; });
            menu.ShowAsContext();
        }
    }
    
    [EditorToolbarElement(id, typeof(DefaultMainToolbar))]
    class DropDownToggleElement : EditorToolbarDropdownToggle
    {
        public const string id = "QuickEyeToolbar/LayoutTabs3";
        static string dropChoice = null;

        public DropDownToggleElement()
        {
            text = "Axis";
            dropdownClicked += ShowDropdown;
        }

        void ShowDropdown()
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("X"), dropChoice == "X", () => { text = "X"; dropChoice = "X"; });
            menu.AddItem(new GUIContent("Y"), dropChoice == "Y", () => { text = "Y"; dropChoice = "Y"; });
            menu.AddItem(new GUIContent("Z"), dropChoice == "Z", () => { text = "Z"; dropChoice = "Z"; });
            menu.ShowAsContext();
        }
    }
}