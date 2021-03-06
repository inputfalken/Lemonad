﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Extensions.Task;
using Lemonad.ErrorHandling.Internal.Either;
using Index = Lemonad.ErrorHandling.Extensions.Result.Index;

namespace Lemonad.ErrorHandling.Internal {
    /// <summary>
    ///     An asynchronous version of <see cref="Result{T,TError}" /> with the same functionality.
    /// </summary>
    internal class AsyncResult<T, TError> : IAsyncResult<T, TError> {
        private AsyncResult(Task<IEither<T, TError>> either) => Either = either.ToAsyncEither();

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}" />
        public IAsyncResult<TResult, TError> Cast<TResult>()
            => new AsyncResult<TResult, TError>(EitherMethods.CastAsync<T, TResult, TError>(Either.ToTaskEither()));

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}" />
        public IAsyncResult<T, TResult> CastError<TResult>()
            => new AsyncResult<T, TResult>(EitherMethods.CastErrorAsync<T, TResult, TError>(Either.ToTaskEither()));

        /// <inheritdoc cref="Result{T,TError}.Do" />
        public IAsyncResult<T, TError> Do(Action action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new AsyncResult<T, TError>(EitherMethods.Do(Either.ToTaskEither(), action));
        }

        public IAsyncResult<T, TError> DoAsync(Func<Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new AsyncResult<T, TError>(EitherMethods.DoAsync(Either.ToTaskEither(), action));
        }

        /// <inheritdoc cref="Result{T,TError}.DoWith" />
        public IAsyncResult<T, TError> DoWith(Action<T> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new AsyncResult<T, TError>(EitherMethods.DoWithAsync(Either.ToTaskEither(), action));
        }

        public IAsyncResult<T, TError> DoWithAsync(Func<T, Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new AsyncResult<T, TError>(EitherMethods.DoWithAsync(Either.ToTaskEither(), action));
        }

        /// <inheritdoc cref="Result{T,TError}.DoWithError" />
        public IAsyncResult<T, TError> DoWithError(Action<TError> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new AsyncResult<T, TError>(EitherMethods.DoWithError(Either.ToTaskEither(), action));
        }

        public IAsyncResult<T, TError> DoWithErrorAsync(Func<TError, Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new AsyncResult<T, TError>(EitherMethods.DoWithErrorAsync(Either.ToTaskEither(), action));
        }

        public IAsyncEither<T, TError> Either { get; }

        public IAsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<T, TError>(
                EitherMethods.FilterAsync(Either.ToTaskEither(), predicate, errorSelector));
        }

        public IAsyncResult<T, TError> FilterAsync(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<T, TError>(
                EitherMethods.FilterAsyncPredicate(Either.ToTaskEither(), predicate, errorSelector));
        }

        public IAsyncResult<TResult, TError> FlatMap<TResult>(
            Func<T, TResult?> selector,
            Func<TError> errorSelector
        ) where TResult : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMap(selector.Compose(x => x.ToResult(errorSelector)));
        }

        public IAsyncResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(x => x.Either),
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(x => x.Either),
                    resultSelector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TSelector : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMap(x =>
                selector.Compose(y => y.ToResult(errorSelector))(x).Map(y => resultSelector(x, y)));
        }

        public IAsyncResult<TResult, TError> FlatMap<TResult>(
            Func<T, IResult<TResult, TError>> selector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return FlatMapAsync(selector.Compose(Index.ToAsyncResult));
        }

        public IAsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return FlatMapAsync(selector.Compose(Index.ToAsyncResult), resultSelector);
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult>(
            Func<T, Task<TResult?>> selector,
            Func<TError> errorSelector
        ) where TResult : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMapAsync(selector.Compose(x => x.ToAsyncResult(errorSelector)));
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, Task<TSelector?>> selector,
            Func<T, TSelector, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TSelector : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMapAsync(x =>
                selector.Compose(y => y.ToAsyncResult(errorSelector))(x).Map(y => resultSelector(x, y)));
        }

        public IAsyncResult<TResult, TError>
            FlatMapAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(y => y.Either.ToTaskEither())
                )
            );
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(y => y.Either.ToTaskEither()),
                    resultSelector
                )
            );
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(Either.ToTaskEither(),
                    selector.Compose(x => x.Either.ToTaskEither()),
                    errorSelector)
            );
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(y => y.Either.ToTaskEither()),
                    resultSelector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlattenAsync(selector.Compose(Index.ToAsyncResult), errorSelector);
        }

        public IAsyncResult<T, TError> Flatten<TResult>(
            Func<T, IResult<TResult, TError>> selector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return FlattenAsync(selector.Compose(Index.ToAsyncResult));
        }

        public IAsyncResult<T, TError> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<T, TError>(
                EitherMethods.FlattenAsync(
                    Either.ToTaskEither(),
                    selector.Compose(y => y.Either.ToTaskEither()),
                    errorSelector
                )
            );
        }

        public IAsyncResult<T, TError> FlattenAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<T, TError>(EitherMethods.FlattenAsync(Either.ToTaskEither(),
                selector.Compose(y => y.Either.ToTaskEither())));
        }

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}" />
        public IAsyncResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>()
            => new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullCastAsync<T, TResult, TError, TErrorResult>(Either.ToTaskEither())
            );

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult}" />
        public IAsyncResult<TResult, TResult> FullCast<TResult>() =>
            new AsyncResult<TResult, TResult>(
                EitherMethods.FullCastAsync<T, TResult, TError>(Either.ToTaskEither())
            );

        public IAsyncResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FullFlatMapAsync(selector.Compose(Index.ToAsyncResult), errorSelector);
        }

        public IAsyncResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FullFlatMapAsync(selector.Compose(Index.ToAsyncResult), resultSelector, errorSelector);
        }

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullFlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(y => y.Either.ToTaskEither()),
                    resultSelector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullFlatMapAsync(
                    Either.ToTaskEither(),
                    selector.Compose(y => y.Either.ToTaskEither()),
                    errorSelector
                )
            );
        }

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}" />
        public IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TErrorResult>(EitherMethods.FullMapAsync(Either.ToTaskEither(), selector,
                errorSelector));
        }

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TErrorResult>(EitherMethods.FullMapAsync(Either.ToTaskEither(), selector,
                errorSelector));
        }

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TErrorResult>(EitherMethods.FullMapAsync(Either.ToTaskEither(), selector,
                errorSelector));
        }

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TErrorResult>(EitherMethods.FullMapAsync(Either.ToTaskEither(), selector,
                errorSelector));
        }

        public IAsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector
        ) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<T, TError>(EitherMethods.IsErrorWhenAsync(Either.ToTaskEither(), predicate,
                errorSelector));
        }

        public IAsyncResult<T, TError> IsErrorWhenAsync(
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector
        ) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<T, TError>(
                EitherMethods.IsErrorWhenAsyncPredicate(Either.ToTaskEither(), predicate, errorSelector));
        }

        public IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        ) {
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return JoinAsync(
                inner.ToAsyncResult(),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector
            );
        }

        public IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        ) {
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return JoinAsync(
                inner.ToAsyncResult(),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector,
                comparer
            );
        }

        public IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        ) {
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return new AsyncResult<TResult, TError>(
                EitherMethods.JoinAsync(
                    Either.ToTaskEither(),
                    inner.Either.ToTaskEither(),
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    errorSelector,
                    comparer
                )
            );
        }

        public IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        ) {
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.JoinAsync(
                    Either.ToTaskEither(),
                    inner.Either.ToTaskEither(),
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TError> Map<TResult>(Func<T, TResult> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<TResult, TError>(EitherMethods.MapAsync(Either.ToTaskEither(), selector));
        }

        public IAsyncResult<TResult, TError> MapAsync<TResult>(Func<T, Task<TResult>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<TResult, TError>(EitherMethods.MapAsyncSelector(Either.ToTaskEither(), selector));
        }

        public IAsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<T, TErrorResult>(EitherMethods.MapErrorAsync(Either.ToTaskEither(), selector));
        }

        public IAsyncResult<T, TErrorResult> MapErrorAsync<TErrorResult>(Func<TError, Task<TErrorResult>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<T, TErrorResult>(
                EitherMethods.MapErrorAsyncSelector(Either.ToTaskEither(), selector));
        }

        /// <inheritdoc cref="Result{T,TError}.Match{TResult}" />
        public Task<TResult> Match<TResult>(
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return EitherMethods.MatchAsync(Either.ToTaskEither(), selector, errorSelector);
        }

        /// <inheritdoc cref="Result{T,TError}.Match" />
        public Task Match(Action<T> action, Action<TError> errorAction) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            if (errorAction is null) throw new ArgumentNullException(nameof(errorAction));
            return EitherMethods.MatchAsync(Either.ToTaskEither(), action, errorAction);
        }

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}" />
        public IAsyncResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector) {
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new AsyncResult<TResult, TError>(
                EitherMethods.SafeCastAsync<T, TResult, TError>(Either.ToTaskEither(), errorSelector));
        }

        public IAsyncResult<TResult, TError> Zip<TOther, TResult>(
            IResult<TOther, TError> other,
            Func<T, TOther, TResult> selector
        ) {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return ZipAsync(other.ToAsyncResult(), selector);
        }

        public IAsyncResult<TResult, TError> ZipAsync<TOther, TResult>(
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> selector
        ) {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new AsyncResult<TResult, TError>(EitherMethods.ZipAsync(Either.ToTaskEither(),
                other.Either.ToTaskEither(),
                selector));
        }

        internal static AsyncResult<T, TError> Factory(Task<IEither<T, TError>> result)
            => new AsyncResult<T, TError>(result);
    }
}