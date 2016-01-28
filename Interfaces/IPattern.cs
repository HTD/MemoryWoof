using CodeDog.Core;

namespace CodeDog.MemoryWoof.Interfaces {

    interface IPattern {

        ulong Seed { get; set; }

        void Next(Paged<ulong> buffer);

        void Next(Paged<ulong> buffer, ulong offset, ulong length);

        ulong this[ulong i] { get; }

    }

}
