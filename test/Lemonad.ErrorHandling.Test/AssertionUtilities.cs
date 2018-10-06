using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Test {
    internal static class AssertionUtilities {
        private static Task Delay => Task.Delay(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 200 : 50);

        public static string EitherValueName { get; } = nameof(IEither<object, object>.Value);
        public static string EitherErrorName { get; } = nameof(IEither<object, object>.Error);

        internal static Result<double, string> Division(double left, double right) {
            if (right == 0)
                return $"Can not divide '{left}' with '{right}'.";
            return left / right;
        }

        internal static Result<string, ExitCodes> Program(int code) {
            switch (code) {
                case 0:
                    return "Success";
                case 1:
                    return ExitCodes.Fail;
                default:
                    return ExitCodes.Unhandled;
            }
        }
        internal static Result<Gender, string> GetGender(int identity) {
            switch (identity) {
                case 0:
                    return Gender.Male;
                case 1:
                    return Gender.Female;
                default:
                    return "Could not determine gender";
            }
        }

        internal static AsyncResult<double, string> DivisionAsync(double left, double right) {
            return (left, right).ToResult(
                    x => right != 0,
                    x => $"Can not divide '{x.Value.left}' with '{x.Value.right}'."
                )
                .ToAsyncResult()
                .Map(async x => {
                    await Delay;
                    return x.left / x.right;
                });
        }

        internal static AsyncResult<Gender, string> GetGenderAsync(int identity) {
            return ResultExtensions.Value<int, string>(identity)
                .ToAsyncResult()
                .Map(async i => {
                    await Delay;
                    switch (identity) {
                        case 0:
                            return ResultExtensions.Value<Gender, string>(Gender.Male);
                        case 1:
                            return ResultExtensions.Value<Gender, string>(Gender.Female);
                        default:
                            return ResultExtensions.Error<Gender, string>("Could not determine gender");
                    }
                }).FlatMap(x => x.ToAsyncResult());
        }

        internal static AsyncResult<string, ExitCodes> ProgramAsync(int code) {
            return ResultExtensions.Value<int, ExitCodes>(code)
                .ToAsyncResult()
                .Map(async i => {
                    await Delay;
                    switch (code) {
                        case 0:
                            return ResultExtensions.Value<string, ExitCodes>("Success");
                        case 1:
                            return ResultExtensions.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return ResultExtensions.Error<string, ExitCodes>(ExitCodes.Unhandled);
                    }
                }).FlatMap(x => x.ToAsyncResult());
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