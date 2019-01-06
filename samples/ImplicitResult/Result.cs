using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;

namespace ImplicitResult {
    public class Result<T, TError> : IResult<T, TError> {
        private readonly IResult<T, TError> _resultImplementation;

        private Result(IResult<T, TError> resultImplementation) => _resultImplementation = resultImplementation;

        public IResult<TResult, TError> Cast<TResult>() => _resultImplementation.Cast<TResult>();

        public IResult<T, TResult> CastError<TResult>() => _resultImplementation.CastError<TResult>();

        public IResult<T, TError> Do(Action action) => _resultImplementation.Do(action);

        public IAsyncResult<T, TError> DoAsync(Func<Task> action) => _resultImplementation.DoAsync(action);

        public IResult<T, TError> DoWith(Action<T> action) => _resultImplementation.DoWith(action);

        public IAsyncResult<T, TError> DoWithAsync(Func<T, Task> action) =>
            _resultImplementation.DoWithAsync(action);

        public IResult<T, TError> DoWithError(Action<TError> action) => _resultImplementation.DoWithError(action);

        public IAsyncResult<T, TError> DoWithErrorAsync(Func<TError, Task> action) =>
            _resultImplementation.DoWithErrorAsync(action);

        public IEither<T, TError> Either => _resultImplementation.Either;

        public IResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector) =>
            _resultImplementation.Filter(predicate, errorSelector);

        public IAsyncResult<T, TError> FilterAsync(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector) =>
            _resultImplementation.FilterAsync(predicate, errorSelector);

        public IResult<TResult, TError> FlatMap<TResult>(Func<T, IResult<TResult, TError>> selector) =>
            _resultImplementation.FlatMap(selector);

        public IResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector) =>
            _resultImplementation.FlatMap(selector, resultSelector);

        public IResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            _resultImplementation.FlatMap(selector, errorSelector);

        public IResult<TResult, TError> FlatMap<TResult>(Func<T, TResult?> selector, Func<TError> errorSelector)
            where TResult : struct => _resultImplementation.FlatMap(selector, errorSelector);

        public IResult<TResult, TError> FlatMap<TFlatMap, TResult>(Func<T, TFlatMap?> selector,
            Func<T, TFlatMap, TResult> resultSelector, Func<TError> errorSelector)
            where TFlatMap : struct =>
            _resultImplementation.FlatMap(selector, resultSelector, errorSelector);

        public IResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            _resultImplementation.FlatMap(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult>(Func<T, Task<TResult?>> selector,
            Func<TError> errorSelector) where TResult : struct =>
            _resultImplementation.FlatMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult>(Func<T, Task<TFlatMap?>> selector,
            Func<T, TFlatMap, TResult> resultSelector, Func<TError> errorSelector)
            where TFlatMap : struct =>
            _resultImplementation.FlatMapAsync(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TError>
            FlatMapAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector) =>
            _resultImplementation.FlatMapAsync(selector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> selector, Func<T, TSelector, TResult> resultSelector) =>
            _resultImplementation.FlatMapAsync(selector, resultSelector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) => _resultImplementation.FlatMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        ) => _resultImplementation.FlatMapAsync(selector, resultSelector, errorSelector);

        public IResult<T, TError> Flatten<TResult, TErrorResult>(Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector) => _resultImplementation.Flatten(selector, errorSelector);

        public IResult<T, TError> Flatten<TResult>(Func<T, IResult<TResult, TError>> selector) =>
            _resultImplementation.Flatten(selector);

        public IAsyncResult<T, TError> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            _resultImplementation.FlattenAsync(selector, errorSelector);

        public IAsyncResult<T, TError> FlattenAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector) =>
            _resultImplementation.FlattenAsync(selector);

        public IResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            _resultImplementation.FullCast<TResult, TErrorResult>();

        public IResult<TResult, TResult> FullCast<TResult>() => _resultImplementation.FullCast<TResult>();

        public IResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector, Func<TError, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMap(selector, errorSelector);

        public IResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMap(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMapAsync(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMapAsync(selector, errorSelector);

        public IResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector) => _resultImplementation.FullMap(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector) =>
            _resultImplementation.FullMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector) =>
            _resultImplementation.FullMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector) =>
            _resultImplementation.FullMapAsync(selector, errorSelector);

        public IResult<T, TError> IsErrorWhen(Func<T, bool> predicate, Func<T, TError> errorSelector) =>
            _resultImplementation.IsErrorWhen(predicate, errorSelector);

        public IAsyncResult<T, TError> IsErrorWhenAsync(Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector) =>
            _resultImplementation.IsErrorWhenAsync(predicate, errorSelector);

        public IResult<TResult, TError> Join<TInner, TKey, TResult>(IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector) => _resultImplementation.Join(inner, outerKeySelector, innerKeySelector,
            resultSelector, errorSelector);

        public IResult<TResult, TError> Join<TInner, TKey, TResult>(IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector, IEqualityComparer<TKey> comparer) => _resultImplementation.Join(inner,
            outerKeySelector, innerKeySelector, resultSelector, errorSelector, comparer);

        public IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer) =>
            _resultImplementation.JoinAsync(inner, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector,
                comparer);

        public IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            _resultImplementation.JoinAsync(inner, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector);

        public IResult<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            _resultImplementation.Map(selector);

        public IAsyncResult<TResult, TError> MapAsync<TResult>(Func<T, Task<TResult>> selector) =>
            _resultImplementation.MapAsync(selector);

        public IResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            _resultImplementation.MapError(selector);

        public IAsyncResult<T, TErrorResult>
            MapErrorAsync<TErrorResult>(Func<TError, Task<TErrorResult>> selector) =>
            _resultImplementation.MapErrorAsync(selector);

        public TResult Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            _resultImplementation.Match(selector, errorSelector);

        public void Match(Action<T> action, Action<TError> errorAction) {
            _resultImplementation.Match(action, errorAction);
        }

        public IResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector) =>
            _resultImplementation.SafeCast<TResult>(errorSelector);

        public IResult<TResult, TError> Zip<TOther, TResult>(IResult<TOther, TError> other,
            Func<T, TOther, TResult> selector) => _resultImplementation.Zip(other, selector);

        public IAsyncResult<TResult, TError> ZipAsync<TOther, TResult>(IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> selector) => _resultImplementation.ZipAsync(other, selector);

        public IAsyncResult<TFlatMap, TError> FlatMapAsync<TFlatMap, TResult>(Func<T, Task<TFlatMap?>> flatMapSelector,
            Func<TError> errorSelector, Func<T, TFlatMap, TResult> resultSelector) where TFlatMap : struct =>
            throw new NotImplementedException();

        public static implicit operator Result<T, TError>(T value) =>
            new Result<T, TError>(Result.Value<T, TError>(value));

        public static implicit operator Result<T, TError>(TError error) =>
            new Result<T, TError>(Result.Error<T, TError>(error));

        public override string ToString() => _resultImplementation.ToString();
    }
}