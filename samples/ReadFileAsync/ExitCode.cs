namespace ReadFileAsync {
    internal static partial class Program {
        internal enum ExitCode {
            FileNotFound = 1,
            InvalidFileContent = 2,
            InvalidFileExtension = 3,
            FailedWritingText = 4
        }
    }
}