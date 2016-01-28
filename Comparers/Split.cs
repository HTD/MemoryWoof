using CodeDog.MemoryWoof.Models;
using CodeDog.MemoryWoof.Patterns;

namespace CodeDog.MemoryWoof.Comparers {

    public class Split : ComparerConsole {

        public override string Name { get { return "Basic split comparer"; } }
        public override string Description { get { return "Compares 2 identical pseudo-random patterns."; } }

        protected override void CreatePattern() {
            var pattern = new XorShiftStar64();
            var length = Sample.Length >> 1;
            ulong seed = pattern.Seed;
            pattern.Next(Sample, 0, length);
            pattern.Seed = seed;
            pattern.Next(Sample, length, length);
            Iterations = length;
        }

        public override void Start() {
            Initialize();
            ulong a, b, c;
            for (Iteration = 0, c = Iterations; Iteration < Iterations; Iteration++, c++) {
                a = Sample[Iteration];
                b = Sample[c];
                if (a != b) OnError(new ComparerError { OffsetA = Iteration, OffsetB = c, A = a, B = b });
            }
            OnDone();
        }

    }

}