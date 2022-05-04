using System;
using System.Globalization;
using UnityEngine;

namespace QuickEye.Utility
{
    [Serializable]
    public struct UnityDateOnly : IComparable, IComparable<UnityDateOnly>, IEquatable<UnityDateOnly>, ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private string arrayElementName;
#endif
        [SerializeField]
        private int dayNumber;

        // Maps to Jan 1st year 1
        private const int MinDayNumber = 0;

        // Maps to December 31 year 9999. The value calculated from "new DateTime(9999, 12, 31).Ticks / TimeSpan.TicksPerDay"
        private const int MaxDayNumber = 3_652_058;

        private static int DayNumberFromDateTime(DateTime dt) => (int)((ulong)dt.Ticks / TimeSpan.TicksPerDay);

        private DateTime GetEquivalentDateTime() => new DateTime(dayNumber * TimeSpan.TicksPerDay);

        private UnityDateOnly(int dayNumber): this()
        {
            Debug.Assert((uint)dayNumber <= MaxDayNumber);
            this.dayNumber = dayNumber;
        }

        /// <summary>
        /// Gets the earliest possible date that can be created.
        /// </summary>
        public static UnityDateOnly MinValue => new UnityDateOnly(MinDayNumber);

        /// <summary>
        /// Gets the latest possible date that can be created.
        /// </summary>
        public static UnityDateOnly MaxValue => new UnityDateOnly(MaxDayNumber);

        /// <summary>
        /// Creates a new instance of the UnityDateOnly structure to the specified year, month, and day.
        /// </summary>
        /// <param name="year">The year (1 through 9999).</param>
        /// <param name="month">The month (1 through 12).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month" />).</param>
        public UnityDateOnly(int year, int month, int day): this() =>
            dayNumber = DayNumberFromDateTime(new DateTime(year, month, day));

        /// <summary>
        /// Creates a new instance of the UnityDateOnly structure to the specified year, month, and day for the specified calendar.
        /// </summary>
        /// <param name="year">The year (1 through the number of years in calendar).</param>
        /// <param name="month">The month (1 through the number of months in calendar).</param>
        /// <param name="day">The day (1 through the number of days in <paramref name="month"/>).</param>
        /// <param name="calendar">The calendar that is used to interpret year, month, and day.<paramref name="month"/>.</param>
        public UnityDateOnly(int year, int month, int day, Calendar calendar): this() =>
            dayNumber = DayNumberFromDateTime(new DateTime(year, month, day, calendar));

        /// <summary>
        /// Creates a new instance of the UnityDateOnly structure to the specified number of days.
        /// </summary>
        /// <param name="dayNumber">The number of days since January 1, 0001 in the Proleptic Gregorian calendar.</param>
        public static UnityDateOnly FromDayNumber(int dayNumber)
        {
            if ((uint)dayNumber > MaxDayNumber)
            {
                throw new ArgumentOutOfRangeException(nameof(dayNumber));
            }

            return new UnityDateOnly(dayNumber);
        }

        /// <summary>
        /// Gets the year component of the date represented by this instance.
        /// </summary>
        public int Year => GetEquivalentDateTime().Year;

        /// <summary>
        /// Gets the month component of the date represented by this instance.
        /// </summary>
        public int Month => GetEquivalentDateTime().Month;

        /// <summary>
        /// Gets the day component of the date represented by this instance.
        /// </summary>
        public int Day => GetEquivalentDateTime().Day;

        /// <summary>
        /// Gets the day of the week represented by this instance.
        /// </summary>
        public DayOfWeek DayOfWeek => (DayOfWeek)(((uint)dayNumber + 1) % 7);

        /// <summary>
        /// Gets the day of the year represented by this instance.
        /// </summary>
        public int DayOfYear => GetEquivalentDateTime().DayOfYear;

        /// <summary>
        /// Gets the number of days since January 1, 0001 in the Proleptic Gregorian calendar represented by this instance.
        /// </summary>
        public int DayNumber => dayNumber;

