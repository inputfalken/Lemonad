using System;
using System.Linq;
using Lemonad.ErrorHandling;

namespace ConsoleInputValidation {
    internal static class Program {
        private static int Main(string[] args) {
            Console.WriteLine("Please supply your name.");
            return Console.ReadLine()
                .ToResult(s => string.IsNullOrEmpty(s) == false, x => ExitCode.EmptyName)
                .Flatten(OnlyAlphanumericLetters)
                .Map(_ => ExitCode.Success)
                .FullCast<int>()
                .DoWithError(x => Console.WriteLine($"Bad input, exiting with code: {x}"))
                .DoWith(x => Console.WriteLine($"Good input, exiting with code: {x}"))
                .Match();
        }

        private static Result<string, ExitCode> OnlyAlphanumericLetters(string input) {
            if (input.All(c => char.IsLetter(c) || char.IsNumber(c)))
                return input;
            return ExitCode.NameContainsNoneAlphaNumericChars;
        }

        private enum ExitCode {
            Success = 0,
            EmptyName = 1,
            NameContainsNoneAlphaNumericChars = 2
        }
    }
}