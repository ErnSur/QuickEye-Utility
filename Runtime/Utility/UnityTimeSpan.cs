using System;
using UnityEngine;

namespace QuickEye.Utility
{
    [Serializable]
    public struct UnityTimeSpan
    {
        [SerializeField]
        private long ticks;

        public int Days => TimeSpan.Days;
        public int Hours => TimeSpan.Hours;
        public int Minutes => TimeSpan.Minutes;
        public int Seconds => TimeSpan.Seconds;
        public int Milliseconds => TimeSpan.Milliseconds;
        public long Ticks => TimeSpan.Ticks;
        public double TotalDays => TimeSpan.TotalDays;
        public double TotalHours => TimeSpan.TotalHours;
        public double TotalMinutes => TimeSpan.TotalMinutes;
        public double TotalSeconds => TimeSpan.TotalSeconds;
        public double TotalMilliseconds => TimeSpan.TotalMilliseconds;
        public TimeSpan TimeSpan => new TimeSpan(ticks);

        public UnityTimeSpan(TimeSpan timeSpan) : this()
        {
            ticks = timeSpan.Ticks;
        }

        public UnityTimeSpan(long ticks) : this(new TimeSpan(ticks))
        {
        }

        public UnityTimeSpan(int hours, int minutes, int seconds) : this(new TimeSpan(hours, minutes, seconds))
        {
        }

        public UnityTimeSpan(int days, int hours, int minutes, int seconds) : this(new TimeSpan(days, hours, minutes,
            seconds))
        {
        }

        public UnityTimeSpan(int days, int hours, int minutes, int seconds, int milliseconds) : this(new TimeSpan(days,
            hours, minutes, seconds, milliseconds))
        {
        }

        public static implicit operator TimeSpan(UnityTimeSpan timeSpan) => timeSpan.TimeSpan;
        public static explicit operator UnityTimeSpan(TimeSpan timeSpan) => new UnityTimeSpan(timeSpan);
        public static UnityTimeSpan operator +(UnityTimeSpan d, TimeSpan t) => new UnityTimeSpan(d.TimeSpan + t);
        public static UnityTimeSpan operator -(UnityTimeSpan d, TimeSpan t) => new UnityTimeSpan(d.TimeSpan - t);
        public static bool operator ==(UnityTimeSpan d1, UnityTimeSpan d2) => d1.TimeSpan == d2.TimeSpan;
        public static bool operator >(UnityTimeSpan t1, UnityTimeSpan t2) => t1.TimeSpan > t2.TimeSpan;
        public static bool operator >=(UnityTimeSpan t1, UnityTimeSpan t2) => t1.TimeSpan >= t2.TimeSpan;
        public static bool operator !=(UnityTimeSpan d1, UnityTimeSpan d2) => d1.TimeSpan != d2.TimeSpan;
        public static bool operator <(UnityTimeSpan t1, UnityTimeSpan t2) => t1.TimeSpan < t2.TimeSpan;
        public static bool operator <=(UnityTimeSpan t1, UnityTimeSpan t2) => t1.TimeSpan <= t2.TimeSpan;

        public override string ToString() => TimeSpan.ToString();
        public string ToString(string format) => TimeSpan.ToString(format);
        public string ToString(string format, IFormatProvider provider) => TimeSpan.ToString(format, provider);

        public bool Equals(UnityTimeSpan other) => ticks == other.ticks;
        public override bool Equals(object obj) => obj is UnityTimeSpan other && Equals(other);
        public override int GetHashCode() => ticks.GetHashCode();
    }
}