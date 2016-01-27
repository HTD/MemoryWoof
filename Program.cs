using CodeDog.MemoryWoof.Comparers;
using CodeDog.System;
using CodeDog.System.ConsoleExtensions;

namespace CodeDog.MemoryWoof {

    public static class Program {

        public const double DefaultTestSizeInGB = 100.0;

        public static ConsoleEx X = new ConsoleEx();

        static void Main(string[] args) {
            ShowCredits();
            Test(args);
            X.Wait(-1);
        }

        static void ShowCredits() {
            var i = new AssemblyInfo();
            X.WriteLine($"<name>{i.Product} {i.Version}</name>");
            X.WriteLine($"<description>{i.Copyright}</description>");
            X.WriteLine();
            X.WriteLine($"<description>{i.Description}</description>");
            X.WriteLine();
        }

        static void Test(string[] args) {
            new SplitComparer().Test(GetTestSizeInGB(args));
        }

        static double GetTestSizeInGB(string[] args) {
            if (args != null && args.Length > 0) {
                string arg = args[0];
                double size = 0;
                bool parseResult = double.TryParse(arg, out size);
                if (!parseResult) return DefaultTestSizeInGB;
                if (size < 0) size = -size;
                if (size < 0.1) return 0;
                return size;
            }
            return DefaultTestSizeInGB;
        }

    }

}
