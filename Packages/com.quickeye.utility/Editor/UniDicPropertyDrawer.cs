using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    [CustomPropertyDrawer(typeof(UnityDic<,>))]
    public class UniDicPropertyDrawer : PropertyDrawer
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

    [CustomPropertyDrawer(typeof(UnityDic<,>.KvP))]
    public class KeyValuePairPropertyDrawer : PropertyDrawer
    {
        private static (bool wideMode, float labelWidth) _previousValues;

        private static bool HasFoldout(SerializedProperty prop)
        {
            return prop.propertyType == SerializedPropertyType.Generic;
        }

        private void SplitRects(Rect position, bool hasFoldout, out Rect keyRect, out Rect valueRect)
        {
            position.height -= 2;
            keyRect = position;
            valueRect = position;

            keyRect.width /= 3f;
            var padding =
#if UNITY_2022_2_OR_NEWER
                2;
#else
                hasFoldout ? 22 : 2;
#endif
            valueRect.xMin = keyRect.xMax + padding;
        }

        private static void GetRelativeProps(SerializedProperty property, out SerializedProperty keyProp,
            out SerializedProperty valueProp,
            out SerializedProperty duplicateProp)
        {
            keyProp = property.FindPropertyRelative(nameof(UnityDic<int, int>.KvP.Key));
            valueProp = property.FindPropertyRelative(nameof(UnityDic<int, int>.KvP.Value));
            duplicateProp = property.FindPropertyRelative(nameof(UnityDic<int, int>.KvP.duplicatedKey));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGlobalValues();
            EditorGUIUtility.wideMode = false;

            EditorGUI.BeginProperty(position, label, property);
            GetRelativeProps(property, out var keyProp, out var valueProp, out var duplicateProp);
            SplitRects(position, HasFoldout(valueProp), out var keyRect, out var valueRect);

            if (duplicateProp.boolValue)
                DrawInvalidKeyIndicator(keyRect);

            var isExpanded = keyProp.isExpanded || valueProp.isExpanded;
            keyProp.isExpanded = valueProp.isExpanded = isExpanded;

            DrawKeyProp(keyProp, keyRect);
            DrawValueProp(valueProp, valueRect);

            if (keyProp.isExpanded != valueProp.isExpanded)
                keyProp.isExpanded = valueProp.isExpanded = !isExpanded;

            EditorGUI.EndProperty();
            RestoreGlobalValues();
        }

        private void CacheGlobalValues()
        {
            _previousValues = (EditorGUIUtility.wideMode, EditorGUIUtility.labelWidth);
        }

        private void RestoreGlobalValues()
        {
            (EditorGUIUtility.wideMode, EditorGUIUtility.labelWidth) = _previousValues;
        }

        private void DrawValueProp(SerializedProperty prop, Rect valueRect)
        {
            EditorGUIUtility.labelWidth = valueRect.width / 3;
            EditorGUI.PropertyField(valueRect, prop,
                HasFoldout(prop) ? new GUIContent("Value") : GUIContent.none, true);
        }

        private void DrawKeyProp(SerializedProperty prop, Rect keyRect)
        {
            EditorGUIUtility.labelWidth = keyRect.width / 3;
            EditorGUI.PropertyField(keyRect, prop, HasFoldout(prop) ? new GUIContent("Key") : GUIContent.none,
                true);
        }

        private static void DrawInvalidKeyIndicator(Rect keyRect)
        {
            var sideLineRect = keyRect;
            sideLineRect.width = 2;
            sideLineRect.height = EditorGUIUtility.singleLineHeight;
            sideLineRect.x -= 2;
            var color = Color.red;
            color.a = .7f;
            EditorGUI.DrawRect(sideLineRect, color);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheGlobalValues();
            EditorGUIUtility.wideMode = false;
            GetRelativeProps(property, out var keyProp, out var valueProp, out _);

            var height = Mathf.Max(EditorGUI.GetPropertyHeight(keyProp), EditorGUI.GetPropertyHeight(valueProp));
            RestoreGlobalValues();
            return height;
        }
    }
}