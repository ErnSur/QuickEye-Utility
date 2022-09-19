using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace QuickEye.Utility.Editor
{
    internal static class PropertyDrawerUtility
    {
        private static readonly MethodInfo _GetFieldInfoFromPropertyMethodInfo;
        private static readonly MethodInfo _GetDrawerTypeForPropertyAndTypeMethodInfo;
        static PropertyDrawerUtility()
        {
            var scriptAttributeUtilityType =
                typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ScriptAttributeUtility");
            _GetFieldInfoFromPropertyMethodInfo = scriptAttributeUtilityType?.GetMethod("GetFieldInfoFromProperty",
                BindingFlags.Static | BindingFlags.NonPublic);
            _GetDrawerTypeForPropertyAndTypeMethodInfo = scriptAttributeUtilityType?.GetMethod(
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

        /// <summary>
        /// Get custom property drawer for serialized property
        /// </summary>
        /// <param name="property">Property drawer target</param>
        /// <param name="drawer">Property drawer result</param>
        /// <param name="failed">If method failed due to reflection</param>
        /// <returns></returns>
        public static bool TryGetPropertyDrawer(SerializedProperty property, out PropertyDrawer drawer, out bool failed)
        {
            try
            {
                GetFieldInfoFromProperty(property, out var type);
                var drawerType = GetDrawerTypeForPropertyAndType(property, type);
                if (drawerType == null)
                {
                    drawer = null;
                    failed = false;
                    return false;
                }

                drawer = (PropertyDrawer)Activator.CreateInstance(drawerType);
                failed = drawer == null;
                return !failed;
            }
            catch
            {
                drawer = null;
                return failed = false;
            }
        }

        private static FieldInfo GetFieldInfoFromProperty(SerializedProperty property, out Type type)
        {
            var parameters = new object[] { property, null };
            var fieldInfo = _GetFieldInfoFromPropertyMethodInfo.Invoke(null, parameters);
            type = (Type)parameters[1];
            return (FieldInfo)fieldInfo;
        }

        private static Type GetDrawerTypeForPropertyAndType(SerializedProperty property, Type type)
        {
            var result = _GetDrawerTypeForPropertyAndTypeMethodInfo.Invoke(null, new object[] { property, type });
            return (Type)result;
        }
    }
}