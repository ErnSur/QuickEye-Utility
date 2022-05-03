using System;
using UnityEngine;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUIUtility;

namespace QuickEye.Utility.Editor
{
    public static class GUIControls
    {
        public const float IndentPerLevel = 15;
        public const float MultiFieldSpacing = 4;
        private static float Indent => indentLevel * IndentPerLevel;

        private static readonly int[] DateOnlyValues = { 0, 0, 0 };

        private static readonly GUIContent[] DateTimeLabels =
        {
            new GUIContent("D", "Day"),
            new GUIContent("M", "Month"),
            new GUIContent("Y", "Year")
        };

        public static UnityTimeSpan TimeSpanField(Rect position, GUIContent label, UnityTimeSpan unityTimeSpan,
            bool isTimeOfDay = false)
        {
            return new UnityTimeSpan(TimeSpanField(position, label, unityTimeSpan.Ticks, isTimeOfDay));
        }

        public static long TimeSpanField(Rect position, GUIContent label, long ticks, bool isTimeOfDay = false)
        {
            var controlRect = MultiFieldPrefixLabel(position, label, 3);

            var timeSpan = new TimeSpan(ticks);
            var hours = timeSpan.Hours + timeSpan.Days * 24;
            var minutes = timeSpan.Minutes;
            var seconds = timeSpan.Seconds + timeSpan.Milliseconds / 1000d;
            using (var multiField = new MultiFieldScope(controlRect, 3))
            {
                var content = new GUIContent("H");
                multiField.SetLabelWidth(content);
                hours = IntField(multiField.GetRect(0), content, hours);

                content.text = "M";
                multiField.SetLabelWidth(content);
                minutes = IntField(multiField.GetRect(1), content, minutes);

                content.text = "S";
                multiField.SetLabelWidth(content);
                seconds = DoubleField(multiField.GetRect(2), content, seconds);
            }

            var newTicks = ticks;
            try
            {
                newTicks = (TimeSpan.FromHours(hours) + TimeSpan.FromMinutes(minutes) + TimeSpan.FromSeconds(seconds))
                    .Ticks;
            }
            catch (OverflowException)
            {
            }

            if (isTimeOfDay)
            {
                // MaxTimeTicks is the max tick value for the time in the day. It is calculated using DateTime.Today.AddTicks(-1).TimeOfDay.Ticks.
                const long maxTimeOfDayTicks = 863_999_999_999;
                newTicks = MathUtility.Clamp(newTicks, 0, maxTimeOfDayTicks);
            }

            return newTicks;
        }

        public static UnityDateOnly DateOnlyField(Rect position, GUIContent label, UnityDateOnly date)
        {
            var controlRect = MultiFieldPrefixLabel(position, label, 3);

            DateOnlyValues[0] = date.Day;
            DateOnlyValues[1] = date.Month;
            DateOnlyValues[2] = date.Year;

            MultiIntField(controlRect, DateTimeLabels, DateOnlyValues);

            DateOnlyValues[2] = Mathf.Clamp(DateOnlyValues[2], DateTime.MinValue.Year, DateTime.MaxValue.Year);
            DateOnlyValues[1] = Mathf.Clamp(DateOnlyValues[1], 1, 12);
            DateOnlyValues[0] = Mathf.Clamp(DateOnlyValues[0], 1,
                DateTime.DaysInMonth(DateOnlyValues[2], DateOnlyValues[1]));
            var newDate = new UnityDateOnly(DateOnlyValues[2], DateOnlyValues[1], DateOnlyValues[0]);
            return newDate;
        }

        public static float GetHeightForFields(int fields)
        {
            return fields * (singleLineHeight + standardVerticalSpacing) - standardVerticalSpacing;
        }

        private static bool LabelHasContent(GUIContent label)
        {
            if (label == null)
                return true;
            return !string.IsNullOrEmpty(label.text) || label.image != null;
        }

        internal static bool ShouldDrawWideMode(GUIContent label) => wideMode || !LabelHasContent(label);

        private static Rect MultiFieldPrefixLabel(Rect totalPosition, GUIContent label, int columns)
        {
            if (!LabelHasContent(label))
                return IndentedRect(totalPosition);

            var fieldPosition = PrefixLabel(totalPosition, label);

            if (wideMode)
            {
                // If there are 2 columns we use the same column widths as if there had been 3 columns
                // in order to make columns line up neatly.
                if (columns == 2)
                {
                    var columnWidth = (fieldPosition.width - (3 - 1) * MultiFieldSpacing) / 3f;
                    fieldPosition.xMax -= (columnWidth + MultiFieldSpacing);
                }

                return fieldPosition;
            }

            fieldPosition = totalPosition;
            fieldPosition.xMin += Indent + IndentPerLevel;
            fieldPosition.yMin += singleLineHeight + standardVerticalSpacing;
            return fieldPosition;
        }
    }
}