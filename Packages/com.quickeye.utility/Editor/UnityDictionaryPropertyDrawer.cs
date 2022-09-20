using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>))]
    public class UnityDictionaryPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var prop = property.FindPropertyRelative("list");
            EditorGUI.PropertyField(position, prop, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var prop = property.FindPropertyRelative("list");
            return EditorGUI.GetPropertyHeight(prop);
        }
    }
}