using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions.AsyncMaybe;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Maybe.Task;
using Lemonad.ErrorHandling.Extensions.Result.Task;
using Xunit;

namespace Assertion {
    public static class AssertExtensions {
        public static IResult<T, TError> AssertError<T, TError>(this IResult<T, TError> source, TError expected) {
            Assert.False(source.Either.HasValue);
            Assert.True(source.Either.HasError);
            Assert.Equal(expected, source.Either.Error);
            Assert.Equal(default, source.Either.Value);
            return source;
        }

        public static IAsyncResult<T, TError> AssertError<T, TError>(
            this IAsyncResult<T, TError> source,
            TError expected
        ) {
            async Task<IResult<T, TError>> Resolve() {
                var result = await source;
                Assert.False(result.Either.HasValue);
                Assert.True(result.Either.HasError);
                Assert.Equal(expected, result.Either.Error);
                Assert.Equal(default, result.Either.Value);
                return result;
            }

            return Resolve().ToAsyncResult();
        }

        public static IResult<T, string> AssertErrorContains<T>(this IResult<T, string> source, string expected) {
            Assert.False(source.Either.HasValue);
            Assert.True(source.Either.HasError);
            Assert.Contains(expected, source.Either.Error);
            Assert.Equal(default, source.Either.Value);
            return source;
        }

        public static IMaybe<T> AssertNone<T>(this IMaybe<T> source) {
            Assert.False(source.HasValue);
            Assert.Equal(default, source.Value);
            return source;
        }

        public static IAsyncMaybe<T> AssertNone<T>(this IAsyncMaybe<T> source) {
            async Task<IMaybe<T>> Resolve() {
                var maybe = await source;
                Assert.False(await source.HasValue);
                return maybe;
            }

            return Resolve().ToAsyncMaybe();
        }

        public static IMaybe<T> AssertValue<T>(this IMaybe<T> source, T expected) {
            Assert.True(source.HasValue);
            Assert.Equal(expected, source.Value);
            return source;
        }

        public static IAsyncMaybe<T> AssertValue<T>(this IAsyncMaybe<T> source, T expected) {
            async Task<IMaybe<T>> Resolve() {
                var maybe = await source;
                Assert.True(await source.HasValue);
                Assert.Equal(expected, source.Value);
                return maybe;
            }

            return Resolve().ToAsyncMaybe();
        }

        public static IResult<T, TError> AssertValue<T, TError>(this IResult<T, TError> source, T expected) {
            Assert.Equal(default, source.Either.Error);
            Assert.Equal(expected, source.Either.Value);
            Assert.True(source.Either.HasValue);
            Assert.False(source.Either.HasError);
            return source;
        }

        public static IAsyncResult<T, TError> AssertValue<T, TError>(this IAsyncResult<T, TError> source, T expected) {
            async Task<IResult<T, TError>> Resolve() {
                var result = await source;
                Assert.True(result.Either.HasValue);
                Assert.False(result.Either.HasError);
                Assert.Equal(default, result.Either.Error);
                Assert.Equal(expected, result.Either.Value);
                return result;
            }

            return Resolve().ToAsyncResult();
        }
    }
}