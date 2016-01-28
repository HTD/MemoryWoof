using CodeDog.MemoryWoof.Interfaces;
using CodeDog.MemoryWoof.Models;
using CodeDog.MemoryWoof.Patterns;

namespace CodeDog.MemoryWoof.Comparers {

    public class Direct : ComparerConsole {

        public override string Name { get { return "Direct pattern comparer"; } }
        public override string Description { get { return "Compares pattern in memory with computed values."; } }

        private IPattern Pattern;
        private ulong Seed;

        protected override void CreatePattern() {
            Pattern = new XorShiftStar64();
            Seed = Pattern.Seed;
            Pattern.Next(Sample);
            Iterations = Sample.Length;
        }

        public override void Start() {
            Initialize();
            Pattern.Seed = Seed;
            ulong a, b;
            for (Iteration = 0; Iteration < Iterations; Iteration++) {
                a = Pattern[Iteration];
                b = Sample[Iteration];
                if (a != b) OnError(new ComparerError { OffsetA = Iteration, OffsetB = Iteration, A = a, B = b });
            }
            OnDone();
        }

    }

}