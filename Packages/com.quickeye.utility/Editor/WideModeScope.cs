using System;
using UnityEditor;

namespace QuickEye.Utility.Editor
{
    internal readonly struct WideModeScope : IDisposable
    {
        private readonly bool _previousValue;

        public WideModeScope(bool value)
        {
            _previousValue = EditorGUIUtility.wideMode;
            EditorGUIUtility.wideMode = value;
        }

        public void Dispose()
        {
            EditorGUIUtility.wideMode = _previousValue;
        }
    }
}