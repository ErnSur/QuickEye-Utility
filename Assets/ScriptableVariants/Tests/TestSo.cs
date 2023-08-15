using System;
using UnityEditor;
using UnityEngine;

namespace ScriptableVariants.Editor.Tests
{
    public class TestSo : ScriptableObject
    {
        public int prop1;
        public Vector2Int prop2;
    }
    
    public class TestWindow : EditorWindow
    {
        public Action Draw;
        public void OnGUI()
        {
            Draw?.Invoke();
        }
    }
}