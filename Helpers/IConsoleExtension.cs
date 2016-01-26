namespace CodeDog.System.ConsoleExtensions {

    public interface IConsoleExtension {

        void Write(string s, params object[] arg);

        void WriteLine(string s, params object[] arg);

    }

}