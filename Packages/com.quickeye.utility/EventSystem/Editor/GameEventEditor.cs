using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuickEye.Utility.Editor
{
    using UnityEditor;

    [CustomEditor(typeof(GameEventBase), true)]
    public class GameEventEditor : Editor
    {
        SerializedProperty descriptionProp;
        SerializedProperty eventProp;
        SerializedProperty lastPayloadProp;

        /// <summary>
        /// null means: Mixed Values
        /// </summary>
        bool? WasInvoked => Targets.Any(v => v != Target.WasInvoked) ? null : Target.WasInvoked;

        GameEventBase Target => (GameEventBase)target;
        IEnumerable<GameEventBase> Targets => targets.Cast<GameEventBase>();

        void OnEnable()
        {
            descriptionProp = serializedObject.FindProperty(nameof(DummyEvent.developerDescription));
            eventProp = serializedObject.FindProperty("_event");
            lastPayloadProp = serializedObject.FindProperty("_lastPayload");
        }

        PropertyField NewPropertyField(SerializedProperty prop)
        {
            if (prop == null)
                return null;
            var field = new PropertyField(prop);
            field.name = "PropertyField:" + prop.propertyPath;
            return field;
        }

        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            //AddPropertyField(root, wasInvokedProp);
            var scriptField = NewPropertyField(serializedObject.FindProperty("m_Script"));
            scriptField.SetEnabled(false);
            var eventField = NewPropertyField(eventProp);
            var lastPayloadField = NewPropertyField(lastPayloadProp);
            var descriptionField = NewPropertyField(descriptionProp);
            descriptionField.SetEnabled(targets.All(EditorUtility.IsPersistent));

            root.Add(scriptField);
            root.Add(eventField);
            root.Add(lastPayloadField);
            root.Add(new Button(OnInvoke) { text = "Invoke" });
            root.Add(descriptionField);
            return root;
        }

        void OnInvoke()
        {
            foreach (IInvokable invokable in targets)
            {
                invokable?.RepeatLastInvoke();
            }
        }

        class DummyEvent : GameEvent<int> { }
    }
}