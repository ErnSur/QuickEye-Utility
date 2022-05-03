using System;
using System.Runtime.CompilerServices;

namespace QuickEye.Utility
{
    public static class MathUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Clamp(long value, long min, long max)
        {
            if (min > max)
            {
                throw new ArgumentException("min value cannot be larger than max");
            }

            if (value < min)
            {
                return min;
            }
            else if (value > max)
            {
                return max;
            }

            return value;
        }
    }
}