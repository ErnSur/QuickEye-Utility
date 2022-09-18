using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    // TODO:
    // docs
    // draw default foldout prop drawer if reflection fails
    // try hook into reorderable array?
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
        private static readonly RectOffset _IconRectOffset = new RectOffset(26, 0, -1, 0);
        private static readonly Vector2 _IconSize = new Vector2(16, 16);
        private static (bool wideMode, float labelWidth) _previousValues;

        private static void GetRelativeProps(SerializedProperty property, out SerializedProperty keyProp,
            out SerializedProperty valueProp,
            out SerializedProperty duplicateProp)
        {
            keyProp = property.FindPropertyRelative(nameof(UnityDic<int, int>.KvP.Key));
            valueProp = property.FindPropertyRelative(nameof(UnityDic<int, int>.KvP.Value));
            duplicateProp = property.FindPropertyRelative(nameof(UnityDic<int, int>.KvP.eo_duplicatedKey));
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            CacheGlobalValues();
            EditorGUIUtility.wideMode = false;

            EditorGUI.BeginProperty(position, label, property);
            GetRelativeProps(property, out var keyProp, out var valueProp, out var duplicateProp);
            SplitRects(position, out var keyRect, out var valueRect);

            if (duplicateProp.boolValue)
                DrawInvalidKeyIndicator(keyRect);
            using (var change = new EditorGUI.ChangeCheckScope())
            {
                DrawKeyProp(keyRect, keyProp);
                if (!change.changed)
                    DrawValueProp(valueRect, valueProp);
            }


            EditorGUI.EndProperty();
            RestoreGlobalValues();
        }

        private static bool HasFoldout(SerializedProperty prop)
        {
            return prop.propertyType == SerializedPropertyType.Generic;
        }

        private static void SplitRects(Rect position, out Rect keyRect, out Rect valueRect)
        {
            position.height -= 2;
            keyRect = position;
            valueRect = position;

            keyRect.width /= 3f;
            var padding = 2;
            valueRect.xMin = keyRect.xMax + padding;
            //if (EditorGUIUtility.hierarchyMode)
            {
                int num = EditorStyles.foldout.padding.left - EditorStyles.label.padding.left;
                valueRect.xMin += num;
            }
        }

        private static void CacheGlobalValues()
        {
            _previousValues = (EditorGUIUtility.wideMode, EditorGUIUtility.labelWidth);
        }

        private static void RestoreGlobalValues()
        {
            (EditorGUIUtility.wideMode, EditorGUIUtility.labelWidth) = _previousValues;
        }

        private static void DrawValueProp(Rect position, SerializedProperty property)
        {
            EditorGUIUtility.labelWidth = position.width / 3;
            if (IsNonArrayFoldoutProperty(property))
            {
                PropertyDrawerUtility.DrawPropertyChildren(position, property);
            }
            else
            {
                EditorGUI.PropertyField(position, property,
                    HasFoldout(property) ? new GUIContent("Value") : GUIContent.none, true);
            }
        }

        private static void DrawKeyProp(Rect position, SerializedProperty property)
        {
            if (property.isArray)
                EditorGUIUtility.hierarchyMode = false;

            EditorGUIUtility.labelWidth = position.width / 3;

            if (IsNonArrayFoldoutProperty(property))
            {
                PropertyDrawerUtility.DrawPropertyChildren(position, property);
            }
            else
            {
                EditorGUI.PropertyField(position, property,
                    HasFoldout(property) ? new GUIContent("Key") : GUIContent.none,
                    true);
            }

            EditorGUIUtility.hierarchyMode = true;
        }


        private static void DrawInvalidKeyIndicator(Rect keyRect)
        {
            var iconRect = _IconRectOffset.Add(keyRect);
            iconRect.size = _IconSize;
            using (new EditorGUIUtility.IconSizeScope(_IconSize))
            {
                var content = EditorGUIUtility.IconContent("CollabConflict Icon");
                content.tooltip = "Duplicate key";
                EditorGUI.LabelField(iconRect, content);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            CacheGlobalValues();
            EditorGUIUtility.wideMode = false;
            GetRelativeProps(property, out var keyProp, out var valueProp, out _);

            var height = Mathf.Max(GetPropHeight(keyProp), GetPropHeight(valueProp));

            RestoreGlobalValues();
            return height;
        }

        private static float GetPropHeight(SerializedProperty property)
        {
            var height = EditorGUI.GetPropertyHeight(property);
            if (IsNonArrayFoldoutProperty(property))
            {
                property.isExpanded = true;
                height = EditorGUI.GetPropertyHeight(property);
                return height - EditorGUIUtility.singleLineHeight;
            }

            return height;
        }

        private static bool IsNonArrayFoldoutProperty(SerializedProperty property)
        {
            return !property.isArray && property.propertyType == SerializedPropertyType.Generic &&
                   !PropertyDrawerUtility.TryGetPropertyDrawer(property, out _);
        }
    }
}