using System;
using NUnit.Framework;

namespace QuickEye.Utility.Tests
{
    public class UnityTimeSpanTests
    {
        [Test]
        [TestCase(0, 2)]
        [TestCase(-1, -1)]
        [TestCase(11, 10)]
        [TestCase(999, -999)]
        public void Comparison(int ticks1, int ticks2)
        {
            var ts = new TimeSpan(ticks1);
            var uts = new UnityTimeSpan(ticks2);

            Assert.AreEqual(ts == uts, ticks1 == ticks2);
            Assert.AreEqual(ts != uts, ticks1 != ticks2);
            Assert.AreEqual(ts > uts, ticks1 > ticks2);
            Assert.AreEqual(ts < uts, ticks1 < ticks2);
            Assert.AreEqual(ts >= uts, ticks1 >= ticks2);
            Assert.AreEqual(ts <= uts, ticks1 <= ticks2);

            Assert.AreEqual(uts == ts, ticks2 == ticks1);
            Assert.AreEqual(uts != ts, ticks2 != ticks1);
            Assert.AreEqual(uts > ts, ticks2 > ticks1);
            Assert.AreEqual(uts < ts, ticks2 < ticks1);
            Assert.AreEqual(uts >= ts, ticks2 >= ticks1);
            Assert.AreEqual(uts <= ts, ticks2 <= ticks1);
        }

        [Test]
        public void RandomComparison()
        {
            for (int i = 0; i < 100; i++)
            {
                Comparison(UnityEngine.Random.Range(-1000, 1000), UnityEngine.Random.Range(-1000, 1000));
            }
        }
    }
}