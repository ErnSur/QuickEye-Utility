using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

namespace QuickEye.Utility
{
    // Unity specific additions to basic TimeSpan class
    public partial struct UnityTimeSpan : IComparable<TimeSpan>, IEquatable<TimeSpan>
    {
        [FormerlySerializedAs("ticks")]
        [SerializeField]
        long _ticks;

        internal UnityTimeSpan(TimeSpan timespan) : this(timespan.Ticks)
        {
        }

        public int CompareTo(TimeSpan other)
        {
            long t = other.Ticks;
            if (_ticks > t) return 1;
            if (_ticks < t) return -1;
            return 0;
        }

        public bool Equals(TimeSpan other)
        {
            return _ticks == other.Ticks;
        }

        public static implicit operator TimeSpan(UnityTimeSpan timeSpan) => new TimeSpan(timeSpan._ticks);
        public static implicit operator UnityTimeSpan(TimeSpan timeSpan) => new UnityTimeSpan(timeSpan.Ticks);

        public static bool operator <(UnityTimeSpan t1, TimeSpan t2) => t1.Ticks < t2.Ticks;
        public static bool operator >(UnityTimeSpan t1, TimeSpan t2) => t1.Ticks > t2.Ticks;
        public static bool operator <=(UnityTimeSpan t1, TimeSpan t2) => t1.Ticks <= t2.Ticks;
        public static bool operator >=(UnityTimeSpan t1, TimeSpan t2) => t1.Ticks >= t2.Ticks;
        
        public static bool operator <(TimeSpan t1, UnityTimeSpan t2) => t1.Ticks < t2.Ticks;
        public static bool operator >(TimeSpan t1, UnityTimeSpan t2) => t1.Ticks > t2.Ticks;
        public static bool operator <=(TimeSpan t1, UnityTimeSpan t2) => t1.Ticks <= t2.Ticks;
        public static bool operator >=(TimeSpan t1, UnityTimeSpan t2) => t1.Ticks >= t2.Ticks;

        public static bool operator ==(UnityTimeSpan d1, TimeSpan d2) => d1.Ticks == d2.Ticks;
        public static bool operator !=(UnityTimeSpan d1, TimeSpan d2) => d1.Ticks != d2.Ticks;
        
        public static bool operator ==(TimeSpan d1, UnityTimeSpan d2) => d1.Ticks == d2.Ticks;
        public static bool operator !=(TimeSpan d1, UnityTimeSpan d2) => d1.Ticks != d2.Ticks;

        public static UnityTimeSpan operator +(UnityTimeSpan t1, TimeSpan t2) => t1.Add(new UnityTimeSpan(t2.Ticks));
        public static UnityTimeSpan operator -(UnityTimeSpan t1, TimeSpan t2) => t1.Subtract(new UnityTimeSpan(t2.Ticks));

        public static UnityTimeSpan operator +(TimeSpan t2, UnityTimeSpan t1) => t1.Add(new UnityTimeSpan(t2.Ticks));
        public static UnityTimeSpan operator -(TimeSpan t1, UnityTimeSpan t2) => t1.Subtract(t2);
    }
    
