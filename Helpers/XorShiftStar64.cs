using System;
using CodeDog.System;

namespace CodeDog.Algorithms {

    /// <summary>
    /// Turbo XorShift* PRNG
    /// </summary>
    public class XorShiftStar64 {

        private ulong State;

        public ulong Seed {
            get { return State; }
            set {
                State = value > 0 ? value : 0x8000000000000000 | (ulong)DateTime.Now.Ticks;
            }
        }

        public XorShiftStar64(ulong seed = 0) {
            Seed = seed;
        }

        public void Next(MemBlock<ulong> buffer, ulong offset, ulong length) {
            ulong limit = offset + length;
            for (ulong i = offset; i < limit; i++) {
                State ^= State >> 12;
                State ^= State << 25;
                State ^= State >> 27;
                buffer[i] = State * 2685821657736338717;
            }
        }

        public void Next(MemBlock<ulong> buffer) => Next(buffer, 0, buffer.Length);

    }

}