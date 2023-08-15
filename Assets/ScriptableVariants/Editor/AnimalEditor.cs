using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace QuickEye.ScriptableObjectVariants
{
    [CustomEditor(typeof(Person))]
    public class PersonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Person.age)));
            //base.OnInspectorGUI();
        }
        // public override VisualElement CreateInspectorGUI()
        // {
        //     var root = new VisualElement();
        //     root.Add(new PropertyField() { bindingPath = nameof(Person.age) });
        //     return root;
        // }
    }

    [CustomEditor(typeof(Animal))]
    public class AnimalEditor : Editor
    {
        private void OnEnable()
        {
            var colorProp = serializedObject.FindProperty(nameof(Animal.color));
            var tempTarget =
                new SerializedObject(Instantiate(serializedObject.targetObject)).FindProperty(colorProp.propertyPath);
            var copy = tempTarget;
            Debug.Log($"one {colorProp.stringValue} and {copy.stringValue}");
            colorProp.stringValue = "newVal";
            Debug.Log($"two {colorProp.stringValue} and {copy.stringValue}");
            colorProp.serializedObject.ApplyModifiedProperties();
            Debug.Log($"three {colorProp.stringValue} and {copy.stringValue}");
        }

        public override void OnInspectorGUI()
        {
            using (new PropertyChangeCheckScope(OnPropRend))
            {
                //EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Animal.color)));
                //EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(Animal.family)));
                DoDrawDefaultInspector(serializedObject);
                //base.OnInspectorGUI();
            }
        }

        // only fails if inside editor there is a change check
        internal static bool DoDrawDefaultInspector(SerializedObject obj)
        {
            Debug.Log($"Start 1");
            EditorGUI.BeginChangeCheck();
            obj.UpdateIfRequiredOrScript();
            SerializedProperty iterator = obj.GetIterator();
            for (bool enterChildren = true; iterator.NextVisible(enterChildren); enterChildren = false)
            {
                using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                    EditorGUILayout.PropertyField(iterator, true);
            }

            obj.ApplyModifiedProperties();
            Debug.Log($"End 1");
            // TODO: nie ma opcji jakoś, muszę tutaj wywołać _end.Invoke() i zamknąć ostatni change scope
            return EditorGUI.EndChangeCheck();
        }

        private void OnPropRend(SerializedProperty obj)
        {
            //Debug.Log($"Hejo prop {obj.displayName}");
        }
    }
}