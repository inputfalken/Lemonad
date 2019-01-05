using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Result;

namespace Assertion {
    public static class AssertionUtilities {
        public enum Gender {
            Male = 0,
            Female = 1
        }

        public enum Error {
            First = 0,
            Second = 1
        }

        public const string ActionParamName = "action";
        public const string ErrorActionParamName = "errorAction";
        public const string ErrorParamName = "error";
        public const string ErrorSelectorName = "errorSelector";
        public const string ExtensionParameterName = "source";
        public const string JoinCompareParameter = "comparer";
        public const string JoinInnerKeyParameter = "innerKeySelector";

        public const string JoinInnerParameter = "inner";
        public const string JoinOuterKeyParameter = "outerKeySelector";
        public const string MaybeNoneSelector = "noneSelector";
        public const string MaybeValueSelector = "valueSelector";
        public const string PredicateName = "predicate";
        public const string ResultSelector = "resultSelector";
        public const string SelectorName = "selector";
        public const string ValueParamName = "value";
        public const string ValueSelectorName = "valueSelector";

        public const string ZipOtherParameter = "other";
        public static Task Delay => Task.Delay(200);

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

        public static IMaybe<int> DivisionMaybe(int left, int right) =>
            right != 0 ? Maybe.Value(left / right) : Maybe.None<int>();

        public static string FormatStringParserMessage<T>(string input) =>
            input is null
                ? $"Could not parse type {typeof(string).Name} (null) into {typeof(T).Name}."
                : $"Could not parse type {typeof(string).Name} (\"{input}\") into {typeof(T).Name}.";

        public static IAsyncResult<Gender, string> GetGenderAsync(int identity) => GetGender(identity).ToAsyncResult();

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

    }
}