using System;
using CodeDog.Core;
using CodeDog.MemoryWoof.Interfaces;

namespace CodeDog.MemoryWoof.Patterns {

    /// <summary>
    /// Turbo XorShift* PRNG
    /// </summary>
    public class XorShiftStar64 : IPattern {

        private ulong State;
        private ulong Offset;

        public ulong Seed {
            get { return State; }
            set {
                State = value > 0 ? value : 0x8000000000000000 | (ulong)DateTime.Now.Ticks;
                Offset = 0;
            }
        }

        public XorShiftStar64(ulong seed = 0) {
            Seed = seed;
        }

        public void Next(Paged<ulong> buffer, ulong offset, ulong length) {
            ulong limit = offset + length;
            for (ulong i = offset; i < limit; i++) {
                State ^= State >> 12;
                State ^= State << 25;
                State ^= State >> 27;
                buffer[i] = State * 2685821657736338717;
            }
            Offset += offset + limit;
        }

        public void Next(Paged<ulong> buffer) => Next(buffer, 0, buffer.Length);

        public ulong this[ulong i] {
            get {
                if (i > 0 && i < Offset) Seed = Seed;
                while (true) {
                    State ^= State >> 12;
                    State ^= State << 25;
                    State ^= State >> 27;
                    Offset++;
                    if (i == Offset - 1) return State * 2685821657736338717;
                }
            }
        }

    }

}