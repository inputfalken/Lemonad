using System.Linq;

namespace Lemonad.ErrorHandling.Extensions.String {
    public static partial class ResultString {
        public static IResult<string, IsNullOrWhiteSpaceCase> IsNullOrWhiteSpace(this string value) {
            if (value is null)
                return ErrorHandling.Result.Error<string, IsNullOrWhiteSpaceCase>(IsNullOrWhiteSpaceCase.Null);
            if (value == string.Empty)
                return ErrorHandling.Result.Error<string, IsNullOrWhiteSpaceCase>(IsNullOrWhiteSpaceCase.Empty);

            return value.Any(char.IsWhiteSpace)
                ? ErrorHandling.Result.Error<string, IsNullOrWhiteSpaceCase>(IsNullOrWhiteSpaceCase.WhiteSpace)
                : ErrorHandling.Result.Value<string, IsNullOrWhiteSpaceCase>(value);
        }
    }

    public enum IsNullOrWhiteSpaceCase {
        Null,
        WhiteSpace,
        Empty
    }
}