using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    public class PropertyWrapperScope : IDisposable
    {
        private readonly Action<Rect, SerializedProperty> _begin, _end;
        private readonly EditorGUIUtility.PropertyCallbackScope _propertyCallbackScope;
        private SerializedProperty _lastProperty;
        private Rect _lastRect;
        
        public PropertyWrapperScope(Action<Rect, SerializedProperty> begin, Action<Rect, SerializedProperty> end)
        {
            _begin = begin;
            _end = end;
            _propertyCallbackScope = new EditorGUIUtility.PropertyCallbackScope(StartPropertyRendering);
        }

        private void StartPropertyRendering(Rect rect, SerializedProperty property)
        {
            EndScope();
            _lastProperty = property.Copy();
            _lastRect = rect;
            _begin?.Invoke(rect, property.Copy());
        }

        private void EndScope()
        {
            if (_lastProperty?.serializedObject == null)
                return;

            _end?.Invoke(_lastRect, _lastProperty.Copy());
        }

        public void Dispose()
        {
            EndScope();
            _propertyCallbackScope?.Dispose();
            _lastProperty?.Dispose();
        }
    }
}