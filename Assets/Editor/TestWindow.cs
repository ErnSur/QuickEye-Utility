using System;
using System.Collections;
using System.Collections.Generic;
using QuickEye.UIToolkit;
using QuickEye.Utility.Editor;
using UnityEditor;
using UnityEngine;

public class TestWindow : EditorWindow
{
    [MenuItem("Test/Test Window")]
    public static void Open() => GetWindow<TestWindow>();

    private void CreateGUI()
    {
        var group = new TabGroup();
        rootVisualElement.Add(new LayoutToolbar());
        group.Add(new Tab("hello"));
        group.Add(new Tab("hello2"));
    }
}
