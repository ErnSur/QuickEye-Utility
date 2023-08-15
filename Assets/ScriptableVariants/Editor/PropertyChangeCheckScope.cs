using System;
using UnityEditor;
using UnityEngine;

namespace QuickEye.ScriptableObjectVariants
{
    // lets assume that after modyfying any prop at the end of onInspectorGUI GYU.changed is trigered again
    public class PropertyChangeCheckScope : IDisposable
    {
        private readonly Action<SerializedProperty> _onPropertyChange;
        private EditorGUIUtility.PropertyCallbackScope _propertyCallbackScope;
        private SerializedProperty _lastProperty;

        public PropertyChangeCheckScope(Action<SerializedProperty> onPropertyChange)
        {
            _onPropertyChange = onPropertyChange;
            _propertyCallbackScope = new EditorGUIUtility.PropertyCallbackScope(StartPropertyRendering);
        }

        private void StartPropertyRendering(Rect _, SerializedProperty property)
        {
            if (property.propertyPath == "color")
                GUI.enabled = false;
            EndChangeCheck();
           // Debug.Log($"Start CUSTOM {property.displayName}");
            _lastProperty = property.Copy();
        }

        private void EndChangeCheck()
        {
            //Debug.Log($"Try End CUSTOM ");
            if (_lastProperty == null)
                return;
           // Debug.Log($"End CUSTOM {_lastProperty.displayName}");

            if (CustomChangeCheck.EndChangeCheck())
            {
                //Debug.Log($"Modified: {_lastProperty.propertyPath}");
                _onPropertyChange?.Invoke(_lastProperty.Copy());
            }
        }

        public void Dispose()
        {
            EndChangeCheck();
            _propertyCallbackScope?.Dispose();
            _lastProperty?.Dispose();
        }
    }
}