using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.DataTypes.Result.Extensions {
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
            (await source.ConfigureAwait(false)).Filter(predicate, errorSelector);

        [Pure]
        public static Task<Result<T, TError>> IsErrorWhenNull<T, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector) =>
            source.IsErrorWhen(EquailtyFunctions.IsNull, errorSelector);

        [Pure]
        public static async Task<Result<T, TResult>> CastError<T, TResult, TError>
            (this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).CastError<TResult>();

        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullCast<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).FullCast<TResult, TErrorResult>();

        [Pure]
        public static async Task<Result<TResult, TError>> Cast<T, TResult, TError>(
            this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).Cast<TResult>();

        [Pure]
        public static async Task<Result<TResult, TError>> SafeCast<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector)
            => (await source.ConfigureAwait(false)).SafeCast<TResult>(errorSelector);
    }
}