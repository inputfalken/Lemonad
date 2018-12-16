namespace Lemonad.ErrorHandling.Extensions.String {
    public static partial class ResultString {
        public static IResult<string, IsNullOrEmptyCase> IsNullOrEmpty(this string value) {
            if (value is null)
                return ErrorHandling.Result.Error<string, IsNullOrEmptyCase>(IsNullOrEmptyCase.Null);

            return value.Length == 0
                ? ErrorHandling.Result.Error<string, IsNullOrEmptyCase>(IsNullOrEmptyCase.Empty)
                : ErrorHandling.Result.Value<string, IsNullOrEmptyCase>(value);
        }
    }

    public enum IsNullOrEmptyCase {
        Null,
        Empty
    }
}