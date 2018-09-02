using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;

namespace ReadFileAsync {
    internal class Program {
        private static int Main(string[] args) {
            var result = Task.FromResult("foo")
                .ToOutcome(x => true, () => "")
                .IsErrorWhen(x => Task.FromResult(false), () => "")
                .Match(x => x, x => x)
                .Result;

            Console.WriteLine(result);
            return 0;
        }

        private static Outcome<string[], int> Readfiles(string filename) => File.ReadAllLinesAsync(filename);

        private static Result<IReadOnlyList<string>, int> VerifyFileContent(IReadOnlyList<string> lines) =>
            lines.Count > 0 ? ResultExtensions.Ok<IReadOnlyList<string>, int>(lines) : 3;

        private static Result<string, int> VerifyFilextension(string filename) =>
            Path.GetExtension(filename) == ".txt" ? (Result<string, int>) filename : 2;
    }
}