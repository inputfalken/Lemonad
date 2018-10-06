using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Either;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     An asynchronous version of <see cref="Result{T,TError}" /> with the same functionality.
    /// </summary>
    public readonly struct AsyncResult<T, TError> {
        public AsyncResult(Task<IEither<T, TError>> either) => Either = either;

        internal Task<IEither<T, TError>> Either { get; }

        public static implicit operator AsyncResult<T, TError>(Task<Result<T, TError>> result) =>
            new AsyncResult<T, TError>(Factory(result));

        public static implicit operator AsyncResult<T, TError>(T value) =>
            new AsyncResult<T, TError>(Task.FromResult(ResultExtensions.Value<T, TError>(value).Either));

        private static async Task<IEither<T, TError>> Factory(Task<Result<T, TError>> value) =>
            (await value.ConfigureAwait(false)).Either;

        private static async Task<Result<T, TError>> Factory(Task<T> value) =>
            ResultExtensions.Value<T, TError>(await value.ConfigureAwait(false));

        private static async Task<Result<T, TError>> ErrorFactory(Task<TError> error) =>
            ResultExtensions.Error<T, TError>(await error.ConfigureAwait(false));

        public static implicit operator AsyncResult<T, TError>(Task<T> value) => Factory(value);

        public static implicit operator AsyncResult<T, TError>(Task<TError> error) => ErrorFactory(error);

        public static implicit operator AsyncResult<T, TError>(TError error) =>
            new AsyncResult<T, TError>(Task.FromResult(ResultExtensions.Error<T, TError>(error).Either));

        public AsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            AsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            new AsyncResult<TResult, TError>(EitherMethods.JoinAsync(Either, inner.Either, outerKeySelector,
                innerKeySelector, resultSelector,
                errorSelector, comparer));

        public AsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            AsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.JoinAsync(Either, inner.Either, outerKeySelector,
                innerKeySelector, resultSelector,
                errorSelector));

        [Pure]
        public AsyncResult<TResult, TError> Zip<TOther, TResult>(AsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.ZipAsync(Either, other.Either, resultSelector));

        public AsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector) =>
            new AsyncResult<T, TError>(EitherMethods.FilterAsync(Either, predicate, errorSelector));

        public AsyncResult<T, TError> Filter(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector) =>
            new AsyncResult<T, TError>(EitherMethods.FilterAsyncPredicate(Either, predicate, errorSelector));

        public AsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) =>
            new AsyncResult<T, TError>(EitherMethods.IsErrorWhenAsync(Either, predicate, errorSelector));

        public AsyncResult<T, TError> IsErrorWhen(
            Func<T, Task<bool>> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            new AsyncResult<T, TError>(
                EitherMethods.IsErrorWhenAsyncPredicate(Either, predicate, errorSelector));

        public AsyncResult<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            new AsyncResult<TResult, TError>(EitherMethods.MapAsync(Either, selector));

        public AsyncResult<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector) =>
            new AsyncResult<TResult, TError>(EitherMethods.MapAsyncSelector(Either, selector));

        public AsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            new AsyncResult<T, TErrorResult>(EitherMethods.MapErrorAsync(Either, selector));

        public AsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, Task<TErrorResult>> selector) =>
            new AsyncResult<T, TErrorResult>(EitherMethods.MapErrorAsyncSelector(Either, selector));

        /// <inheritdoc cref="Result{T,TError}.Do" />
        public AsyncResult<T, TError> Do(Action action) =>
            new AsyncResult<T, TError>(EitherMethods.DoAsync(Either, action));

        /// <inheritdoc cref="Result{T,TError}.DoWithError" />
        public AsyncResult<T, TError> DoWithError(Action<TError> action) =>
            new AsyncResult<T, TError>(EitherMethods.DoWithErrorAsync(Either, action));

        /// <inheritdoc cref="Result{T,TError}.DoWith" />
        public AsyncResult<T, TError> DoWith(Action<T> action) =>
            new AsyncResult<T, TError>(EitherMethods.DoWithAsync(Either, action));

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}" />
        public AsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => new AsyncResult<TResult, TErrorResult>(
            EitherMethods.FullMapAsync(Either, selector, errorSelector));

        /// <inheritdoc cref="Result{T,TError}.Match{TResult}" />
        public Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            EitherMethods.MatchAsync(Either, selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match" />
        public Task Match(Action<T> action, Action<TError> errorAction) =>
            EitherMethods.MatchAsync(Either, action, errorAction);

        public AsyncResult<TResult, TError> FlatMap<TResult>(Func<T, AsyncResult<TResult, TError>> flatSelector) =>
            new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(Either, flatSelector.Compose(y => y.Either)));

        public AsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, AsyncResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => new AsyncResult<TResult, TError>(
            EitherMethods.FlatMapAsync(Either, flatSelector.Compose(y => y.Either), resultSelector));

        public AsyncResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, AsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(Either,
                    flatMapSelector.Compose(x => x.Either),
                    errorSelector)
            );
        }

        public AsyncResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, AsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.FlatMapAsync(Either, flatMapSelector.Compose(y => y.Either),
                resultSelector, errorSelector));

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}" />
        public AsyncResult<TResult, TError> Cast<TResult>() =>
            new AsyncResult<TResult, TError>(EitherMethods.CastAsync<T, TResult, TError>(Either));

        public AsyncResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, AsyncResult<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            new AsyncResult<T, TError>(
                EitherMethods.FlattenAsync(Either, selector.Compose(y => y.Either), errorSelector));

        public AsyncResult<T, TError> Flatten<TResult>(Func<T, AsyncResult<TResult, TError>> selector) =>
            new AsyncResult<T, TError>(EitherMethods.FlattenAsync(Either, selector.Compose(y => y.Either)));

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}" />
        public AsyncResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullCastAsync<T, TResult, TError, TErrorResult>(Either));

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult}" />
        public AsyncResult<TResult, TResult> FullCast<TResult>() =>
            new AsyncResult<TResult, TResult>(EitherMethods.FullCastAsync<T, TResult, TError>(Either));

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}" />
        public AsyncResult<T, TResult> CastError<TResult>() =>
            new AsyncResult<T, TResult>(EitherMethods.CastErrorAsync<T, TResult, TError>(Either));

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}" />
        public AsyncResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.SafeCastAsync<T, TResult, TError>(Either, errorSelector));

        public AsyncResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, AsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            new AsyncResult<TResult, TErrorResult>(EitherMethods.FullFlatMapAsync(Either,
                flatMapSelector.Compose(y => y.Either), resultSelector, errorSelector));

        public AsyncResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, AsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullFlatMapAsync(Either, flatMapSelector.Compose(y => y.Either),
                    errorSelector));
    }
}