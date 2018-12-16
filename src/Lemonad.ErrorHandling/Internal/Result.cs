using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Result;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Internal {
    internal readonly struct Result<T, TError> : IResult<T, TError> {
        internal static IResult<T, TError> ValueFactory(in T element)
            => new Result<T, TError>(
                in element,
                default,
                false,
                true
            );

        internal static IResult<T, TError> ErrorFactory(in TError error)
            => new Result<T, TError>(
                default,
                in error,
                true,
                false
            );

        public IAsyncResult<T, TError> DoAsync(Func<Task> selector)
            => this.ToAsyncResult().DoAsync(selector);

        public IAsyncResult<T, TError> DoWithAsync(Func<T, Task> selector)
            => this.ToAsyncResult().DoWithAsync(selector);

        public IAsyncResult<T, TError> DoWithErrorAsync(Func<TError, Task> selector)
            => this.ToAsyncResult().DoWithErrorAsync(selector);

        public IAsyncResult<T, TError> FilterAsync(
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector
        ) => this.ToAsyncResult().FilterAsync(predicate, errorSelector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult>(
            Func<T, IAsyncResult<TResult, TError>> flatSelector
        ) => this.ToAsyncResult().FlatMapAsync(flatSelector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector
        ) => this.ToAsyncResult().FlatMapAsync(flatSelector, resultSelector);

        public IAsyncResult<T, TError> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) => this.ToAsyncResult().FlattenAsync(selector, errorSelector);

        public IAsyncResult<T, TError> FlattenAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector)
            => this.ToAsyncResult().FlattenAsync(selector);

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) => this.ToAsyncResult().FullFlatMapAsync(flatMapSelector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector
        ) => this.ToAsyncResult().FullFlatMapAsync(flatMapSelector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) => this.ToAsyncResult().FullMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) => this.ToAsyncResult().FullMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) => this.ToAsyncResult().FullMapAsync(selector, errorSelector);

        public IAsyncResult<T, TError> IsErrorWhenAsync(
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector
        ) => this.ToAsyncResult().IsErrorWhenAsync(predicate, errorSelector);

        public IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        ) => this
            .ToAsyncResult()
            .JoinAsync(
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector,
                comparer
            );

        public IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        ) => this
            .ToAsyncResult()
            .JoinAsync(
                inner,
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector
            );

        public IAsyncResult<TResult, TError> MapAsync<TResult>(Func<T, Task<TResult>> selector)
            => this.ToAsyncResult().MapAsync(selector);

        public IAsyncResult<T, TErrorResult> MapErrorAsync<TErrorResult>(Func<TError, Task<TErrorResult>> selector)
            => this.ToAsyncResult().MapErrorAsync(selector);

        public IAsyncResult<TResult, TError> ZipAsync<TOther, TResult>(
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        ) => this.ToAsyncResult().ZipAsync(other, resultSelector);

        public IEither<T, TError> Either { get; }

        private Result(in T value, in TError error, bool hasError, bool hasValue)
            => Either = new NonNullableEither<T, TError>(
                in value,
                in error,
                hasError,
                hasValue
            );

        private Result(in IEither<T, TError> either)
            => Either = new NonNullableEither<T, TError>(
                either.Value,
                either.Error,
                either.HasError,
                either.HasValue
            );

        public override string ToString() =>
            $"{(Either.HasValue ? "Ok" : "Error")} ==> {typeof(Result<T, TError>).ToHumanString()}{StringFunctions.PrettyTypeString(Either.HasValue ? (object) Either.Value : Either.Error)}";

        public TResult Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector)
            => EitherMethods.Match(
                Either,
                selector,
                errorSelector
            );

        public void Match(Action<T> action, Action<TError> errorAction)
            => EitherMethods.Match(
                Either,
                action,
                errorAction
            );

        public IResult<T, TError> DoWith(Action<T> action)
            => new Result<T, TError>(
                EitherMethods.DoWith(
                    Either,
                    action
                )
            );

        public IResult<T, TError> Do(Action action)
            => new Result<T, TError>(
                EitherMethods.Do(
                    Either,
                    action
                )
            );

        public IResult<T, TError> DoWithError(Action<TError> action)
            => new Result<T, TError>(
                EitherMethods.DoWithError(
                    Either,
                    action
                )
            );

        public IResult<T, TError> Filter(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector)
            => new Result<T, TError>(
                EitherMethods.Filter(
                    Either,
                    predicate,
                    errorSelector
                )
            );

        public IResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector)
            => new Result<T, TError>(
                EitherMethods.IsErrorWhen(
                    Either,
                    predicate,
                    errorSelector
                )
            );

        public IResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector)
            => new Result<TResult, TErrorResult>(
                EitherMethods.FullMap(
                    Either,
                    selector,
                    errorSelector
                )
            );

        public IResult<TResult, TError> Map<TResult>(Func<T, TResult> selector)
            => new Result<TResult, TError>(
                EitherMethods.Map(
                    Either,
                    selector
                )
            );

        public IResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector)
            => new Result<T, TErrorResult>(
                EitherMethods.MapError(
                    Either,
                    selector
                )
            );

        public IResult<TResult, TError> FlatMap<TResult>(Func<T, IResult<TResult, TError>> flatSelector)
            => new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    flatSelector.Compose(x => x.Either
                    )
                )
            );

        public IResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector)
            => new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    flatSelector.Compose(x => x.Either),
                    resultSelector
                )
            );

        public IResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> flatMapSelector,
            Func<TErrorResult, TError> errorSelector)
            => new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    flatMapSelector.Compose(x => x.Either),
                    errorSelector
                )
            );

        public IResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector)
            => new Result<TResult, TError>(
                EitherMethods.FlatMap(
                    Either,
                    flatMapSelector.Compose(x => x.Either), resultSelector,
                    errorSelector
                )
            );

        public IResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) => new Result<T, TError>(
            EitherMethods.Flatten(
                Either,
                selector.Compose(x => x.Either),
                errorSelector
            )
        );

        public IResult<T, TError> Flatten<TResult>(Func<T, IResult<TResult, TError>> selector)
            => new Result<T, TError>(
                EitherMethods.Flatten(
                    Either,
                    selector.Compose(x => x.Either)
                )
            );

        public IResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector)
            => new Result<TResult, TErrorResult>(
                EitherMethods.FullFlatMap(
                    Either,
                    flatMapSelector.Compose(x => x.Either),
                    errorSelector
                )
            );

        public IResult<T, TResult> CastError<TResult>()
            => new Result<T, TResult>(
                EitherMethods.CastError<T, TResult, TError>(Either)
            );

        public IResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>()
            => new Result<TResult, TErrorResult>(
                EitherMethods.FullCast<T, TResult, TError, TErrorResult>(Either)
            );

        public IResult<TResult, TResult> FullCast<TResult>()
            => new Result<TResult, TResult>(
                EitherMethods.FullCast<T, TResult, TError>(Either)
            );

        public IResult<TResult, TError> Cast<TResult>()
            => new Result<TResult, TError>(
                EitherMethods.Cast<T, TResult, TError>(Either)
            );

        public IResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector)
            => new Result<TResult, TError>(
                EitherMethods.SafeCast<T, TResult, TError>(Either, errorSelector)
            );

        public IResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector)
            => new Result<TResult, TError>(
                EitherMethods.Join(
                    Either,
                    inner.Either,
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    errorSelector
                )
            );

        public IResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer)
            => new Result<TResult, TError>(
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

        public IResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector)
            => new Result<TResult, TErrorResult>(
                EitherMethods.FullFlatMap(
                    Either,
                    flatMapSelector.Compose(x => x.Either),
                    resultSelector,
                    errorSelector
                )
            );

        public IResult<TResult, TError> Zip<TOther, TResult>(
            IResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector)
            => new Result<TResult, TError>(
                EitherMethods.Zip(
                    Either,
                    other.Either,
                    resultSelector
                )
            );
    }
}