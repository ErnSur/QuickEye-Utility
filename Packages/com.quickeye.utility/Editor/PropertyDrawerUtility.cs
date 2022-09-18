using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    internal static class PropertyDrawerUtility
    {
        private static readonly MethodInfo _getFieldInfoFromPropertyMethodInfo;
        private static readonly MethodInfo _getDrawerTypeForPropertyAndTypeMethodInfo;

        static PropertyDrawerUtility()
        {
            var scriptAttributeUtilityType =
                typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ScriptAttributeUtility");
            _getFieldInfoFromPropertyMethodInfo = scriptAttributeUtilityType.GetMethod("GetFieldInfoFromProperty",
                BindingFlags.Static | BindingFlags.NonPublic);
            _getDrawerTypeForPropertyAndTypeMethodInfo = scriptAttributeUtilityType.GetMethod(
                "GetDrawerTypeForPropertyAndType",
                BindingFlags.Static | BindingFlags.NonPublic);
        }

        public static void DrawPropertyChildren(Rect position, SerializedProperty prop)
        {
            var endProperty = prop.GetEndProperty();
            var childrenDepth = prop.depth + 1;
            while (prop.NextVisible(true) && !SerializedProperty.EqualContents(prop, endProperty))
            {
                if (prop.depth != childrenDepth)
                    continue;
                position.height = EditorGUI.GetPropertyHeight(prop);
                EditorGUI.PropertyField(position, prop, true);
                position.y += position.height + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        public static bool TryGetPropertyDrawer(SerializedProperty property, out PropertyDrawer drawer)
        {
            GetFieldInfoFromProperty(property, out var type);
            var drawerType = GetDrawerTypeForPropertyAndType(property, type);
            if (drawerType == null)
            {
                drawer = null;
                return false;
            }

            drawer = (PropertyDrawer)Activator.CreateInstance(drawerType);
            return drawer != null;
        }

        private static FieldInfo GetFieldInfoFromProperty(SerializedProperty property, out Type type)
        {
            var parameters = new object[] { property, null };
            var fieldInfo = _getFieldInfoFromPropertyMethodInfo.Invoke(null, parameters);
            type = (Type)parameters[1];
            return (FieldInfo)fieldInfo;
        }

        private static Type GetDrawerTypeForPropertyAndType(SerializedProperty property, Type type)
        {
            var result = _getDrawerTypeForPropertyAndTypeMethodInfo.Invoke(null, new object[] { property, type });
            return (Type)result;
        }
    }
}