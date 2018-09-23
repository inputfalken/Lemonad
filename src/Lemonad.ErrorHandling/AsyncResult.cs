using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     An asynchronous version of <see cref="Result{T,TError}" /> with the same functionality.
    /// </summary>
    public readonly struct AsyncResult<T, TError> {
        private AsyncResult(Task<Result<T, TError>> result) =>
            TaskResult = result ?? throw new ArgumentNullException(nameof(result));

        internal Task<Result<T, TError>> TaskResult { get; }

        public static implicit operator AsyncResult<T, TError>(Task<Result<T, TError>> result) =>
            new AsyncResult<T, TError>(result);

        public static implicit operator AsyncResult<T, TError>(T value) =>
            new AsyncResult<T, TError>(Task.FromResult(ResultExtensions.Value<T, TError>(value)));

        public AsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            AsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            TaskResultFunctions.Join(TaskResult, inner.TaskResult, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector, comparer);

        public AsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            AsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            TaskResultFunctions.Join(TaskResult, inner.TaskResult, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector);

        [Pure]
        public AsyncResult<TResult, TError> Zip<TOther, TResult>(AsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) =>
            TaskResultFunctions.Zip(TaskResult, other.TaskResult, resultSelector);

        private static async Task<Result<T, TError>> Factory(Task<T> foo) => await foo.ConfigureAwait(false);
        private static async Task<Result<T, TError>> ErrorFactory(Task<TError> foo) => await foo.ConfigureAwait(false);

        public static implicit operator AsyncResult<T, TError>(Task<T> value) => Factory(value);

        public static implicit operator AsyncResult<T, TError>(Task<TError> error) => ErrorFactory(error);

        public static implicit operator AsyncResult<T, TError>(TError error) =>
            new AsyncResult<T, TError>(Task.FromResult(ResultExtensions.Error<T, TError>(error)));

        /// <inheritdoc cref="Result{T,TError}.Filter(System.Func{T,bool},System.Func{TError})" />
        public AsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) =>
            TaskResultFunctions.Filter(TaskResult, predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Filter(System.Func{T,bool},System.Func{Maybe{T},TError})" />
        public AsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<Maybe<T>, TError> errorSelector) =>
            TaskResultFunctions.Filter(TaskResult, predicate, errorSelector);

        public AsyncResult<T, TError> Filter(Func<T, Task<bool>> predicate, Func<TError> errorSelector) =>
            TaskResultFunctions.Filter(TaskResult, predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Filter(System.Func{T,bool},System.Func{Maybe{T},TError})" />
        public AsyncResult<T, TError> Filter(Func<T, Task<bool>> predicate, Func<Maybe<T>, TError> errorSelector) =>
            TaskResultFunctions.Filter(TaskResult, predicate, errorSelector);

        public Task<Either<T, TError>> Either => TaskResultFunctions.Either(TaskResult);

        /// <inheritdoc cref="Result{T,TError}.Multiple" />
        public AsyncResult<T, IReadOnlyList<TError>> Multiple(
            params Func<Result<T, TError>, Result<T, TError>>[] validations) =>
            TaskResultFunctions.Multiple(TaskResult, validations);

        /// <inheritdoc cref="Result{T,TError}.IsErrorWhen(Func{T,bool},Func{TError})" />
        public AsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(TaskResult, predicate, errorSelector);

        public AsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(TaskResult, predicate, errorSelector);

        public AsyncResult<T, TError> IsErrorWhen(
            Func<T, Task<bool>> predicate,
            Func<TError> errorSelector) => TaskResultFunctions.IsErrorWhen(TaskResult, predicate, errorSelector);

        public AsyncResult<T, TError> IsErrorWhen(
            Func<T, Task<bool>> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(TaskResult, predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.IsErrorWhenNull(System.Func{TError})" />
        public AsyncResult<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhenNull(TaskResult, errorSelector);

        public AsyncResult<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            TaskResultFunctions.Map(TaskResult, selector);

        public AsyncResult<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector) =>
            TaskResultFunctions.Map(TaskResult, selector);

        public AsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            TaskResultFunctions.MapError(TaskResult, selector);

        public AsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, Task<TErrorResult>> selector) =>
            TaskResultFunctions.MapError(TaskResult, selector);

        /// <inheritdoc cref="Result{T,TError}.Do" />
        public AsyncResult<T, TError> Do(Action action) => TaskResultFunctions.Do(TaskResult, action);

        /// <inheritdoc cref="Result{T,TError}.DoWithError" />
        public AsyncResult<T, TError> DoWithError(Action<TError> action) =>
            TaskResultFunctions.DoWithError(TaskResult, action);

        /// <inheritdoc cref="Result{T,TError}.DoWith" />
        public AsyncResult<T, TError> DoWith(Action<T> action) => TaskResultFunctions.DoWith(TaskResult, action);

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}" />
        public AsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => TaskResultFunctions.FullMap(TaskResult, selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match{TResult}" />
        public Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            TaskResultFunctions.Match(TaskResult, selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match" />
        public Task Match(Action<T> action, Action<TError> errorAction) =>
            TaskResultFunctions.Match(TaskResult, action, errorAction);

        public AsyncResult<TResult, TError> FlatMap<TResult>(Func<T, AsyncResult<TResult, TError>> flatSelector) =>
            TaskResultFunctions.FlatMap(TaskResult, x => flatSelector(x).TaskResult);

        public AsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, AsyncResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(TaskResult, x => flatSelector(x).TaskResult, resultSelector);

        public AsyncResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, AsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(TaskResult, x => flatMapSelector(x).TaskResult, errorSelector);

        public AsyncResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, AsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(TaskResult, x => flatMapSelector(x).TaskResult, resultSelector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}" />
        public AsyncResult<TResult, TError> Cast<TResult>() => TaskResultFunctions.Cast<T, TResult, TError>(TaskResult);

        public AsyncResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, AsyncResult<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(TaskResult, x => selector(x).TaskResult, errorSelector);

        public AsyncResult<T, TError> Flatten<TResult>(Func<T, AsyncResult<TResult, TError>> selector) =>
            TaskResultFunctions.Flatten(TaskResult, x => selector(x).TaskResult);

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}" />
        public AsyncResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            TaskResultFunctions.FullCast<T, TResult, TError, TErrorResult>(TaskResult);

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult}" />
        public AsyncResult<TResult, TResult> FullCast<TResult>() => FullCast<TResult, TResult>();

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}" />
        public AsyncResult<T, TResult> CastError<TResult>() =>
            TaskResultFunctions.CastError<T, TError, TResult>(TaskResult);

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}" />
        public AsyncResult<TResult, TError> SafeCast<TResult>(Func<TError> errorSelector) =>
            TaskResultFunctions.SafeCast<T, TResult, TError>(TaskResult, errorSelector);

        public AsyncResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, AsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(TaskResult, x => flatMapSelector(x).TaskResult, resultSelector,
                errorSelector);

        public AsyncResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, AsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(TaskResult, x => flatMapSelector(x).TaskResult, errorSelector);
    }
}