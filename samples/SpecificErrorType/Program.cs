using System;
using System.IO;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions.AsyncResult;

namespace SpecificErrorType {
    internal static class Program {
        private static async Task Main(string[] args) => await ReadFile(Console.ReadLine()).DoWith(Console.WriteLine);

        private static IAsyncResult<string, ErrorModel> ReadFile(string input) {
            return new ResultModel<string>(input)
                .IsErrorWhen(
                    string.IsNullOrWhiteSpace,
                    s => new ErrorModel {Message = "String cannot be null or empty", Code = 2}
                )
                .MapAsync(s => File.ReadAllTextAsync(s));
        }
    }
}