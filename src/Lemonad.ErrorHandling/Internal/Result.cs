﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Extensions.Task;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Internal {
    internal class Result<T, TError> : IResult<T, TError> {
        internal Result(in T value, in TError error, bool hasError, bool hasValue) : this(
            new NonNullableEither<T, TError>(
                in value,
                in error,
                hasError,
                hasValue
            )
        ) { }

        private Result(in IEither<T, TError> either)
            => Either = new NonNullableEither<T, TError>(
                either.Value,
                either.Error,
                either.HasError,
                either.HasValue
            );

        public IResult<TResult, TError> Cast<TResult>()
            => new Result<TResult, TError>(
                EitherMethods.Cast<T, TResult, TError>(Either)
            );

        public IResult<T, TResult> CastError<TResult>()
            => new Result<T, TResult>(
                EitherMethods.CastError<T, TResult, TError>(Either)
            );

        public IResult<T, TError> Do(Action action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new Result<T, TError>(
                EitherMethods.Do(
                    Either,
                    action
                )
            );
        }

        public IAsyncResult<T, TError> DoAsync(Func<Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return this.ToAsyncResult().DoAsync(action);
        }

        public IResult<T, TError> DoWith(Action<T> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new Result<T, TError>(
                EitherMethods.DoWith(
                    Either,
                    action
                )
            );
        }

        public IAsyncResult<T, TError> DoWithAsync(Func<T, Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return this.ToAsyncResult().DoWithAsync(action);
        }

        public IResult<T, TError> DoWithError(Action<TError> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return new Result<T, TError>(
                EitherMethods.DoWithError(
                    Either,
                    action
                )
            );
        }

        public IAsyncResult<T, TError> DoWithErrorAsync(Func<TError, Task> action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            return this.ToAsyncResult().DoWithErrorAsync(action);
        }

        public IEither<T, TError> Either { get; }

        public IResult<T, TError> Filter(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<T, TError>(
                EitherMethods.Filter(
                    Either,
                    predicate,
                    errorSelector
                )
            );
        }

        public IAsyncResult<T, TError> FilterAsync(
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector
        ) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FilterAsync(predicate, errorSelector);
        }

        public IResult<TResult, TError> FlatMap<TResult>(Func<T, IResult<TResult, TError>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    selector.Compose(x => x.Either
                    )
                )
            );
        }

        public IResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    selector.Compose(x => x.Either),
                    resultSelector
                )
            );
        }

        public IResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    selector.Compose(x => x.Either),
                    errorSelector
                )
            );
        }

        public IResult<TResult, TError> FlatMap<TResult>(
            Func<T, TResult?> selector,
            Func<TError> errorSelector
        ) where TResult : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMap(selector.Compose(x => x.ToResult(errorSelector)));
        }

        public IResult<TResult, TError> FlatMap<TFlatMap, TResult>(
            Func<T, TFlatMap?> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TFlatMap : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMap(x =>
                selector.Compose(y => y.ToResult(errorSelector))(x).Map(y => resultSelector(x, y)));
        }

        public IResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    selector.Compose(x => x.Either), resultSelector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult>(
            Func<T, IAsyncResult<TResult, TError>> selector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return this.ToAsyncResult().FlatMapAsync(selector);
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return this.ToAsyncResult().FlatMapAsync(selector, resultSelector);
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FlatMapAsync(selector, errorSelector);
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FlatMapAsync(selector, resultSelector, errorSelector);
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult>(
            Func<T, Task<TResult?>> selector,
            Func<TError> errorSelector
        ) where TResult : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMapAsync(selector.Compose(x => x.ToAsyncResult(errorSelector)));
        }

        public IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult>(
            Func<T, Task<TFlatMap?>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TFlatMap : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return FlatMapAsync(x =>
                selector.Compose(y => y.ToAsyncResult(errorSelector))(x).Map(y => resultSelector(x, y)));
        }

        public IResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<T, TError>(
                EitherMethods.Flatten(
                    Either,
                    selector.Compose(x => x.Either),
                    errorSelector
                )
            );
        }

        public IResult<T, TError> Flatten<TResult>(Func<T, IResult<TResult, TError>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new Result<T, TError>(
                EitherMethods.Flatten(
                    Either,
                    selector.Compose(x => x.Either)
                )
            );
        }

        public IAsyncResult<T, TError> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FlattenAsync(selector, errorSelector);
        }

        public IAsyncResult<T, TError> FlattenAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector)
            => this.ToAsyncResult().FlattenAsync(selector);

        public IResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>()
            => new Result<TResult, TErrorResult>(
                EitherMethods.FullCast<T, TResult, TError, TErrorResult>(Either)
            );

        public IResult<TResult, TResult> FullCast<TResult>()
            => new Result<TResult, TResult>(
                EitherMethods.FullCast<T, TResult, TError>(Either)
            );

        public IResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<TResult, TErrorResult>(
                EitherMethods.FullFlatMap(
                    Either,
                    selector.Compose(x => x.Either),
                    errorSelector
                )
            );
        }

        public IResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<TResult, TErrorResult>(
                EitherMethods.FullFlatMap(
                    Either,
                    selector.Compose(x => x.Either),
                    resultSelector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FullFlatMapAsync(selector, resultSelector, errorSelector);
        }

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FullFlatMapAsync(selector, errorSelector);
        }

        public IResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<TResult, TErrorResult>(
                EitherMethods.FullMap(
                    Either,
                    selector,
                    errorSelector
                )
            );
        }

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FullMapAsync(selector, errorSelector);
        }

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FullMapAsync(selector, errorSelector);
        }

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().FullMapAsync(selector, errorSelector);
        }

        public IResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector
        ) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<T, TError>(
                EitherMethods.IsErrorWhen(
                    Either,
                    predicate,
                    errorSelector
                )
            );
        }

        public IAsyncResult<T, TError> IsErrorWhenAsync(
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector
        ) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this.ToAsyncResult().IsErrorWhenAsync(predicate, errorSelector);
        }

        public IResult<TResult, TError> Join<TInner, TKey, TResult>(
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
            return new Result<TResult, TError>(
                EitherMethods.Join(
                    Either,
                    inner.Either,
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    errorSelector
                )
            );
        }

        public IResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        ) {
            if (inner == null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return new Result<TResult, TError>(
                EitherMethods.Join(
                    Either,
                    inner.Either,
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
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        ) {
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            if (comparer is null) throw new ArgumentNullException(nameof(comparer));
            return this
                .ToAsyncResult()
                .JoinAsync(
                    inner,
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
            Func<TError> errorSelector
        ) {
            if (inner is null) throw new ArgumentNullException(nameof(inner));
            if (outerKeySelector is null) throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector is null) throw new ArgumentNullException(nameof(innerKeySelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return this
                .ToAsyncResult()
                .JoinAsync(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    errorSelector
                );
        }

        public IResult<TResult, TError> Map<TResult>(Func<T, TResult> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new Result<TResult, TError>(
                EitherMethods.Map(
                    Either,
                    selector
                )
            );
        }

        public IAsyncResult<TResult, TError> MapAsync<TResult>(Func<T, Task<TResult>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return this.ToAsyncResult().MapAsync(selector);
        }

        public IResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new Result<T, TErrorResult>(
                EitherMethods.MapError(
                    Either,
                    selector
                )
            );
        }

        public IAsyncResult<T, TErrorResult> MapErrorAsync<TErrorResult>(Func<TError, Task<TErrorResult>> selector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return this.ToAsyncResult().MapErrorAsync(selector);
        }

        public TResult Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return EitherMethods.Match(
                Either,
                selector,
                errorSelector
            );
        }

        public void Match(Action<T> action, Action<TError> errorAction) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            if (errorAction is null) throw new ArgumentNullException(nameof(errorAction));
            EitherMethods.Match(
                Either,
                action,
                errorAction
            );
        }

        public IResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector) {
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return new Result<TResult, TError>(
                EitherMethods.SafeCast<T, TResult, TError>(Either, errorSelector)
            );
        }

        public IResult<TResult, TError> Zip<TOther, TResult>(
            IResult<TOther, TError> other,
            Func<T, TOther, TResult> selector
        ) {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return new Result<TResult, TError>(
                EitherMethods.Zip(
                    Either,
                    other.Either,
                    selector
                )
            );
        }

        public IAsyncResult<TResult, TError> ZipAsync<TOther, TResult>(
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> selector
        ) {
            if (other is null) throw new ArgumentNullException(nameof(other));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return this.ToAsyncResult().ZipAsync(other, selector);
        }

        internal static IResult<T, TError> Factory(IEither<T, TError> either) => new Result<T, TError>(either);

        public override string ToString() =>
            $"{(Either.HasValue ? "Ok" : "Error")} ==> {typeof(Result<T, TError>).ToHumanString()}{StringFunctions.PrettyTypeString(Either.HasValue ? (object) Either.Value : Either.Error)}";
    }
}