﻿using System;
using System.Linq;
using Lemonad.ErrorHandling;

namespace ConsoleInputValidation {
    internal static class Program {
        private static IResult<int, string> Divide(int left, int right) {
            return (left: left, right: right)
                .ToResult(x => x.right == 0, x => $"Cannot divide '{x.left}' with '{x.right}'.")
                .Map(x => x.left / x.right);
        }

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

        private static IResult<string, ExitCode> OnlyAlphanumericLetters(string input) => input.ToResult(
            x => x.All(c => char.IsLetter(c) || char.IsNumber(c)),
            x => ExitCode.NameContainsNoneAlphaNumericChars);

        private enum ExitCode {
            Success = 0,
            EmptyName = 1,
            NameContainsNoneAlphaNumericChars = 2
        }
    }
}