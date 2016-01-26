using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDog.MemoryWoof.Models {

    public class ComparerState {

        public DateTime Time_FirstStarted { get; set; }

        public DateTime Time_LastStarted { get; set; }

        public DateTime Time_LastDone { get; set; }

        public double Speed { get; set; }

        public ulong SampleSize { get; set; }

        public ulong Tested_Last { get; set; }

        public ulong Tested_Total { get; set; }

        public ulong Errors { get; set; }

    }

}
