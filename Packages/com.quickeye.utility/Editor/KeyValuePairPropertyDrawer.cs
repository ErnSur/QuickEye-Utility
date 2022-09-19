using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    [CustomPropertyDrawer(typeof(UnityDictionary<,>.KvP))]
    public class KeyValuePairPropertyDrawer : PropertyDrawer
    {
        private static readonly GUIContent _InvalidKeyContent = new GUIContent(string.Empty,
            "An element with the same key already exists in the dictionary.");

        private static readonly RectOffset _IconRectOffset = new RectOffset(26, 0, -1, 0);
        private static readonly Vector2 _IconSize = new Vector2(16, 16);

        private static float LineHeightWithSpacing =>
            EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        private static void GetRelativeProps(SerializedProperty property, out SerializedProperty keyProp,
            out SerializedProperty valueProp,
            out SerializedProperty duplicateProp)
        {
            keyProp = property.FindPropertyRelative(nameof(UnityDictionary<int, int>.KvP.key));
            valueProp = property.FindPropertyRelative(nameof(UnityDictionary<int, int>.KvP.value));
            duplicateProp = property.FindPropertyRelative(nameof(UnityDictionary<int, int>.KvP.eo_duplicatedKey));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            GetRelativeProps(property, out var keyProp, out var valueProp, out var duplicateProp);
            SplitRect(position, out var keyRect, out var valueRect);

            DrawInvalidKeyIndicator(keyRect, duplicateProp.boolValue);
            DrawKeyValuePairField(keyRect, keyProp, "Key");
            DrawKeyValuePairField(valueRect, valueProp, "Value");

            EditorGUI.EndProperty();
        }

        private static void SplitRect(Rect position, out Rect keyRect, out Rect valueRect)
        {
            keyRect = position;
            valueRect = position;
            keyRect.width /= 3f;
            var valueRectPadding = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
            valueRect.xMin = keyRect.xMax + valueRectPadding;
        }

        private static void DrawKeyValuePairField(Rect position, SerializedProperty property, string label)
        {
            using (new WideModeScope(GetWideModeFor(property)))
            {
                EditorGUIUtility.labelWidth = position.width / 3;

                if (ShouldHideFoldoutProperty(property))
                {
                    PropertyDrawerUtility.DrawPropertyChildren(position, property);
                    return;
                }

                position.height = EditorGUI.GetPropertyHeight(property);
                EditorGUI.PropertyField(position, property,
                    CanHaveFoldout(property) ? new GUIContent(label) : GUIContent.none, true);
            }
        }

        private static void DrawInvalidKeyIndicator(Rect keyRect, bool isInvalid)
        {
            if (_InvalidKeyContent.image == null)
                _InvalidKeyContent.image = EditorGUIUtility.IconContent("CollabConflict Icon").image;

            var iconRect = _IconRectOffset.Add(keyRect);
            iconRect.size = _IconSize;
            using (new EditorGUIUtility.IconSizeScope(_IconSize))
            {
                var content = isInvalid ? _InvalidKeyContent : GUIContent.none;
                EditorGUI.LabelField(iconRect, content);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            GetRelativeProps(property, out var keyProp, out var valueProp, out _);
            var height = Mathf.Max(GetHeightFor(keyProp), GetHeightFor(valueProp));
            return height;
        }

        private static float GetHeightFor(SerializedProperty property)
        {
            using (new WideModeScope(GetWideModeFor(property)))
            {
                var height = EditorGUI.GetPropertyHeight(property) + GetHeightOffsetFor(property);
                if (!ShouldHideFoldoutProperty(property))
                    return height;
                property.isExpanded = true;
                return EditorGUI.GetPropertyHeight(property) + GetHeightOffsetFor(property) - LineHeightWithSpacing;
            }
        }

        /// <summary>
        /// Some properties have a strange implementations/bugs that return extra unused space from EditorGUI.GetPropertyHeight
        /// </summary>
        private static float GetHeightOffsetFor(SerializedProperty property)
        {
            return property.propertyType switch
            {
                SerializedPropertyType.Vector4 when !EditorGUIUtility.wideMode => -LineHeightWithSpacing,
                SerializedPropertyType.Rect or SerializedPropertyType.RectInt => -LineHeightWithSpacing,
                _ => 0
            };
        }

        private static bool ShouldHideFoldoutProperty(SerializedProperty property)
        {
            return !property.isArray && CanHaveFoldout(property) &&
                   !PropertyDrawerUtility.TryGetPropertyDrawer(property, out _, out var failed) && !failed;
        }

        private static bool GetWideModeFor(SerializedProperty property)
        {
            return property.propertyType is SerializedPropertyType.Bounds or SerializedPropertyType.BoundsInt
                or SerializedPropertyType.Vector4;
        }

        private static bool CanHaveFoldout(SerializedProperty prop)
        {
            return prop.propertyType is
                SerializedPropertyType.Generic or
                SerializedPropertyType.Vector4 or
                SerializedPropertyType.Bounds or
                SerializedPropertyType.BoundsInt;
        }
    }
}