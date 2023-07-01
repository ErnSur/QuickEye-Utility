using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using QuickEye.Utility;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.EventSystem.Editor
{
    public class EventViewer : EditorWindow
    {
        [MenuItem("Quick Eye/Event Viewer")]
        static void Open() => GetWindow<EventViewer>("Event Viewer");

        [SerializeField]
        List<Object> events;

        SerializedProperty eventsProp;

        void OnEnable()
        {
            events = TypeCache.GetTypesDerivedFrom(typeof(SingletonEvent<>))
                .Concat(TypeCache.GetTypesDerivedFrom(typeof(SingletonEvent<,>)))
                //.Where(t => typeof(IEvent).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .Select(t =>
                {
                    var instanceProp = GetInstanceProp(t);
                    return instanceProp.GetValue(null);
                })
                .Cast<Object>()
                .ToList();
            eventsProp = new SerializedObject(this).FindProperty(nameof(events));
        }

        static PropertyInfo GetInstanceProp(Type t)
        {
            if (t == null)
                return null;
            var genericBaseType = t.BaseType.GetGenericTypeDefinition();
            if (genericBaseType != typeof(SingletonEvent<,>) && genericBaseType != typeof(SingletonEvent<>))
                return GetInstanceProp(t.BaseType);
            var instanceProp = t.BaseType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
            return instanceProp;
        }

        void OnGUI()
        {
            EditorGUILayout.PropertyField(eventsProp);
        }
    }
}