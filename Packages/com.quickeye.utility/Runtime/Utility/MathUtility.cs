using System;
using System.Runtime.CompilerServices;
using UnityEngine;

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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Repeat(int t, int length)
        {
            return (t % length + length) % length;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PingPong(int t, int length)
        {
            t = Repeat(t, length * 2);
            return length - Mathf.Abs(t - length);
        }
    }
}