using CodeDog.MemoryWoof.Models;
using CodeDog.MemoryWoof.Patterns;

namespace CodeDog.MemoryWoof.Comparers {

    public class Mirror : ComparerConsole {

        public override string Name { get { return "Mirror comparer"; } }
        public override string Description { get { return "Compares pseudo-random pattern with its mirrored copy."; } }

        private ulong Last; // last offset in the sample

        protected override void CreatePattern() {
            var pattern = new XorShiftStar64();
            var length = Sample.Length >> 1;
            ulong seed = pattern.Seed;
            pattern.Next(Sample, 0, length);
            Last = Sample.Length - 1;
            for (ulong i = 0; i < length; i++) Sample[Last - i] = Sample[i];
            Iterations = length;
        }

        public override void Start() {
            Initialize();
            ulong a, b;
            for (Iteration = 0; Iteration < Iterations; Iteration++) {
                a = Sample[Iteration];
                b = Sample[Last - Iteration];
                if (a != b) OnError(new ComparerError { OffsetA = Iteration, OffsetB = Last - Iteration, A = a, B = b });
            }
            OnDone();
        }

    }

}