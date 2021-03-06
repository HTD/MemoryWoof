﻿using CodeDog.MemoryWoof.Comparers;
using CodeDog.Core;
using CodeDog.Core.ConsoleExtensions;

namespace CodeDog.MemoryWoof {

    public static class Program {

        public const double DefaultTestSizeInGB = 100.0;

        public static ConsoleEx X = new ConsoleEx();

        static void Main(string[] args) {
            ShowCredits();
            TestVsTest(args);
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
            new Split().Test(GetTestSizeInGB(args));
        }

        static void TestVsTest(string[] args) {
            new Split().Test(GetTestSizeInGB(args) / 3);
            X.WriteLine();
            new Mirror().Test(GetTestSizeInGB(args) / 3);
            X.WriteLine();
            new Direct().Test(GetTestSizeInGB(args) / 3);
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
