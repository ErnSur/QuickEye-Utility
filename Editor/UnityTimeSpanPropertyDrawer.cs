using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace QuickEye.Utility.Editor
{
    [CustomPropertyDrawer(typeof(UnityTimeSpan))]
    [CustomPropertyDrawer(typeof(TimeOfDayAttribute))]
    public class UnityTimeSpanPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GUIControls.GetHeightForFields(GUIControls.ShouldDrawWideMode(label) ? 1 : 2);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var ticksProp = property.FindPropertyRelative("ticks");

            label = BeginProperty(position, label, property);
            BeginChangeCheck();
            var newTicks = GUIControls.TimeSpanField(position, label, ticksProp.longValue, attribute is TimeOfDayAttribute);
            if (EndChangeCheck())
                ticksProp.longValue = newTicks;
            EndProperty();
        }
    }
}