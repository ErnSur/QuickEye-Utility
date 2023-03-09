using System.Collections.Generic;
using System.Linq;
using QuickEye.Utility;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace QuickEye.EventSystem.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(GameEventBase), true)]
    public class GameEventEditor : UnityEditor.Editor
    {
        SerializedProperty descriptionProp;
        SerializedProperty wasInvokedProp;
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
            wasInvokedProp = serializedObject.FindProperty("wasInvoked");
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
            var wasInvokedField = NewPropertyField(wasInvokedProp);
            var lastPayloadField = NewPropertyField(lastPayloadProp);
            var descriptionField = NewPropertyField(descriptionProp);
            var invokeButton = new Button(OnInvoke) { text = "Invoke" };

            wasInvokedField.SetEnabled(false);
            descriptionField.SetEnabled(targets.All(EditorUtility.IsPersistent));
            invokeButton.SetEnabled(EditorApplication.isPlaying);

            root.Add(scriptField);
            root.Add(wasInvokedField);
            //root.Add(eventField);
            root.Add(lastPayloadField);
            root.Add(invokeButton);
            //root.Add(descriptionField);
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