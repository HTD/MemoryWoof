using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;

namespace CodeDog.Core {

    public static class Memory {

        const ulong MEM_SAFETY_MARGIN = 0x20000000; // 512MB

        public static ulong MaxAvailable {
            get {
                return new ComputerInfo().AvailablePhysicalMemory;
            }
        }

        public static ulong Available {
            get {
#if DEBUG1
                var a = MaxAvailable - MEM_SAFETY_MARGIN;
                return (a > 0x40000000) ? 0x40000000 : a;
#else
                return MaxAvailable - MEM_SAFETY_MARGIN;
#endif
            }
        }

        public static double ToGibi(ulong size) => Math.Round(size / (double)0x40000000, 3);

    }

}
