using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions {
    public static class AsynchronousResultExtensions {
        /// <inheritdoc cref="Result{T,TError}.Map{TResult}"/>
        [Pure]
        public static async Task<Result<TResult, TError>> Map<T, TResult, TError>(this Task<Result<T, TError>> source,
            Func<T, TResult> selector) => (await source.ConfigureAwait(false)).Map(selector);

        /// <inheritdoc cref="Result{T,TError}.MapError{TErrorResult}"/>
        [Pure]
        public static async Task<Result<T, TErrorResult>> MapError<T, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<TError, TErrorResult> selector) =>
            (await source.ConfigureAwait(false)).MapError(selector);

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}"/>
        [Pure]
        public static async Task<Result<TResult, TErrorResult>> FullMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => (await source.ConfigureAwait(false)).FullMap(selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match{TResult}"/>
        [Pure]
        public static async Task<TResult> Match<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) =>
            (await source.ConfigureAwait(false)).Match(selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match"/>
        public static async Task Match<T, TError>(
            this Task<Result<T, TError>> source,
            Action<T> action,
            Action<TError> errorAction) =>
            (await source.ConfigureAwait(false)).Match(action, errorAction);

        /// <inheritdoc cref="Result{T,TError}.Do"/>
        public static async Task<Result<T, TError>> Do<T, TError>(this Task<Result<T, TError>> source, Action action) =>
            (await source.ConfigureAwait(false)).Do(action);

        /// <inheritdoc cref="Result{T,TError}.DoWithError"/>
        public static async Task<Result<T, TError>> DoWithError<T, TError>(
            this Task<Result<T, TError>> source,
            Action<TError> action) => (await source.ConfigureAwait(false)).DoWithError(action);

        /// <inheritdoc cref="Result{T,TError}.DoWith"/>
        public static async Task<Result<T, TError>> DoWith<T, TError>(
            this Task<Result<T, TError>> source,
            Action<T> action) =>
            (await source.ConfigureAwait(false)).DoWith(action);

        /// <inheritdoc cref="Result{T,TError}.Filter"/>
        [Pure]
        public static async Task<Result<T, TError>> Filter<T, TError>(
            this Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Filter(predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.IsErrorWhen"/>
        [Pure]
        public static async Task<Result<T, TError>> IsErrorWhen<T, TError>(
            this Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhen(predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.IsErrorWhenNull"/>
        [Pure]
        public static async Task<Result<T, TError>> IsErrorWhenNull<T, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhenNull(errorSelector);

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}"/>
        [Pure]
        internal static async Task<Result<T, TErrorResult>> CastError<T, TError, TErrorResult>
            (this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).CastError<TErrorResult>();

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}"/>
        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullCast<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).FullCast<TResult, TErrorResult>();

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}"/>
        [Pure]
        internal static async Task<Result<TResult, TError>> Cast<T, TResult, TError>(
            this Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).Cast<TResult>();

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}"/>
        [Pure]
        internal static async Task<Result<TResult, TError>> SafeCast<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector)
            => (await source.ConfigureAwait(false)).SafeCast<TResult>(errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Flatten{TResult,TErrorResult}(System.Func{T,Lemonad.ErrorHandling.Result{TResult,TErrorResult}},System.Func{TErrorResult,TError})"/>
        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<T, Result<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Flatten(selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Flatten{TResult}(System.Func{T,Lemonad.ErrorHandling.Result{TResult,TError}})"/>
        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TError, TResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> selector) => (await source.ConfigureAwait(false)).Flatten(selector);

        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<T, Task<Result<TResult, TErrorResult>>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            await (await source.ConfigureAwait(false)).Flatten(selector, errorSelector);

        [Pure]
        public static async Task<Result<T, TError>> Flatten<T, TError, TResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> selector) =>
            await (await source.ConfigureAwait(false)).Flatten(selector);
    }
}