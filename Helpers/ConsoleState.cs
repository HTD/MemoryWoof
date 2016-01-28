using System;

namespace CodeDog.Core.ConsoleExtensions {

    public class ConsoleState {

        public static object Lock = new object();

        public ConsoleColor BackgroundColor;
        public ConsoleColor ForegroundColor;
        public int X;
        public int Y;

        public ConsoleState() {
            BackgroundColor = Console.BackgroundColor;
            ForegroundColor = Console.ForegroundColor;
            X = Console.CursorLeft;
            Y = Console.CursorTop;
        }

        public static void Set(ConsoleState s) {
            Console.BackgroundColor = s.BackgroundColor;
            Console.ForegroundColor = s.ForegroundColor;
            Console.SetCursorPosition(s.X, s.Y);
        }
    }

}