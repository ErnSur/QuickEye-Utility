namespace QuickEye.Utility
{
    /// <summary>
    /// int struct that goes back to 0 when value equals length - 1
    /// and goes back to length - 1 when equals -1
    /// </summary>
    public struct RepeatingInt
    {
        public int Length;

        private int _value;

        public int Value
        {
            get => _value;
            set => _value = (value % Length + Length) % Length;
        }

        /// <param name="length">Exclusive</param>
        public RepeatingInt(int length) : this(0, length) { }

        /// <param name="length">Exclusive</param>
        public RepeatingInt(int value, int length) => (_value, Length) = (value, length);

        public static implicit operator int(RepeatingInt v)
        {
            return v._value;
        }

        public static RepeatingInt operator ++(RepeatingInt rhs)
        {
            return new RepeatingInt(rhs._value + 1, rhs.Length);
        }

        public static RepeatingInt operator --(RepeatingInt rhs)
        {
            return new RepeatingInt(rhs._value - 1, rhs.Length);
        }
    }
}