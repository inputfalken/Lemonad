namespace Lemonad.ErrorHandling.Extensions.String {
    public static partial class ResultString {
        public static IResult<string, IsNullOrWhiteSpaceCase> IsNullOrWhiteSpace(this string value) {
            if (value is null)
                return ErrorHandling.Result.Error<string, IsNullOrWhiteSpaceCase>(IsNullOrWhiteSpaceCase.Null);
            foreach (var character in value) {
                if (!char.IsWhiteSpace(character))
                    return ErrorHandling.Result.Error<string, IsNullOrWhiteSpaceCase>(IsNullOrWhiteSpaceCase
                        .WhiteSpace);
            }

            return ErrorHandling.Result.Value<string, IsNullOrWhiteSpaceCase>(value);
        }
    }

    public enum IsNullOrWhiteSpaceCase {
        Null,
        WhiteSpace
    }

}