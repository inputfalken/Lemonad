using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;

namespace ReadFileAsync {
    internal static partial class Program {
        private static async Task<int> Main(string[] args) {
            var result = await "data.txt"
                .ToResult(File.Exists, () => ExitCode.FileNotFound)
                .Filter(x => Path.GetExtension(x) == ".txt", () => ExitCode.InvalidFileExtension)
                .ToAsyncResult()
                .Map(s => File.ReadAllTextAsync(s))
                .Filter(s => s == "Hello World", () => ExitCode.InvalidFileContent)
                .FlatMap(s => ProcessText(s, "processed.txt").ToAsyncResult())
                .Match(x => (ExitCode: 0, Message: x),
                    x => {
                        string message;
                        switch (x) {
                            case ExitCode.FileNotFound:
                                message = "File not found.";
                                break;
                            case ExitCode.InvalidFileContent:
                                message = "Invalid file content.";
                                break;
                            case ExitCode.InvalidFileExtension:
                                message = "Invalid file extension.";
                                break;
                            case ExitCode.FailedWritingText:
                                message = "Could not write to file, try running again.";
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(x), x, null);
                        }

                        return (ExitCode: (int) x, Message: message);
                    });

            Console.WriteLine(result.Message);
            return result.ExitCode;
        }

        private static void LogFatal(string message, Exception exception) {
            // Log fatal somewhere...
        }

        private static async Task<Result<string, ExitCode>> ProcessText(
            string text, string filePath) {
            // You can also handle exceptions more effectivly with Result<T, TError>.
            try {
                await File.WriteAllTextAsync(filePath, text.ToUpper(CultureInfo.InvariantCulture));
                // Return a message indicating a success.
                return "Successfully precessed file.";
            }
            catch (Exception e) {
                LogFatal($"Could not write to file {filePath}.", e);
                // Return an error indicating a failure.
                return ExitCode.FailedWritingText;
            }
        }
    }
}