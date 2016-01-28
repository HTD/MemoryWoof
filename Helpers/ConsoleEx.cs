using System;
using System.Collections.Generic;
using System.Threading;

namespace CodeDog.Core.ConsoleExtensions {

    /// <summary>
    /// Console class extension
    /// Allows styled writes, use XML tags for style names
    /// </summary>
    public class ConsoleEx : IConsoleExtension {

        private struct Style {
            public string Name;
            public string TagOpen;
            public string TagClose;
            public ConsoleColor? ForegroundColor;
            public ConsoleColor? BackgroundColor;
        }

        private List<Style> DefinedStyles = new List<Style>();
        private const string TagOpenFormat = "<{0}>";
        private const string TagCloseFormat = "</{0}>";

        public ConsoleState State { get { return new ConsoleState(); } set { ConsoleState.Set(value); } }

        public ConsoleEx() {
            DefineStyle("NAME", ConsoleColor.White);
            DefineStyle("DESCRIPTION", ConsoleColor.Gray);
            DefineStyle("OK", ConsoleColor.DarkGreen);
            DefineStyle("ERROR", ConsoleColor.Red);
            DefineStyle("ERROR-DETAILS", ConsoleColor.DarkRed);
            DefineStyle("STRONG", ConsoleColor.White);
            DefineStyle("SPECIAL1", ConsoleColor.DarkCyan);
            DefineStyle("SPECIAL2", ConsoleColor.DarkMagenta);
            DefineStyle("SPECIAL3", ConsoleColor.DarkYellow);
        }

        public void DefineStyle(string name, ConsoleColor? foregroundColor, ConsoleColor? backgroundColor = null) {
            DefinedStyles.Add(new Style {
                Name = name, ForegroundColor = foregroundColor,
                BackgroundColor = backgroundColor,
                TagOpen = String.Format(TagOpenFormat, name),
                TagClose = String.Format(TagCloseFormat, name)
            });
        }

        public void Write(string s = null, params object[] arg) {
            if (s == null) return;
            if (arg != null && arg.Length > 0) s = String.Format(s, arg);
            var fore = Console.ForegroundColor;
            var back = Console.BackgroundColor;
            var done = false;
            foreach (var style in DefinedStyles) {
                var open = String.Format(TagOpenFormat, style.Name);
                var close = String.Format(TagCloseFormat, style.Name);
                var l1 = style.TagOpen.Length;
                var l2 = style.TagClose.Length;
                var p1 = s.IndexOf(style.TagOpen, StringComparison.InvariantCultureIgnoreCase);
                var p2 = s.IndexOf(style.TagClose, StringComparison.InvariantCultureIgnoreCase);
                if (p1 >= 0 && p2 >= 0) {
                    var part1 = s.Substring(0, p1);
                    var part2 = s.Substring(p1 + l1, p2 - p1 - l1);
                    var part3 = s.Substring(p2 + l2);
                    if (part1.Length > 0) {
                        Console.BackgroundColor = back;
                        Console.ForegroundColor = fore;
                        Write(part1);
                    }
                    if (part2.Length > 0) {
                        if (style.BackgroundColor != null) Console.BackgroundColor = (ConsoleColor)style.BackgroundColor;
                        if (style.ForegroundColor != null) Console.ForegroundColor = (ConsoleColor)style.ForegroundColor;
                        Write(part2);
                    }
                    if (part3.Length > 0) {
                        Console.BackgroundColor = back;
                        Console.ForegroundColor = fore;
                        Write(part3);
                    }
                    done = true;
                    break;
                }
            }
            if (!done) Console.Write(s);
        }

        public void WriteLine(string s = null, params object[] arg) {
            if (s == null) { Console.WriteLine(); return; }
            Write(s + Environment.NewLine, arg);
        }

        public void Wait(int ms = 2000) {
            if (ms > 0) Thread.Sleep(ms);
            else if (ms < 0) Console.ReadKey(true);
        }

        public void Beep() {
            Console.Beep(4000, 250); // BEEP..
        }

        public void Boop() {
            Console.Beep(); // BOOP...
        }

        public void BeBeBeep() {
            Console.Beep(4000, 125); // BE..
            Console.Beep(4000, 125); // BE..
            Console.Beep(4000, 125); // BEEP!
        }

        public void Boooop() {
            Console.Beep(500, 1000); // BOOOOP!
        }
    }
}