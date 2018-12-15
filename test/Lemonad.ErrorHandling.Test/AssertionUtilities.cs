using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Test {
    internal static class AssertionUtilities {
        private static Task Delay => Task.Delay(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 200 : 50);

        public static string EitherValueName { get; } = nameof(IEither<object, object>.Value);
        public static string EitherErrorName { get; } = nameof(IEither<object, object>.Error);
        public static string MaybeValueName { get; } = nameof(Maybe<object>.Value);

        internal static IResult<double, string> Division(double left, double right) => (left, right).ToResult(
                x => right != 0,
                x => $"Can not divide '{x.left}' with '{x.right}'."
            )
            .Map(x => x.left / x.right);

        internal static IAsyncResult<double, string> DivisionAsync(double left, double right) {
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

        internal static IResult<Gender, string> GetGender(int identity) {
            return ErrorHandling.Result.Value<int, string>(identity)
                .Map(i => {
                    switch (identity) {
                        case 0:
                            return ErrorHandling.Result.Value<Gender, string>(Gender.Male);
                        case 1:
                            return ErrorHandling.Result.Value<Gender, string>(Gender.Female);
                        default:
                            return ErrorHandling.Result.Error<Gender, string>("Could not determine gender.");
                    }
                }).FlatMap(x => x);
        }

        internal static IAsyncResult<Gender, string> GetGenderAsync(int identity) {
            return ErrorHandling.Result.Value<int, string>(identity)
                .ToAsyncResult()
                .MapAsync(async i => {
                    await Delay;
                    switch (identity) {
                        case 0:
                            return ErrorHandling.Result.Value<Gender, string>(Gender.Male);
                        case 1:
                            return ErrorHandling.Result.Value<Gender, string>(Gender.Female);
                        default:
                            return ErrorHandling.Result.Error<Gender, string>("Could not determine gender");
                    }
                }).FlatMapAsync(x => x.ToAsyncResult());
        }

        internal static IResult<string, ExitCodes> Program(int code) {
            return ErrorHandling.Result.Value<int, ExitCodes>(code)
                .Map(i => {
                    switch (code) {
                        case 0:
                            return ErrorHandling.Result.Value<string, ExitCodes>("Success");
                        case 1:
                            return ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Unhandled);
                    }
                }).FlatMap(x => x);
        }

        internal static IAsyncResult<string, ExitCodes> ProgramAsync(int code) {
            return ErrorHandling.Result.Value<int, ExitCodes>(code)
                .ToAsyncResult()
                .MapAsync(async i => {
                    await Delay;
                    switch (code) {
                        case 0:
                            return ErrorHandling.Result.Value<string, ExitCodes>("Success");
                        case 1:
                            return ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return ErrorHandling.Result.Error<string, ExitCodes>(ExitCodes.Unhandled);
                    }
                }).FlatMapAsync(x => x.ToAsyncResult());
        }

        internal enum ExitCodes {
            Fail = 1,
            Unhandled
        }

        internal enum Gender {
            Male = 0,
            Female = 1
        }
    }
}