        /// <summary>
        /// Adds the specified number of days to the value of this instance.
        /// </summary>
        /// <param name="value">The number of days to add. To subtract days, specify a negative number.</param>
        /// <returns>An instance whose value is the sum of the date represented by this instance and the number of days represented by value.</returns>
        public UnityDateOnly AddDays(int value)
        {
            int newDayNumber = dayNumber + value;
            if ((uint)newDayNumber > MaxDayNumber)
            {
                ThrowOutOfRange();
            }

            return new UnityDateOnly(newDayNumber);

            void ThrowOutOfRange() =>
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Adds the specified number of months to the value of this instance.
        /// </summary>
        /// <param name="value">A number of months. The months parameter can be negative or positive.</param>
        /// <returns>An object whose value is the sum of the date represented by this instance and months.</returns>
        public UnityDateOnly AddMonths(int value) =>
            new UnityDateOnly(DayNumberFromDateTime(GetEquivalentDateTime().AddMonths(value)));

        /// <summary>
        /// Adds the specified number of years to the value of this instance.
        /// </summary>
        /// <param name="value">A number of years. The value parameter can be negative or positive.</param>
        /// <returns>An object whose value is the sum of the date represented by this instance and the number of years represented by value.</returns>
        public UnityDateOnly AddYears(int value) =>
            new UnityDateOnly(DayNumberFromDateTime(GetEquivalentDateTime().AddYears(value)));

        /// <summary>
        /// Determines whether two specified instances of UnityDateOnly are equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if left and right represent the same date; otherwise, false.</returns>
        public static bool operator ==(UnityDateOnly left, UnityDateOnly right) => left.dayNumber == right.dayNumber;

        /// <summary>
        /// Determines whether two specified instances of UnityDateOnly are not equal.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if left and right do not represent the same date; otherwise, false.</returns>
        public static bool operator !=(UnityDateOnly left, UnityDateOnly right) => left.dayNumber != right.dayNumber;

        /// <summary>
        /// Determines whether one specified UnityDateOnly is later than another specified DateTime.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if left is later than right; otherwise, false.</returns>
        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther}.op_GreaterThan(TSelf, TOther)" />
        public static bool operator >(UnityDateOnly left, UnityDateOnly right) => left.dayNumber > right.dayNumber;

