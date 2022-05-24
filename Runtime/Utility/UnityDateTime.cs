using System;
using UnityEngine;

namespace QuickEye.Utility
{
    [Serializable]
    public struct UnityDateTime : ISerializationCallbackReceiver
    {
#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private string arrayElementName;
#endif
        [SerializeField]
        private UnityDateOnly date;

        [SerializeField,TimeOfDay]
        private UnityTimeSpan time;

        public int Day => DateTime.Day;
        public int Month => DateTime.Month;
        public int Year => DateTime.Year;
        public int Hour => DateTime.Hour;
        public int Minute => DateTime.Minute;
        public int Second => DateTime.Second;
        public int Millisecond => DateTime.Millisecond;
        public long Ticks => DateTime.Ticks;
        public TimeSpan TimeOfDay => DateTime.TimeOfDay;
        public DateTime DateTime => new DateTime(date.Year, date.Month, date.Day) + time;

        public UnityDateTime(DateTime dateTime) : this()
        {
            date = UnityDateOnly.FromDateTime(dateTime);
            time = new UnityTimeSpan(dateTime.TimeOfDay);
        }

        public UnityDateTime(long ticks) : this(new DateTime(ticks))
        {
        }

        public UnityDateTime(int year, int month, int day) : this(new DateTime(year, month, day))
        {
        }

        public static implicit operator DateTime(UnityDateTime unityDateTime) => unityDateTime.DateTime;
        public static explicit operator UnityDateTime(DateTime date) => new UnityDateTime(date);
        public static UnityDateTime operator +(UnityDateTime d, TimeSpan t) => new UnityDateTime(d.DateTime + t);
        public static UnityDateTime operator -(UnityDateTime d, TimeSpan t) => new UnityDateTime(d.DateTime - t);
        public static bool operator ==(UnityDateTime d1, UnityDateTime d2) => d1.DateTime == d2.DateTime;
        public static bool operator >(UnityDateTime t1, UnityDateTime t2) => t1.DateTime > t2.DateTime;
        public static bool operator >=(UnityDateTime t1, UnityDateTime t2) => t1.DateTime >= t2.DateTime;
        public static bool operator !=(UnityDateTime d1, UnityDateTime d2) => d1.DateTime != d2.DateTime;
        public static bool operator <(UnityDateTime t1, UnityDateTime t2) => t1.DateTime < t2.DateTime;
        public static bool operator <=(UnityDateTime t1, UnityDateTime t2) => t1.DateTime <= t2.DateTime;

        public override string ToString() => DateTime.ToString();
        public string ToString(string format) => DateTime.ToString(format);
        public string ToString(IFormatProvider provider) => DateTime.ToString(provider);
        public string ToString(string format, IFormatProvider provider) => DateTime.ToString(format, provider);

        public bool Equals(UnityDateTime other) => (date, time) == (other.date, other.time);
        public override bool Equals(object obj) => obj is UnityDateTime other && Equals(other);
        public override int GetHashCode() => Ticks.GetHashCode();

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
#if UNITY_EDITOR
            arrayElementName = DateTime.ToString("d/M/yyyy");
            if (DateTime.TimeOfDay != TimeSpan.Zero)
                arrayElementName += " +Time";
#endif
        }
    }
}