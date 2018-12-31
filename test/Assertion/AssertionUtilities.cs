using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Result;

namespace Assertion {
    public static class AssertionUtilities {
        private static Task Delay => Task.Delay(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? 200 : 50);

        public static string EitherValueName { get; } = nameof(IEither<object, object>.Value);
        public static string EitherErrorName { get; } = nameof(IEither<object, object>.Error);
        public static string MaybeValueName { get; } = nameof(IMaybe<object>.Value);

        public const string PredicateName = "predicate";

        public const string ExtensionParameterName = "source";
        public const string MaybeValueSelector = "valueSelector";
        public const string MaybeNoneSelector = "noneSelector";
        public const string SelectorName = "selector";

        public static IMaybe<int> DivisionMaybe(int left, int right) =>
            right != 0 ? Maybe.Value(left / right) : Maybe.None<int>();

        public static IResult<double, string> Division(double left, double right) => (left, right).ToResult(
                x => right != 0,
                x => $"Can not divide '{x.left}' with '{x.right}'."
            )
            .Map(x => x.left / x.right);

        public static string FormatStringParserMessage<T>(string input) =>
            input is null
                ? $"Could not parse type {typeof(string).Name} (null) into {typeof(T).Name}."
                : $"Could not parse type {typeof(string).Name} (\"{input}\") into {typeof(T).Name}.";

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
            return Result.Value<int, string>(identity)
                .Map(i => {
                    switch (identity) {
                        case 0:
                            return Result.Value<Gender, string>(Gender.Male);
                        case 1:
                            return Result.Value<Gender, string>(Gender.Female);
                        default:
                            return Result.Error<Gender, string>("Could not determine gender.");
                    }
                }).FlatMap(x => x);
        }

        public static IAsyncResult<Gender, string> GetGenderAsync(int identity) {
            return Result.Value<int, string>(identity)
                .ToAsyncResult()
                .MapAsync(async i => {
                    await Delay;
                    switch (identity) {
                        case 0:
                            return Result.Value<Gender, string>(Gender.Male);
                        case 1:
                            return Result.Value<Gender, string>(Gender.Female);
                        default:
                            return Result.Error<Gender, string>("Could not determine gender");
                    }
                }).FlatMapAsync(x => x.ToAsyncResult());
        }

        public static IResult<string, ExitCodes> Program(int code) {
            return Result.Value<int, ExitCodes>(code)
                .Map(i => {
                    switch (code) {
                        case 0:
                            return Result.Value<string, ExitCodes>("Success");
                        case 1:
                            return Result.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return Result.Error<string, ExitCodes>(ExitCodes.Unhandled);
                    }
                }).FlatMap(x => x);
        }

        public static IAsyncResult<string, ExitCodes> ProgramAsync(int code) {
            return Result.Value<int, ExitCodes>(code)
                .ToAsyncResult()
                .MapAsync(async i => {
                    await Delay;
                    switch (code) {
                        case 0:
                            return Result.Value<string, ExitCodes>("Success");
                        case 1:
                            return Result.Error<string, ExitCodes>(ExitCodes.Fail);
                        default:
                            return Result.Error<string, ExitCodes>(ExitCodes.Unhandled);
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