        /// <summary>
        /// Determines whether one specified UnityDateOnly represents a date that is the same as or later than another specified UnityDateOnly.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if left is the same as or later than right; otherwise, false.</returns>
        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther}.op_GreaterThanOrEqual(TSelf, TOther)" />
        public static bool operator >=(UnityDateOnly left, UnityDateOnly right) => left.dayNumber >= right.dayNumber;

        /// <summary>
        /// Determines whether one specified UnityDateOnly is earlier than another specified UnityDateOnly.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if left is earlier than right; otherwise, false.</returns>
        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther}.op_LessThan(TSelf, TOther)" />
        public static bool operator <(UnityDateOnly left, UnityDateOnly right) => left.dayNumber < right.dayNumber;

        /// <summary>
        /// Determines whether one specified UnityDateOnly represents a date that is the same as or earlier than another specified UnityDateOnly.
        /// </summary>
        /// <param name="left">The first object to compare.</param>
        /// <param name="right">The second object to compare.</param>
        /// <returns>true if left is the same as or earlier than right; otherwise, false.</returns>
        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther}.op_LessThanOrEqual(TSelf, TOther)" />
        public static bool operator <=(UnityDateOnly left, UnityDateOnly right) => left.dayNumber <= right.dayNumber;

        /// <summary>
        /// Returns a UnityDateOnly instance that is set to the date part of the specified dateTime.
        /// </summary>
        /// <param name="dateTime">The DateTime instance.</param>
        /// <returns>The UnityDateOnly instance composed of the date part of the specified input time dateTime instance.</returns>
        public static UnityDateOnly FromDateTime(DateTime dateTime) => new UnityDateOnly(DayNumberFromDateTime(dateTime));

        /// <summary>
        /// Compares the value of this instance to a specified UnityDateOnly value and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified DateTime value.
        /// </summary>
        /// <param name="value">The object to compare to the current instance.</param>
        /// <returns>Less than zero if this instance is earlier than value. Greater than zero if this instance is later than value. Zero if this instance is the same as value.</returns>
        public int CompareTo(UnityDateOnly value) => dayNumber.CompareTo(value.dayNumber);

        /// <summary>
        /// Compares the value of this instance to a specified object that contains a specified UnityDateOnly value, and returns an integer that indicates whether this instance is earlier than, the same as, or later than the specified UnityDateOnly value.
        /// </summary>
        /// <param name="value">A boxed object to compare, or null.</param>
        /// <returns>Less than zero if this instance is earlier than value. Greater than zero if this instance is later than value. Zero if this instance is the same as value.</returns>
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is UnityDateOnly unityDateOnly))
            {
                throw new ArgumentException("Must be UnityDateOnly", nameof(value));
            }

            return CompareTo(unityDateOnly);
        }

        /// <summary>
        /// Returns a value indicating whether the value of this instance is equal to the value of the specified UnityDateOnly instance.
        /// </summary>
        /// <param name="value">The object to compare to this instance.</param>
        /// <returns>true if the value parameter equals the value of this instance; otherwise, false.</returns>
        public bool Equals(UnityDateOnly value) => dayNumber == value.dayNumber;

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="value">The object to compare to this instance.</param>
        /// <returns>true if value is an instance of UnityDateOnly and equals the value of this instance; otherwise, false.</returns>
        public override bool Equals(object value) =>
            value is UnityDateOnly unityDateOnly && dayNumber == unityDateOnly.dayNumber;

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer hash code.</returns>
        public override int GetHashCode() => dayNumber;

        /// <summary>
        /// Converts the value of the current UnityDateOnly object to its equivalent long date string representation.
        /// </summary>
        /// <returns>A string that contains the long date string representation of the current UnityDateOnly object.</returns>
        public string ToLongDateString() => ToString("D");

        /// <summary>
        /// Converts the value of the current UnityDateOnly object to its equivalent short date string representation.
        /// </summary>
        /// <returns>A string that contains the short date string representation of the current UnityDateOnly object.</returns>
        public string ToShortDateString() => ToString();

        /// <summary>
        /// Converts the value of the current UnityDateOnly object to its equivalent string representation using the formatting conventions of the current culture.
        /// The UnityDateOnly object will be formatted in short form.
        /// </summary>
        /// <returns>A string that contains the short date string representation of the current UnityDateOnly object.</returns>
        public override string ToString() => ToString("d");

        /// <summary>
        /// Converts the value of the current UnityDateOnly object to its equivalent string representation using the specified format and the formatting conventions of the current culture.
        /// </summary>
        /// <param name="format">A standard or custom date format string.</param>
        /// <returns>A string representation of value of the current UnityDateOnly object as specified by format.</returns>
        public string ToString(string format) =>
            ToString(format, null);

        /// <summary>
        /// Converts the value of the current UnityDateOnly object to its equivalent string representation using the specified culture-specific format information.
        /// </summary>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of value of the current UnityDateOnly object as specified by provider.</returns>
        public string ToString(IFormatProvider provider) => ToString("d", provider);

        /// <summary>
        /// Converts the value of the current UnityDateOnly object to its equivalent string representation using the specified culture-specific format information.
        /// </summary>
        /// <param name="format">A standard or custom date format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <returns>A string representation of value of the current UnityDateOnly object as specified by format and provider.</returns>
        public string ToString(string format,
            IFormatProvider provider)
        {
            return GetEquivalentDateTime().ToString(format, provider);
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
#if UNITY_EDITOR
            arrayElementName = GetEquivalentDateTime().ToString("d/M/yyyy");
#endif
        }
    }
}