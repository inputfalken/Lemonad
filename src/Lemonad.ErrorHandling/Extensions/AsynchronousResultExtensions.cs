using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions {
    public static class AsynchronousResultExtensions {
        [Pure]
        public static async Task<Result<TResult, TError>> Map<T, TResult, TError>(this Task<Result<T, TError>> source,
            Func<T, TResult> selector) => (await source.ConfigureAwait(false)).Map(selector);

        [Pure]
        public static async Task<Result<T, TErrorResult>> MapError<T, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<TError, TErrorResult> selector) =>
            (await source.ConfigureAwait(false)).MapError(selector);

        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => (await source.ConfigureAwait(false)).FullMap(selector, errorSelector);

        [Pure]
        public static async Task<TResult> Match<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) =>
            (await source.ConfigureAwait(false)).Match(selector, errorSelector);

        public static async Task Match<T, TError>(
            this Task<Result<T, TError>> source,
            Action<T> action,
            Action<TError> errorAction) =>
            (await source.ConfigureAwait(false)).Match(action, errorAction);

        public static async Task<Result<T, TError>> Do<T, TError>(this Task<Result<T, TError>> source, Action action) =>
            (await source.ConfigureAwait(false)).Do(action);

        public static async Task<Result<T, TError>> DoWithError<T, TError>(
            this Task<Result<T, TError>> source,
            Action<TError> action) => (await source.ConfigureAwait(false)).DoWithError(action);

        public static async Task<Result<T, TError>> DoWith<T, TError>(
            this Task<Result<T, TError>> source,
            Action<T> action) =>
            (await source.ConfigureAwait(false)).DoWith(action);

        [Pure]
        public static async Task<Result<T, TError>> Filter<T, TError>(
            this Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Filter(predicate, errorSelector);

        [Pure]
        public static async Task<Result<T, TError>> IsErrorWhen<T, TError>(
            this Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhen(predicate, errorSelector);

        [Pure]
        public static async Task<Result<T, TError>> IsErrorWhenNull<T, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhenNull(errorSelector);

        [Pure]
        internal static async Task<Result<T, TErrorResult>> CastError<T, TError, TErrorResult>
            (this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).CastError<TErrorResult>();

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullCast<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).FullCast<TResult, TErrorResult>();

        [Pure]
        internal static async Task<Result<TResult, TError>> Cast<T, TResult, TError>(
            this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).Cast<TResult>();

        [Pure]
        internal static async Task<Result<TResult, TError>> SafeCast<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector)
            => (await source.ConfigureAwait(false)).SafeCast<TResult>(errorSelector);

        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<T, Result<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Flatten(selector, errorSelector);

        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TError, TResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> selector) => (await source.ConfigureAwait(false)).Flatten(selector);

        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<T, Task<Result<TResult, TErrorResult>>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).Flatten(selector, errorSelector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TError, TResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> selector) =>
            await (await source.ConfigureAwait(false)).Flatten(selector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) => await (await source.ConfigureAwait(false))
            .FullFlatMap(flatMapSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) => await (await source.ConfigureAwait(false))
            .FullFlatMap(flatMapSelector, resultSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) => (await source.ConfigureAwait(false))
            .FullFlatMap(flatMapSelector, errorSelector);

        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            (await source.ConfigureAwait(false)).FullFlatMap(flatMapSelector, resultSelector, errorSelector);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> flatSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatSelector);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            await (await source.ConfigureAwait(false)).FlatMap(flatSelector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TSelector, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatSelector, resultSelector);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TSelector, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            await (await source.ConfigureAwait(false)).FlatMap(flatSelector, resultSelector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, errorSelector);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, resultSelector, errorSelector);

        [Pure]
        public static async Task<Result<TResult, TError>> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, resultSelector, errorSelector)
                .ConfigureAwait(false);
    }
}