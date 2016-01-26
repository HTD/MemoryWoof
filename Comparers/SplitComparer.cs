using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeDog.Algorithms;
using CodeDog.MemoryWoof.Interfaces;
using CodeDog.MemoryWoof.Models;

namespace CodeDog.MemoryWoof.Comparers {

    public class SplitComparer : ComparerConsole {

        public override string Name { get { return "Basic split comparer"; } }
        public override string Description { get { return "Compares 2 identical pseudo-random patterns."; } }

        protected override void CreatePattern() {
            var pattern = new XorShiftStar64();
            var length = Sample.Length >> 1;
            ulong seed = 42;
            pattern.Seed = seed;
            pattern.Next(Sample, 0, length);
            pattern.Seed = seed;
            pattern.Next(Sample, length, length);
            Iterations = length;
        }

        public override void Start() {
            Initialize();
            ulong a, b;
            //Sample[0x1234567] = 0; // TEST ERROR
            for (Iteration = 0; Iteration < Iterations; Iteration++) {
                a = Sample[Iteration];
                b = Sample[Iterations + Iteration];
                if (a != b) OnError(new ComparerError { OffsetA = Iteration, OffsetB = Iterations + Iteration, A = a, B = b });
                if (T > 0 && Iteration % T < 1) OnProgress();
            }
            OnDone();
        }

    }

}
