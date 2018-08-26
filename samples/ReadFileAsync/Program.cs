using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;

namespace ReadFileAsync {
    internal class Program {
        private static int Main(string[] args) {
            return args[0]
                .ToResult<string, int>()
                .Filter(File.Exists, () => 1)
                .DoWithError(x => Console.WriteLine("File does not exist."))
                .FlatMap(VerifyFilextension, i => {
                    Console.WriteLine("Invalid file extension.");
                    return i;
                })
                .FlatMap(Readfiles)
                .FlatMap(VerifyFileContent, i => {
                    Console.WriteLine("File cannot be empty");
                    return i;
                })
                .DoWith(x => Console.WriteLine(x.Aggregate((s, s1) => $"{s}{Environment.NewLine}{s1}")))
                .Match(_ => 0, i => i)
                .Result;
        }

        private static Outcome<string[], int> Readfiles(string filename) => File.ReadAllLinesAsync(filename);

        private static Result<IReadOnlyList<string>, int> VerifyFileContent(IReadOnlyList<string> lines) =>
            lines.Count > 0 ? ResultExtensions.Ok<IReadOnlyList<string>, int>(lines) : 3;

        private static Result<string, int> VerifyFilextension(string filename) =>
            Path.GetExtension(filename) == ".txt" ? (Result<string, int>) filename : 2;
    }
}