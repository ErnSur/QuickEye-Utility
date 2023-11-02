using System;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUIUtility;
using static QuickEye.Utility.Editor.GUIControls;

namespace QuickEye.Utility.Editor
{
    internal class MultiFieldScope : IDisposable
    {
        private readonly Rect _position;
        private readonly float _oldLabelWidth;
        private readonly int _oldIndentLevel;
        private readonly float _fieldWidth;
        private static float MinSingleLetterLabelWidth => CalcPrefixLabelWidth(new GUIContent("W"));
        public MultiFieldScope(Rect position, int fieldCount)
        {
            _position = position;
            _oldLabelWidth = labelWidth;
            _oldIndentLevel = indentLevel;
            _fieldWidth = (position.width - (fieldCount - 1) * MultiFieldSpacing) / fieldCount;
            indentLevel = 0;
        }

        public void SetLabelWidth(float width)
        {
            if (width < MinSingleLetterLabelWidth)
                width = MinSingleLetterLabelWidth;
            labelWidth = width;
        }

        public void SetLabelWidth(GUIContent label, GUIStyle style = null)
        {
            SetLabelWidth(CalcPrefixLabelWidth(label, style));
        }

        public Rect GetRect(int index)
        {
            var r = new Rect(_position) { width = _fieldWidth };
            r.x += (_fieldWidth + MultiFieldSpacing) * index;

            return r;
        }

        private static float CalcPrefixLabelWidth(GUIContent label, GUIStyle style = null)
        {
            if (style == null)
                style = EditorStyles.label;
            return style.CalcSize(label).x;
        }

        public void Dispose()
        {
            labelWidth = _oldLabelWidth;
            indentLevel = _oldIndentLevel;
        }
    }
}