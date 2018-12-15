using System;
using System.Linq;
using Lemonad.ErrorHandling;

namespace ImplicitResult {
    internal class Program {
        public static int Main(string[] args) {
            return Result.Value<string[], int>(args)
                .Do(() => Console.WriteLine("Input letters only."))
                .Map(x => Console.ReadLine())
                .FlatMap(Service.LetterOnly)
                .DoWith(s => Console.WriteLine($"Input numbers only for '{s}'"))
                .FlatMap(_ => Service.NumberOnly(Console.ReadLine()), (x, y) => (TextOnly: x, NumberOnly: y))
                .DoWith(x => {
                    Console.WriteLine($"Only text '{x.TextOnly}'.");
                    Console.WriteLine($"Only numbers '{x.NumberOnly}'.");
                })
                .Match(_ => 0, x => x);
        }
    }

    public static class Service {
        // Create Result<T, TError> implicitly
        public static Result<string, int> LetterOnly(string input) {
            if (input.All(char.IsLetter))
                return input;
            return 2;
        }

        // Create Result<T, TError> implicitly
        public static Result<string, int> NumberOnly(string input) {
            if (input.All(char.IsNumber))
                return input;
            return 1;
        }
    }
}