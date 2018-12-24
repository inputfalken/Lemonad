using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Internal;

namespace Assertion {
    public static class AssertionUtilities {
        private static Task Delay => Task.Delay(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 200 : 50);

        public static string EitherValueName { get; } = nameof(IEither<object, object>.Value);
        public static string EitherErrorName { get; } = nameof(IEither<object, object>.Error);
        public static string MaybeValueName { get; } = nameof(Maybe<object>.Value);

        public static IResult<double, string> Division(double left, double right) => (left, right).ToResult(
                x => right != 0,
                x => $"Can not divide '{x.left}' with '{x.right}'."
            )
            .Map(x => x.left / x.right);

        public static IAsyncResult<double, string> DivisionAsync(double left, double right) {
            return (left, right).ToResult(
                    x => right != 0,
                    x => $"Can not divide '{x.left}' with '{x.right}'."
                )
                .ToAsyncResult()
                .MapAsync(async x => {
                    await Delay;
                    return x.left / x.right;
                });
        }

        public static IResult<Gender, string> GetGender(int identity) {
            return Lemonad.ErrorHandling.Result.Value<int, string>(identity)
                .Map(i => {
                    switch (identity) {
                        case 0:
                            return Lemonad.ErrorHandling.Result.Value<Gender, string>(Gender.Male);
                        case 1:
                            return Lemonad.ErrorHandling.Result.Value<Gender, string>(Gender.Female);
                        default:
                            return Lemonad.ErrorHandling.Result.Error<Gender, string>("Could not determine gender.");
                    }
                }).FlatMap(x => x);
        }

        public static IAsyncResult<Gender, string> GetGenderAsync(int identity) {
            return Lemonad.ErrorHandling.Result.Value<int, string>(identity)
                .ToAsyncResult()
                .MapAsync(async i => {
                    await Delay;
                    switch (identity) {
                        case 0:
                            return Lemonad.ErrorHandling.Result.Value<Gender, string>(Gender.Male);
                        case 1:
                            return Lemonad.ErrorHandling.Result.Value<Gender, string>(Gender.Female);
                        default:
                            return Lemonad.ErrorHandling.Result.Error<Gender, string>("Could not determine gender");
                    }
                }).FlatMapAsync(x => x.ToAsyncResult());
        }

        public static IResult<string, ExitCodes> Program(int code) {
            return Lemonad.ErrorHandling.Result.Value<int, ExitCodes>(code)
                .Map(i => {
                    switch (code) {
                        case 0:
                            return Lemonad.ErrorHandling.Result.Value<string, ExitCodes>("Success");
                        case 1:
                            return Lemonad.ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return Lemonad.ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Unhandled);
                    }
                }).FlatMap(x => x);
        }

        public static IAsyncResult<string, ExitCodes> ProgramAsync(int code) {
            return Lemonad.ErrorHandling.Result.Value<int, ExitCodes>(code)
                .ToAsyncResult()
                .MapAsync(async i => {
                    await Delay;
                    switch (code) {
                        case 0:
                            return Lemonad.ErrorHandling.Result.Value<string, ExitCodes>("Success");
                        case 1:
                            return Lemonad.ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return Lemonad.ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Unhandled);
                    }
                }).FlatMapAsync(x => x.ToAsyncResult());
        }

        public enum ExitCodes {
            Fail = 1,
            Unhandled
        }

        public enum Gender {
            Male = 0,
            Female = 1
        }
    }
}