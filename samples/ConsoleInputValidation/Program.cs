using System;
using System.Linq;
using Lemonad.ErrorHandling;

namespace ConsoleInputValidation {
    internal static class Program {
        private static int Main(string[] args) {
            Console.WriteLine("Please supply your name.");
            return Console.ReadLine()
                .ToResult<string, ExitCode>()
                .IsErrorWhen(string.IsNullOrWhiteSpace, () => ExitCode.EmptyName)
                .Flatten(OnlyContainsAlphaNumericChars)
                .FullMap(_ => ExitCode.Success, code => (int) code)
                .Cast<int>()
                .DoWithError(x => Console.WriteLine($"Bad input, exiting with code: {x}"))
                .DoWith(x => Console.WriteLine($"Good input, exitiing with code: {x}"))
                .Match(x => x, x => x);
        }

        private static Result<string, ExitCode> OnlyContainsAlphaNumericChars(string input) {
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