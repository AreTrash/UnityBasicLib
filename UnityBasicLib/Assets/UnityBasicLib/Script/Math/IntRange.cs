using System;

namespace UnityBasicLib
{
    public abstract class IntRange
    {
        public static IntRange Linear(int min, int max) => new LinearIntRange(min, max);
        public static IntRange Tasty(int min, int max, int count = 10) => new TastyIntRange(min, max, count);

        public int Min { get; }
        public int Max { get; }

        public IntRange(int min, int max)
        {
            if (min > max)
            {
                throw new ArgumentException("require: min < max");
            }
            
            Min = min;
            Max = max;
        }

        public bool IsInside(int value)
        {
            return Min <= value && value <= Max;
        }

        public abstract int Random();

        class LinearIntRange : IntRange
        {
            public LinearIntRange(int min, int max) : base(min, max)
            {
            }

            public override int Random()
            {
                return UnityEngine.Random.Range(Min, Max);
            }
        }

        class TastyIntRange : IntRange
        {
            readonly int count;

            public TastyIntRange(int min, int max, int count) : base(min, max)
            {
                this.count = count;
            }

            public override int Random()
            {
                var ret = 0d;
                for (var i = 0; i < count; i++) ret += UnityEngine.Random.Range(Min, Max);
                return (int)Math.Round(ret / count);
            }
        }
    }
}