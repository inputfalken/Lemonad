using Xunit;

namespace Lemonad.ErrorHandling.Unit {
    internal static class AssertResult {
        public static void AssertValue<T, TError>(this IResult<T, TError> result, T expected) {
            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(default, result.Either.Error);
            Assert.Equal(expected, result.Either.Value);
        }

        public static void AssertError<T, TError>(this IResult<T, TError> result, TError expected) {
            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal(expected, result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        }
    }
}