using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;

namespace QuickEye.Utility.Editor
{
    [CustomPropertyDrawer(typeof(UnityDateOnly))]
    public class UnityDateOnlyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return GUIControls.GetHeightForFields(GUIControls.ShouldDrawWideMode(label) ? 1 : 2);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var dayProp = property.FindPropertyRelative("dayNumber");
            var date = UnityDateOnly.FromDayNumber(dayProp.intValue);

            label = BeginProperty(position, label, property);
            BeginChangeCheck();
            var newDate = GUIControls.DateOnlyField(position, label, date);
            if (EndChangeCheck())
                dayProp.intValue = newDate.DayNumber;
            EndProperty();
        }
    }
}