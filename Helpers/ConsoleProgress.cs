using System;

namespace CodeDog.Core.ConsoleExtensions {

    /// <summary>
    /// Allows displaying of dynamic console dot bar when other threads still output data to the console
    /// </summary>
    public class ConsoleProgress {

        /// <summary>
        /// Backup of console state
        /// </summary>
        private ConsoleState B;

        /// <summary>
        /// Optional console extension class
        /// </summary>
        private IConsoleExtension Ex;

        /// <summary>
        /// Current console state
        /// </summary>
        private ConsoleState C {
            get {
                return new ConsoleState();
            }
            set {
                ConsoleState.Set(value);
            }
        }

        /// <summary>
        /// Last console state
        /// </summary>
        private ConsoleState L;

        /// <summary>
        /// Creates console state dot bar at current cursor position
        /// </summary>
        public ConsoleProgress(IConsoleExtension extension = null) {
            Ex = extension;
            L = C;
            Console.WriteLine();
        }

        /// <summary>
        /// Displays subsequent progress dot
        /// </summary>
        public void Dot() {
            lock (ConsoleState.Lock) {
                B = C;
                C = L;
                Console.Write('.');
                L = C;
                C = B;
            }
        }

        /// <summary>
        /// Ends current dot bar with message or "OK." if no message provided
        /// </summary>
        /// <param name="msg"></param>
        public void Done(string msg) {
            lock (ConsoleState.Lock) {
                B = C;
                C = L;
                if (Ex == null) Console.WriteLine(msg); else Ex.WriteLine(msg);
                L = C;
                C = B;
            }
        }

    }

}