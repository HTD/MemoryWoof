using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDog.MemoryWoof.Models {

    public struct ComparerError {

        public ulong OffsetA { get; set; }

        public ulong OffsetB { get; set; }

        public ulong A { get; set; }

        public ulong B { get; set; }

    }

}
