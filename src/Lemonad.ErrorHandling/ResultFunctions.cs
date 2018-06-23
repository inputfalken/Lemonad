using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public static class Result {
        public static IEnumerable<TError> Errors<T, TError>(
            this IEnumerable<Result<T, TError>> enumerable) => enumerable.SelectMany(x => x.AsErrorEnumerable);

        public static IEnumerable<T> Values<T, TError>(
            this IEnumerable<Result<T, TError>> enumerable) => enumerable.SelectMany(x => x.AsEnumerable);

        [Pure]
        public static Result<T, TError> Ok<T, TError>(T element) =>
            new Result<T, TError>(element, default(TError), false, true);

        [Pure]
        public static Result<T, TError> Error<T, TError>(TError error) =>
            new Result<T, TError>(default(T), error, true, false);

        [Pure]
        public static Result<T, TError>
            ToResult<T, TError>(this T? source, Func<TError> errorSelector) where T : struct =>
            source.HasValue
                ? Ok<T, TError>(source.Value)
                : (errorSelector != null
                    ? Error<T, TError>(errorSelector())
                    : throw new ArgumentNullException(nameof(errorSelector)));

        [Pure]
        public static Result<T, TError> ToResult<T, TError>(this T element) => Ok<T, TError>(element);

        [Pure]
        public static Result<T, TError> ToResultError<T, TError>(this TError error) => Error<T, TError>(error);

        [Pure]
        public static Maybe<T> ConvertToMaybe<T, TError>(this Result<TError, T> source) =>
            source.HasValue ? source.Error.Some() : Maybe<T>.None;
    }
}