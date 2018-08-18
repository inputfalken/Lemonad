using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions.Internal {
    // Todo make these methods internal, and expose outcome functions.
    internal static class TaskResultFunctions {
        [Pure]
        internal static async Task<Result<TResult, TError>> Map<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, TResult> selector) => (await source.ConfigureAwait(false)).Map(selector);

        [Pure]
        internal static async Task<Result<T, TErrorResult>> MapError<T, TError, TErrorResult>(Task<Result<T, TError>> source, Func<TError, TErrorResult> selector) =>
            (await source.ConfigureAwait(false)).MapError(selector);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullMap<T, TResult, TError, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => (await source.ConfigureAwait(false)).FullMap(selector, errorSelector);

        [Pure]
        internal static async Task<TResult> Match<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) =>
            (await source.ConfigureAwait(false)).Match(selector, errorSelector);

        internal static async Task Match<T, TError>(Task<Result<T, TError>> source,
            Action<T> action,
            Action<TError> errorAction) =>
            (await source.ConfigureAwait(false)).Match(action, errorAction);

        internal static async Task<Result<T, TError>> Do<T, TError>(Task<Result<T, TError>> source, Action action) =>
            (await source.ConfigureAwait(false)).Do(action);

        internal static async Task<Result<T, TError>> DoWithError<T, TError>(Task<Result<T, TError>> source,
            Action<TError> action) => (await source.ConfigureAwait(false)).DoWithError(action);

        internal static async Task<Result<T, TError>> DoWith<T, TError>(Task<Result<T, TError>> source,
            Action<T> action) =>
            (await source.ConfigureAwait(false)).DoWith(action);

        [Pure]
        internal static async Task<Result<T, TError>> Filter<T, TError>(Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Filter(predicate, errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> IsErrorWhen<T, TError>(Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhen(predicate, errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> IsErrorWhenNull<T, TError>(Task<Result<T, TError>> source,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhenNull(errorSelector);

        [Pure]
        internal static async Task<Result<T, TErrorResult>> CastError<T, TError, TErrorResult>
            (Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).CastError<TErrorResult>();

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullCast<T, TResult, TError, TErrorResult>(Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).FullCast<TResult, TErrorResult>();

        [Pure]
        internal static async Task<Result<TResult, TError>> Cast<T, TResult, TError>(Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).Cast<TResult>();

        [Pure]
        internal static async Task<Result<TResult, TError>> SafeCast<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<TError> errorSelector)
            => (await source.ConfigureAwait(false)).SafeCast<TResult>(errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(Task<Result<T, TError>> source, Func<T, Result<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Flatten(selector, errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TError, TResult>(Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> selector) => (await source.ConfigureAwait(false)).Flatten(selector);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(Task<Result<T, TError>> source, Func<T, Task<Result<TResult, TErrorResult>>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).FlattenInternal(selector, errorSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TError, TResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> selector) =>
            await (await source.ConfigureAwait(false)).FlattenInternal(selector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TResult, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) => await (await source.ConfigureAwait(false))
            .FullFlatMapInternal(flatMapSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TFlatMap, TResult, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) => await (await source.ConfigureAwait(false))
            .FullFlatMapInternal(flatMapSelector, resultSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TResult, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) => (await source.ConfigureAwait(false))
            .FullFlatMap(flatMapSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TFlatMap, TResult, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            (await source.ConfigureAwait(false)).FullFlatMap(flatMapSelector, resultSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> flatSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            await (await source.ConfigureAwait(false)).FlatMapInternal(flatSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TSelector, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatSelector, resultSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TSelector, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            await (await source.ConfigureAwait(false)).FlatMapInternal(flatSelector, resultSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).FlatMapInternal(flatMapSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, resultSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).FlatMapInternal(flatMapSelector, resultSelector, errorSelector)
                .ConfigureAwait(false);
    }
}