    // Updated version of System.TimeSpan
    [Serializable]
    public partial struct UnityTimeSpan
        : IComparable,
            IComparable<UnityTimeSpan>,
            IEquatable<UnityTimeSpan>
    {
        /// <summary>
        /// Represents the number of nanoseconds per tick. This field is constant.
        /// </summary>
        /// <remarks>
        /// The value of this constant is 100.
        /// </remarks>
        public const long NanosecondsPerTick = 100;

        /// <summary>
        /// Represents the number of ticks in 1 microsecond. This field is constant.
        /// </summary>
        /// <remarks>
        /// The value of this constant is 10.
        /// </remarks>
        public const long TicksPerMicrosecond = 10;

        /// <summary>
        /// Represents the number of ticks in 1 millisecond. This field is constant.
        /// </summary>
        /// <remarks>
        /// The value of this constant is 10 thousand; that is, 10,000.
        /// </remarks>
        public const long TicksPerMillisecond = TicksPerMicrosecond * 1000;

        public const long TicksPerSecond = TicksPerMillisecond * 1000;   // 10,000,000

        public const long TicksPerMinute = TicksPerSecond * 60;         // 600,000,000

        public const long TicksPerHour = TicksPerMinute * 60;        // 36,000,000,000

        public const long TicksPerDay = TicksPerHour * 24;          // 864,000,000,000

        internal const long MaxSeconds = long.MaxValue / TicksPerSecond;
        internal const long MinSeconds = long.MinValue / TicksPerSecond;

        internal const long MaxMilliSeconds = long.MaxValue / TicksPerMillisecond;
        internal const long MinMilliSeconds = long.MinValue / TicksPerMillisecond;

        internal const long MaxMicroSeconds = long.MaxValue / TicksPerMicrosecond;
        internal const long MinMicroSeconds = long.MinValue / TicksPerMicrosecond;

        internal const long TicksPerTenthSecond = TicksPerMillisecond * 100;

        public static readonly UnityTimeSpan Zero = new UnityTimeSpan(0);

        public static readonly UnityTimeSpan MaxValue = new UnityTimeSpan(long.MaxValue);
        public static readonly UnityTimeSpan MinValue = new UnityTimeSpan(long.MinValue);

        public UnityTimeSpan(long ticks)
        {
            this._ticks = ticks;
        }

        public UnityTimeSpan(int hours, int minutes, int seconds)
        {
            _ticks = TimeToTicks(hours, minutes, seconds);
        }

        public UnityTimeSpan(int days, int hours, int minutes, int seconds)
            : this(days, hours, minutes, seconds, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityTimeSpan"/> structure to a specified number of
        /// days, hours, minutes, seconds, and milliseconds.
        /// </summary>
        /// <param name="days">Number of days.</param>
        /// <param name="hours">Number of hours.</param>
        /// <param name="minutes">Number of minutes.</param>
        /// <param name="seconds">Number of seconds.</param>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <remarks>
        /// The specified <paramref name="days"/>, <paramref name="hours"/>, <paramref name="minutes"/>, <paramref name="seconds"/>
        /// and <paramref name="milliseconds"/> are converted to ticks, and that value initializes this instance.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The parameters specify a <see cref="UnityTimeSpan"/> value less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>
        /// </exception>
        public UnityTimeSpan(int days, int hours, int minutes, int seconds, int milliseconds) :
            this(days, hours, minutes, seconds, milliseconds, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnityTimeSpan"/> structure to a specified number of
        /// days, hours, minutes, seconds, and milliseconds.
        /// </summary>
        /// <param name="days">Number of days.</param>
        /// <param name="hours">Number of hours.</param>
        /// <param name="minutes">Number of minutes.</param>
        /// <param name="seconds">Number of seconds.</param>
        /// <param name="milliseconds">Number of milliseconds.</param>
        /// <param name="microseconds">Number of microseconds.</param>
        /// <remarks>
        /// The specified <paramref name="days"/>, <paramref name="hours"/>, <paramref name="minutes"/>, <paramref name="seconds"/>
        /// <paramref name="milliseconds"/> and <paramref name="microseconds"/> are converted to ticks, and that value initializes this instance.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The parameters specify a <see cref="UnityTimeSpan"/> value less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>
        /// </exception>
        public UnityTimeSpan(int days, int hours, int minutes, int seconds, int milliseconds, int microseconds)
        {
            long totalMicroseconds = (((long)days * 3600 * 24 + (long)hours * 3600 + (long)minutes * 60 + seconds) * 1000 + milliseconds) * 1000 + microseconds;
            if (totalMicroseconds > MaxMicroSeconds || totalMicroseconds < MinMicroSeconds)
                throw new ArgumentOutOfRangeException(null,SR.Overflow_TimeSpanTooLong);
            _ticks = totalMicroseconds * TicksPerMicrosecond;
        }

        public long Ticks => _ticks;

        public int Days => (int)(_ticks / TicksPerDay);

        public int Hours => (int)((_ticks / TicksPerHour) % 24);

        public int Milliseconds => (int)((_ticks / TicksPerMillisecond) % 1000);

        /// <summary>
        /// Gets the microseconds component of the time interval represented by the current <see cref="UnityTimeSpan"/> structure.
        /// </summary>
        /// <remarks>
        /// The <see cref="Microseconds"/> property represents whole microseconds, whereas the
        /// <see cref="TotalMicroseconds"/> property represents whole and fractional microseconds.
        /// </remarks>
        public int Microseconds => (int)((_ticks / TicksPerMicrosecond) % 1000);

        /// <summary>
        /// Gets the nanoseconds component of the time interval represented by the current <see cref="UnityTimeSpan"/> structure.
        /// </summary>
        /// <remarks>
        /// The <see cref="Nanoseconds"/> property represents whole nanoseconds, whereas the
        /// <see cref="TotalNanoseconds"/> property represents whole and fractional nanoseconds.
        /// </remarks>
        public int Nanoseconds => (int)((_ticks % TicksPerMicrosecond) * 100);

        public int Minutes => (int)((_ticks / TicksPerMinute) % 60);

        public int Seconds => (int)((_ticks / TicksPerSecond) % 60);

        public double TotalDays => ((double)_ticks) / TicksPerDay;

        public double TotalHours => (double)_ticks / TicksPerHour;

        public double TotalMilliseconds
        {
            get
            {
                double temp = (double)_ticks / TicksPerMillisecond;
                if (temp > MaxMilliSeconds)
                    return (double)MaxMilliSeconds;

                if (temp < MinMilliSeconds)
                    return (double)MinMilliSeconds;

                return temp;
            }
        }

        /// <summary>
        /// Gets the value of the current <see cref="UnityTimeSpan"/> structure expressed in whole and fractional microseconds.
        /// </summary>
        /// <remarks>
        /// This property converts the value of this instance from ticks to microseconds.
        /// This number might include whole and fractional microseconds.
        ///
        /// The <see cref="TotalMicroseconds"/> property represents whole and fractional microseconds,
        /// whereas the <see cref="Microseconds"/> property represents whole microseconds.
        /// </remarks>
        public double TotalMicroseconds => (double)_ticks / TicksPerMicrosecond;

        /// <summary>
        /// Gets the value of the current <see cref="UnityTimeSpan"/> structure expressed in whole and fractional nanoseconds.
        /// </summary>
        /// <remarks>
        /// This property converts the value of this instance from ticks to nanoseconds.
        /// This number might include whole and fractional nanoseconds.
        ///
        /// The <see cref="TotalNanoseconds"/> property represents whole and fractional nanoseconds,
        /// whereas the <see cref="Nanoseconds"/> property represents whole nanoseconds.
        /// </remarks>
        public double TotalNanoseconds => (double)_ticks * NanosecondsPerTick;

        public double TotalMinutes => (double)_ticks / TicksPerMinute;

        public double TotalSeconds => (double)_ticks / TicksPerSecond;

        public UnityTimeSpan Add(UnityTimeSpan ts)
        {
            long result = _ticks + ts._ticks;
            // Overflow if signs of operands was identical and result's
            // sign was opposite.
            // >> 63 gives the sign bit (either 64 1's or 64 0's).
            if ((_ticks >> 63 == ts._ticks >> 63) && (_ticks >> 63 != result >> 63))
                throw new OverflowException(SR.Overflow_TimeSpanTooLong);
            return new UnityTimeSpan(result);
        }


        // Compares two UnityTimeSpan values, returning an integer that indicates their
        // relationship.
        //
        public static int Compare(UnityTimeSpan t1, UnityTimeSpan t2)
        {
            if (t1._ticks > t2._ticks) return 1;
            if (t1._ticks < t2._ticks) return -1;
            return 0;
        }

        // Returns a value less than zero if this  object
        public int CompareTo(object value)
        {
            if (value == null) return 1;
            if (!(value is UnityTimeSpan))
                throw new ArgumentException(SR.Arg_MustBeTimeSpan);
            long t = ((UnityTimeSpan)value)._ticks;
            if (_ticks > t) return 1;
            if (_ticks < t) return -1;
            return 0;
        }

        public int CompareTo(UnityTimeSpan value)
        {
            long t = value._ticks;
            if (_ticks > t) return 1;
            if (_ticks < t) return -1;
            return 0;
        }

        public static UnityTimeSpan FromDays(double value)
        {
            return Interval(value, TicksPerDay);
        }

        public UnityTimeSpan Duration()
        {
            if (Ticks == UnityTimeSpan.MinValue.Ticks)
                throw new OverflowException(SR.Overflow_Duration);
            return new UnityTimeSpan(_ticks >= 0 ? _ticks : -_ticks);
        }

        public override bool Equals([NotNullWhen(true)] object value)
        {
            if (value is UnityTimeSpan)
            {
                return _ticks == ((UnityTimeSpan)value)._ticks;
            }
            return false;
        }

        public bool Equals(UnityTimeSpan obj)
        {
            return _ticks == obj._ticks;
        }

        public static bool Equals(UnityTimeSpan t1, UnityTimeSpan t2)
        {
            return t1._ticks == t2._ticks;
        }

        public override int GetHashCode()
        {
            return (int)_ticks ^ (int)(_ticks >> 32);
        }

        public static UnityTimeSpan FromHours(double value)
        {
            return Interval(value, TicksPerHour);
        }

        private static UnityTimeSpan Interval(double value, double scale)
        {
            if (double.IsNaN(value))
                throw new ArgumentException(SR.Arg_CannotBeNaN);
            return IntervalFromDoubleTicks(value * scale);
        }

        private static UnityTimeSpan IntervalFromDoubleTicks(double ticks)
        {
            if ((ticks > long.MaxValue) || (ticks < long.MinValue) || double.IsNaN(ticks))
                throw new OverflowException(SR.Overflow_TimeSpanTooLong);
            if (ticks == long.MaxValue)
                return UnityTimeSpan.MaxValue;
            return new UnityTimeSpan((long)ticks);
        }

        public static UnityTimeSpan FromMilliseconds(double value)
        {
            return Interval(value, TicksPerMillisecond);
        }

        /// <summary>
        /// Returns a <see cref="UnityTimeSpan"/> that represents a specified number of microseconds.
        /// </summary>
        /// <param name="value">A number of microseconds.</param>
        /// <returns>An object that represents <paramref name="value"/>.</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="value"/> is less than <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
        ///
        /// -or-
        ///
        /// <paramref name="value"/> is <see cref="double.PositiveInfinity"/>
        ///
        /// -or-
        ///
        /// <paramref name="value"/> is <see cref="double.NegativeInfinity"/>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> is equal to <see cref="double.NaN"/>.
        /// </exception>
        public static UnityTimeSpan FromMicroseconds(double value)
        {
            // ISSUE: https://github.com/dotnet/runtime/issues/66815
            return Interval(value, TicksPerMicrosecond);
        }

        public static UnityTimeSpan FromMinutes(double value)
        {
            return Interval(value, TicksPerMinute);
        }

        public UnityTimeSpan Negate()
        {
            if (Ticks == UnityTimeSpan.MinValue.Ticks)
                throw new OverflowException(SR.Overflow_NegateTwosCompNum);
            return new UnityTimeSpan(-_ticks);
        }

        public static UnityTimeSpan FromSeconds(double value)
        {
            return Interval(value, TicksPerSecond);
        }

        public UnityTimeSpan Subtract(UnityTimeSpan ts)
        {
            long result = _ticks - ts._ticks;
            // Overflow if signs of operands was different and result's
            // sign was opposite from the first argument's sign.
            // >> 63 gives the sign bit (either 64 1's or 64 0's).
            if ((_ticks >> 63 != ts._ticks >> 63) && (_ticks >> 63 != result >> 63))
                throw new OverflowException(SR.Overflow_TimeSpanTooLong);
            return new UnityTimeSpan(result);
        }

        public UnityTimeSpan Multiply(double factor) => this * factor;

        public UnityTimeSpan Divide(double divisor) => this / divisor;

        public double Divide(UnityTimeSpan ts) => this / ts;

        public static UnityTimeSpan FromTicks(long value)
        {
            return new UnityTimeSpan(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static long TimeToTicks(int hour, int minute, int second)
        {
            // totalSeconds is bounded by 2^31 * 2^12 + 2^31 * 2^8 + 2^31,
            // which is less than 2^44, meaning we won't overflow totalSeconds.
            long totalSeconds = (long)hour * 3600 + (long)minute * 60 + (long)second;
            if (totalSeconds > MaxSeconds || totalSeconds < MinSeconds)
                throw new ArgumentOutOfRangeException(null, SR.Overflow_TimeSpanTooLong);
            return totalSeconds * TicksPerSecond;
        }

        #region ParseAndFormat
        public static UnityTimeSpan Parse(string s)
        {
            return new UnityTimeSpan(System.TimeSpan.Parse(s));
        }

        public static UnityTimeSpan Parse(string input, IFormatProvider formatProvider)
        {
            return new UnityTimeSpan(System.TimeSpan.Parse(input, formatProvider));
        }
#if UNITY_2021_1_OR_NEWER
        public static UnityTimeSpan Parse(ReadOnlySpan<char> input, IFormatProvider formatProvider = null)
        {
            return new UnityTimeSpan(System.TimeSpan.Parse(input, formatProvider));
        }
#endif
        public static UnityTimeSpan ParseExact(string input, string format, IFormatProvider formatProvider)
        {
            return new UnityTimeSpan(System.TimeSpan.ParseExact(input, format, formatProvider));
        }

        public static UnityTimeSpan ParseExact(string input, string[] formats, IFormatProvider formatProvider)
        {
            return new UnityTimeSpan(System.TimeSpan.ParseExact(input, formats, formatProvider));
        }

        public static UnityTimeSpan ParseExact(string input, string format, IFormatProvider formatProvider,
            TimeSpanStyles styles)
        {
            return new UnityTimeSpan(System.TimeSpan.ParseExact(input, format, formatProvider, styles));
        }
#if UNITY_2021_1_OR_NEWER
        public static UnityTimeSpan ParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, TimeSpanStyles styles = TimeSpanStyles.None)
        {
            return new UnityTimeSpan(System.TimeSpan.ParseExact(input, format, formatProvider, styles));
        }
#endif

        public static UnityTimeSpan ParseExact(string input, string[] formats, IFormatProvider formatProvider,
            TimeSpanStyles styles)
        {
            return new UnityTimeSpan(System.TimeSpan.ParseExact(input, formats, formatProvider, styles));
        }
#if UNITY_2021_1_OR_NEWER
        public static UnityTimeSpan ParseExact(ReadOnlySpan<char> input, string[] formats, IFormatProvider formatProvider,
            TimeSpanStyles styles = TimeSpanStyles.None)
        {
            return new UnityTimeSpan(System.TimeSpan.ParseExact(input, formats, formatProvider, styles));
        }
#endif

        public static bool TryParse([NotNullWhen(true)] string s, out UnityTimeSpan result)
        {
            var parsed = System.TimeSpan.TryParse(s, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }

#if UNITY_2021_1_OR_NEWER
        public static bool TryParse(ReadOnlySpan<char> s, out UnityTimeSpan result)
        {
            var parsed = TimeSpan.TryParse(s, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }
#endif

        public static bool TryParse([NotNullWhen(true)] string input, IFormatProvider formatProvider,
            out UnityTimeSpan result)
        {
            var parsed = System.TimeSpan.TryParse(input, formatProvider, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }

#if UNITY_2021_1_OR_NEWER
        public static bool TryParse(ReadOnlySpan<char> input, IFormatProvider formatProvider, out UnityTimeSpan result)
        {
            var parsed = TimeSpan.TryParse(input, formatProvider, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }
#endif

        public static bool TryParseExact([NotNullWhen(true)] string input, [NotNullWhen(true)] string format,
            IFormatProvider formatProvider, out UnityTimeSpan result)
        {
            var parsed = System.TimeSpan.TryParseExact(input, format, formatProvider, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }

#if UNITY_2021_1_OR_NEWER
        public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, out UnityTimeSpan result)
        {
            var parsed = TimeSpan.TryParseExact(input, format, formatProvider, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }
#endif

        public static bool TryParseExact([NotNullWhen(true)] string input, [NotNullWhen(true)] string[] formats,
            IFormatProvider formatProvider, out UnityTimeSpan result)
        {
            var parsed = System.TimeSpan.TryParseExact(input, formats, formatProvider, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }

#if UNITY_2021_1_OR_NEWER
        public static bool TryParseExact(ReadOnlySpan<char> input, [NotNullWhen(true)] string[] formats,
            IFormatProvider formatProvider, out UnityTimeSpan result)
        {
            var parsed = TimeSpan.TryParseExact(input, formats, formatProvider, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }
#endif

        public static bool TryParseExact([NotNullWhen(true)] string input, [NotNullWhen(true)] string format,
            IFormatProvider formatProvider, TimeSpanStyles styles, out UnityTimeSpan result)
        {
            var parsed = System.TimeSpan.TryParseExact(input, format, formatProvider, styles, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }

#if UNITY_2021_1_OR_NEWER
        public static bool TryParseExact(ReadOnlySpan<char> input, ReadOnlySpan<char> format,
            IFormatProvider formatProvider, TimeSpanStyles styles, out UnityTimeSpan result)
        {
            var parsed = TimeSpan.TryParseExact(input, format, formatProvider, styles, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }
#endif

        public static bool TryParseExact([NotNullWhen(true)] string input, [NotNullWhen(true)] string[] formats,
            IFormatProvider formatProvider, TimeSpanStyles styles, out UnityTimeSpan result)
        {
            var parsed = System.TimeSpan.TryParseExact(input, formats, formatProvider, styles, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }

#if UNITY_2021_1_OR_NEWER
        public static bool TryParseExact(ReadOnlySpan<char> input, [NotNullWhen(true)] string[] formats,
            IFormatProvider formatProvider, TimeSpanStyles styles, out UnityTimeSpan result)
        {
            var parsed = TimeSpan.TryParseExact(input, formats, formatProvider, styles, out var res);
            result = new UnityTimeSpan(res.Ticks);
            return parsed;
        }
#endif

        public override string ToString()
        {
            return ((TimeSpan)this).ToString();
        }

        public string ToString(string format)
        {
            return ((TimeSpan)this).ToString(format);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ((TimeSpan)this).ToString(format, formatProvider);
        }
        #endregion

        public static UnityTimeSpan operator -(UnityTimeSpan t)
        {
            if (t._ticks == UnityTimeSpan.MinValue._ticks)
                throw new OverflowException(SR.Overflow_NegateTwosCompNum);
            return new UnityTimeSpan(-t._ticks);
        }

        public static UnityTimeSpan operator -(UnityTimeSpan t1, UnityTimeSpan t2) => t1.Subtract(t2);

        public static UnityTimeSpan operator +(UnityTimeSpan t) => t;

        public static UnityTimeSpan operator +(UnityTimeSpan t1, UnityTimeSpan t2) => t1.Add(t2);

        /// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.op_Multiply(TSelf, TOther)" />
        public static UnityTimeSpan operator *(UnityTimeSpan timeSpan, double factor)
        {
            if (double.IsNaN(factor))
            {
                throw new ArgumentException(SR.Arg_CannotBeNaN, nameof(factor));
            }

            // Rounding to the nearest tick is as close to the result we would have with unlimited
            // precision as possible, and so likely to have the least potential to surprise.
            double ticks = Math.Round(timeSpan.Ticks * factor);
            return IntervalFromDoubleTicks(ticks);
        }

        /// <inheritdoc cref="IMultiplyOperators{TSelf, TOther, TResult}.op_Multiply(TSelf, TOther)" />
        public static UnityTimeSpan operator *(double factor, UnityTimeSpan timeSpan) => timeSpan * factor;

        /// <inheritdoc cref="IDivisionOperators{TSelf, TOther, TResult}.op_Division(TSelf, TOther)" />
        public static UnityTimeSpan operator /(UnityTimeSpan timeSpan, double divisor)
        {
            if (double.IsNaN(divisor))
            {
                throw new ArgumentException(SR.Arg_CannotBeNaN, nameof(divisor));
            }

            double ticks = Math.Round(timeSpan.Ticks / divisor);
            return IntervalFromDoubleTicks(ticks);
        }

        // Using floating-point arithmetic directly means that infinities can be returned, which is reasonable
        // if we consider UnityTimeSpan.FromHours(1) / UnityTimeSpan.Zero asks how many zero-second intervals there are in
        // an hour for which infinity is the mathematic correct answer. Having UnityTimeSpan.Zero / UnityTimeSpan.Zero return NaN
        // is perhaps less useful, but no less useful than an exception.
        /// <inheritdoc cref="IDivisionOperators{TSelf, TOther, TResult}.op_Division(TSelf, TOther)" />
        public static double operator /(UnityTimeSpan t1, UnityTimeSpan t2) => t1.Ticks / (double)t2.Ticks;

        /// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.op_Equality(TSelf, TOther)" />
        public static bool operator ==(UnityTimeSpan t1, UnityTimeSpan t2) => t1._ticks == t2._ticks;

        /// <inheritdoc cref="IEqualityOperators{TSelf, TOther, TResult}.op_Inequality(TSelf, TOther)" />
        public static bool operator !=(UnityTimeSpan t1, UnityTimeSpan t2) => t1._ticks != t2._ticks;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_LessThan(TSelf, TOther)" />
        public static bool operator <(UnityTimeSpan t1, UnityTimeSpan t2) => t1._ticks < t2._ticks;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_LessThanOrEqual(TSelf, TOther)" />
        public static bool operator <=(UnityTimeSpan t1, UnityTimeSpan t2) => t1._ticks <= t2._ticks;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThan(TSelf, TOther)" />
        public static bool operator >(UnityTimeSpan t1, UnityTimeSpan t2) => t1._ticks > t2._ticks;

        /// <inheritdoc cref="IComparisonOperators{TSelf, TOther, TResult}.op_GreaterThanOrEqual(TSelf, TOther)" />
        public static bool operator >=(UnityTimeSpan t1, UnityTimeSpan t2) => t1._ticks >= t2._ticks;
    }
    
    class SR
    {
        public static string Arg_CannotBeNaN => "TimeSpan does not accept floating point Not-a-Number values.";
        public static string Overflow_NegateTwosCompNum =>"Negating the minimum value of a twos complement number is invalid.";
        public static string Overflow_TimeSpanTooLong => "TimeSpan overflowed because the duration is too long.";

        public static string Overflow_Duration =>
            "The duration cannot be returned for TimeSpan.MinValue because the absolute value of TimeSpan.MinValue exceeds the value of TimeSpan.MaxValue.";

        public static string Arg_MustBeTimeSpan => "Object must be of type TimeSpan.";
    }

#if !UNITY_2021_1_OR_NEWER
    public class NotNullWhenAttribute : System.Attribute
    {
        public NotNullWhenAttribute(bool value){}
    }
#endif
}