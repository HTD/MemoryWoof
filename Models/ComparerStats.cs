using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeDog.MemoryWoof.Models {

    public class ComparerStats {

        public double Tested;
        public double Errors;
        public double Time; // in seconds
        public double Speed; // in GB/s
        public double ErrorRatePer1TB;

    }

}
