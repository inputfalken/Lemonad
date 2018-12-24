﻿using System.Threading.Tasks;
using Lemonad.ErrorHandling;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Lemonad.ErrorHandling.Extensions.Result.Task;
using Xunit;

namespace Assertion {
    public static class AssertResult {
        public static IResult<T, TError> AssertValue<T, TError>(this IResult<T, TError> source, T expected) {
            Assert.True(source.Either.HasValue);
            Assert.False(source.Either.HasError);
            Assert.Equal(default, source.Either.Error);
            Assert.Equal(expected, source.Either.Value);
            return source;
        }

        public static IResult<T, TError> AssertError<T, TError>(this IResult<T, TError> source, TError expected) {
            Assert.False(source.Either.HasValue);
            Assert.True(source.Either.HasError);
            Assert.Equal(expected, source.Either.Error);
            Assert.Equal(default, source.Either.Value);
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
    }
}