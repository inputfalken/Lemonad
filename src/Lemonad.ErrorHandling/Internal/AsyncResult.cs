using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Either;

namespace Lemonad.ErrorHandling.Internal {
    /// <summary>
    ///     An asynchronous version of <see cref="Result{T,TError}" /> with the same functionality.
    /// </summary>
    internal readonly struct AsyncResult<T, TError> : IAsyncResult<T, TError> {

        private AsyncResult(Task<IEither<T, TError>> either) => Either = either;

        public Task<IEither<T, TError>> Either { get; }

        public static AsyncResult<T, TError> Factory(Task<IResult<T, TError>> result) =>
            new AsyncResult<T, TError>(FactoryInternal(result));

        public static AsyncResult<T, TError> ValueFactory(in T value) =>
            new AsyncResult<T, TError>(Task.FromResult(Result.Value<T, TError>(value).Either));

        private static async Task<IEither<T, TError>> FactoryInternal(Task<IResult<T, TError>> value) =>
            (await value.ConfigureAwait(false)).Either;

        private static async Task<IEither<T, TError>> ValueFactoryInternal(Task<T> value) =>
            Result.Value<T, TError>(await value.ConfigureAwait(false)).Either;

        private static async Task<IEither<T, TError>> ErrorFactoryInternal(Task<TError> error) =>
            Result.Error<T, TError>(await error.ConfigureAwait(false)).Either;

        public static AsyncResult<T, TError> ValueFactory(Task<T> value) =>
            new AsyncResult<T, TError>(ValueFactoryInternal(value));

        public static AsyncResult<T, TError> ErrorFactory(Task<TError> error) =>
            new AsyncResult<T, TError>(ErrorFactoryInternal(error));

        public static AsyncResult<T, TError> ErrorFactory(in TError error) =>
            new AsyncResult<T, TError>(Task.FromResult(Result.Error<T, TError>(error).Either));

        public IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            new AsyncResult<TResult, TError>(EitherMethods.JoinAsync(Either, inner.Either, outerKeySelector,
                innerKeySelector, resultSelector,
                errorSelector, comparer));

        public IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.JoinAsync(Either, inner.Either, outerKeySelector,
                innerKeySelector, resultSelector,
                errorSelector));

        [Pure]
        public IAsyncResult<TResult, TError> Zip<TOther, TResult>(IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.ZipAsync(Either, other.Either, resultSelector));

        public IAsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector) =>
            new AsyncResult<T, TError>(EitherMethods.FilterAsync(Either, predicate, errorSelector));

        public IAsyncResult<T, TError> Filter(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector) =>
            new AsyncResult<T, TError>(EitherMethods.FilterAsyncPredicate(Either, predicate, errorSelector));

        public IAsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) =>
            new AsyncResult<T, TError>(EitherMethods.IsErrorWhenAsync(Either, predicate, errorSelector));

        public IAsyncResult<T, TError> IsErrorWhen(
            Func<T, Task<bool>> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            new AsyncResult<T, TError>(
                EitherMethods.IsErrorWhenAsyncPredicate(Either, predicate, errorSelector));

        public IAsyncResult<T, IReadOnlyList<TError>> Multiple(
            params Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>>[] validations) {
            var tmp = this;
            return new AsyncResult<T, IReadOnlyList<TError>>(EitherMethods.MultipleAsync(Either,
                validations.Select(x => x.Compose(y => y.Either)(tmp)).ToArray()));
        }

        public IAsyncResult<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            new AsyncResult<TResult, TError>(EitherMethods.MapAsync(Either, selector));

        public IAsyncResult<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector) =>
            new AsyncResult<TResult, TError>(EitherMethods.MapAsyncSelector(Either, selector));

        public IAsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            new AsyncResult<T, TErrorResult>(EitherMethods.MapErrorAsync(Either, selector));

        public IAsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, Task<TErrorResult>> selector) =>
            new AsyncResult<T, TErrorResult>(EitherMethods.MapErrorAsyncSelector(Either, selector));

        /// <inheritdoc cref="Result{T,TError}.Do" />
        public IAsyncResult<T, TError> Do(Action action) =>
            new AsyncResult<T, TError>(EitherMethods.DoAsync(Either, action));

        /// <inheritdoc cref="Result{T,TError}.DoWithError" />
        public IAsyncResult<T, TError> DoWithError(Action<TError> action) =>
            new AsyncResult<T, TError>(EitherMethods.DoWithErrorAsync(Either, action));

        /// <inheritdoc cref="Result{T,TError}.DoWith" />
        public IAsyncResult<T, TError> DoWith(Action<T> action) =>
            new AsyncResult<T, TError>(EitherMethods.DoWithAsync(Either, action));

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}" />
        public IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
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

        public IAsyncResult<TResult, TError> FlatMap<TResult>(Func<T, IAsyncResult<TResult, TError>> flatSelector) =>
            new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(Either, flatSelector.Compose(y => y.Either)));

        public IAsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => new AsyncResult<TResult, TError>(
            EitherMethods.FlatMapAsync(Either, flatSelector.Compose(y => y.Either), resultSelector));

        public IAsyncResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            return new AsyncResult<TResult, TError>(
                EitherMethods.FlatMapAsync(Either,
                    flatMapSelector.Compose(x => x.Either),
                    errorSelector)
            );
        }

        public IAsyncResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.FlatMapAsync(Either, flatMapSelector.Compose(y => y.Either),
                resultSelector, errorSelector));

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}" />
        public IAsyncResult<TResult, TError> Cast<TResult>() =>
            new AsyncResult<TResult, TError>(EitherMethods.CastAsync<T, TResult, TError>(Either));

        public IAsyncResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            new AsyncResult<T, TError>(
                EitherMethods.FlattenAsync(Either, selector.Compose(y => y.Either), errorSelector));

        public IAsyncResult<T, TError> Flatten<TResult>(Func<T, IAsyncResult<TResult, TError>> selector) =>
            new AsyncResult<T, TError>(EitherMethods.FlattenAsync(Either, selector.Compose(y => y.Either)));

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}" />
        public IAsyncResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullCastAsync<T, TResult, TError, TErrorResult>(Either));

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult}" />
        public IAsyncResult<TResult, TResult> FullCast<TResult>() =>
            new AsyncResult<TResult, TResult>(EitherMethods.FullCastAsync<T, TResult, TError>(Either));

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}" />
        public IAsyncResult<T, TResult> CastError<TResult>() =>
            new AsyncResult<T, TResult>(EitherMethods.CastErrorAsync<T, TResult, TError>(Either));

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}" />
        public IAsyncResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector) =>
            new AsyncResult<TResult, TError>(EitherMethods.SafeCastAsync<T, TResult, TError>(Either, errorSelector));

        public IAsyncResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            new AsyncResult<TResult, TErrorResult>(EitherMethods.FullFlatMapAsync(Either,
                flatMapSelector.Compose(y => y.Either), resultSelector, errorSelector));

        public IAsyncResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            new AsyncResult<TResult, TErrorResult>(
                EitherMethods.FullFlatMapAsync(Either, flatMapSelector.Compose(y => y.Either),
                    errorSelector));
    }
}