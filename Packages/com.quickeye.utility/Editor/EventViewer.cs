using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace QuickEye.Utility.Editor
{
    public class EventViewer : EditorWindow
    {
        [MenuItem("QuickEYe/Events")]
        static void Open() => GetWindow<EventViewer>();

        [SerializeField]
        List<Object> events;

        SerializedProperty eventsProp;
        void OnEnable()
        {
            events = TypeCache.GetTypesDerivedFrom<ISingletonScriptableObject>()
                .Where(t => typeof(IEvent).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .Select(t=>
                {
                    Debug.Log($"TypeName {t.FullName}");
                    var instanceProp= t.BaseType.BaseType.GetProperty("Instance", BindingFlags.Static | BindingFlags.Public);
                    return instanceProp.GetValue(null);
                })
                .Cast<Object>()
                .ToList();
            eventsProp = new SerializedObject(this).FindProperty(nameof(events));
        }

        void OnGUI()
        {
            EditorGUILayout.PropertyField(eventsProp);
        }
    }
}