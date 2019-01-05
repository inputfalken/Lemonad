using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling;

namespace SpecificErrorType {
    public class ResultModel<T> : IResult<T, ErrorModel> {
        private readonly IResult<T, ErrorModel> _resultImplementation;

        public ResultModel(T element) : this(Result.Value<T, ErrorModel>(element)) { }

        private ResultModel(IResult<T, ErrorModel> resultImplementation)
            => _resultImplementation = resultImplementation;

        public IResult<TResult, ErrorModel> Cast<TResult>() => _resultImplementation.Cast<TResult>();

        public IResult<T, TResult> CastError<TResult>() => _resultImplementation.CastError<TResult>();

        public IResult<T, ErrorModel> Do(Action action) => _resultImplementation.Do(action);

        public IAsyncResult<T, ErrorModel> DoAsync(Func<Task> action) => _resultImplementation.DoAsync(action);

        public IResult<T, ErrorModel> DoWith(Action<T> action) => _resultImplementation.DoWith(action);

        public IAsyncResult<T, ErrorModel> DoWithAsync(Func<T, Task> action) =>
            _resultImplementation.DoWithAsync(action);

        public IResult<T, ErrorModel> DoWithError(Action<ErrorModel> action) =>
            _resultImplementation.DoWithError(action);

        public IAsyncResult<T, ErrorModel> DoWithErrorAsync(Func<ErrorModel, Task> action) =>
            _resultImplementation.DoWithErrorAsync(action);

        public IEither<T, ErrorModel> Either => _resultImplementation.Either;

        public IResult<T, ErrorModel> Filter(Func<T, bool> predicate, Func<T, ErrorModel> errorSelector) =>
            _resultImplementation.Filter(predicate, errorSelector);

        public IAsyncResult<T, ErrorModel> FilterAsync(Func<T, Task<bool>> predicate,
            Func<T, ErrorModel> errorSelector) => _resultImplementation.FilterAsync(predicate, errorSelector);

        public IResult<TResult, ErrorModel> FlatMap<TResult>(Func<T, IResult<TResult, ErrorModel>> selector) =>
            _resultImplementation.FlatMap(selector);

        public IResult<TResult, ErrorModel> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, ErrorModel>> selector, Func<T, TSelector, TResult> resultSelector) =>
            _resultImplementation.FlatMap(selector, resultSelector);

        public IResult<TResult, ErrorModel> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, ErrorModel> errorSelector) =>
            _resultImplementation.FlatMap(selector, errorSelector);

        public IResult<TResult, ErrorModel> FlatMap<TResult>(Func<T, TResult?> flatMapSelector,
            Func<ErrorModel> errorSelector) where TResult : struct =>
            _resultImplementation.FlatMap(flatMapSelector, errorSelector);

        public IAsyncResult<TResult, ErrorModel> FlatMapAsync<TResult>(Func<T, Task<TResult?>> flatMapSelector,
            Func<ErrorModel> errorSelector) where TResult : struct =>
            _resultImplementation.FlatMapAsync(flatMapSelector, errorSelector);

        public IResult<TResult, ErrorModel> FlatMap<TFlatMap, TResult>(Func<T, TFlatMap?> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector, Func<ErrorModel> errorSelector)
            where TFlatMap : struct =>
            _resultImplementation.FlatMap(flatMapSelector, resultSelector, errorSelector);

        public IAsyncResult<TResult, ErrorModel> FlatMapAsync<TFlatMap, TResult>(
            Func<T, Task<TFlatMap?>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<ErrorModel> errorSelector) where TFlatMap : struct =>
            _resultImplementation.FlatMapAsync(flatMapSelector, resultSelector, errorSelector);

        public IResult<TResult, ErrorModel> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, ErrorModel> errorSelector) =>
            _resultImplementation.FlatMap(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, ErrorModel> FlatMapAsync<TResult>(
            Func<T, IAsyncResult<TResult, ErrorModel>> selector) =>
            _resultImplementation.FlatMapAsync(selector);

        public IAsyncResult<TResult, ErrorModel> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, ErrorModel>> selector,
            Func<T, TSelector, TResult> resultSelector) =>
            _resultImplementation.FlatMapAsync(selector, resultSelector);

        public IAsyncResult<TResult, ErrorModel> FlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, ErrorModel> errorSelector
        ) => _resultImplementation.FlatMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, ErrorModel> FlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, ErrorModel> errorSelector
        ) => _resultImplementation.FlatMapAsync(selector, resultSelector, errorSelector);

        public IResult<T, ErrorModel> Flatten<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector, Func<TErrorResult, ErrorModel> errorSelector) =>
            _resultImplementation.Flatten(selector, errorSelector);

        public IResult<T, ErrorModel> Flatten<TResult>(Func<T, IResult<TResult, ErrorModel>> selector) =>
            _resultImplementation.Flatten(selector);

        public IAsyncResult<T, ErrorModel> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector, Func<TErrorResult, ErrorModel> errorSelector) =>
            _resultImplementation.FlattenAsync(selector, errorSelector);

        public IAsyncResult<T, ErrorModel> FlattenAsync<TResult>(
            Func<T, IAsyncResult<TResult, ErrorModel>> selector) => _resultImplementation.FlattenAsync(selector);

        public IResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            _resultImplementation.FullCast<TResult, TErrorResult>();

        public IResult<TResult, TResult> FullCast<TResult>() => _resultImplementation.FullCast<TResult>();

        public IResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<ErrorModel, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMap(selector, errorSelector);

        public IResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector, Func<T, TFlatMap, TResult> resultSelector,
            Func<ErrorModel, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMap(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<ErrorModel, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMapAsync(selector, resultSelector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<ErrorModel, TErrorResult> errorSelector) =>
            _resultImplementation.FullFlatMapAsync(selector, errorSelector);

        public IResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(Func<T, TResult> selector,
            Func<ErrorModel, TErrorResult> errorSelector) => _resultImplementation.FullMap(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector, Func<ErrorModel, Task<TErrorResult>> errorSelector) =>
            _resultImplementation.FullMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector, Func<ErrorModel, TErrorResult> errorSelector) =>
            _resultImplementation.FullMapAsync(selector, errorSelector);

        public IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(Func<T, TResult> selector,
            Func<ErrorModel, Task<TErrorResult>> errorSelector) =>
            _resultImplementation.FullMapAsync(selector, errorSelector);

        public IResult<T, ErrorModel> IsErrorWhen(Func<T, bool> predicate, Func<T, ErrorModel> errorSelector) =>
            _resultImplementation.IsErrorWhen(predicate, errorSelector);

        public IAsyncResult<T, ErrorModel> IsErrorWhenAsync(Func<T, Task<bool>> predicate,
            Func<T, ErrorModel> errorSelector) => _resultImplementation.IsErrorWhenAsync(predicate, errorSelector);

        public IResult<TResult, ErrorModel> Join<TInner, TKey, TResult>(IResult<TInner, ErrorModel> inner,
            Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<ErrorModel> errorSelector) => _resultImplementation.Join(inner, outerKeySelector, innerKeySelector,
            resultSelector, errorSelector);

        public IResult<TResult, ErrorModel> Join<TInner, TKey, TResult>(IResult<TInner, ErrorModel> inner,
            Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<ErrorModel> errorSelector, IEqualityComparer<TKey> comparer) => _resultImplementation.Join(inner,
            outerKeySelector, innerKeySelector, resultSelector, errorSelector, comparer);

        public IAsyncResult<TResult, ErrorModel> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, ErrorModel> inner, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<ErrorModel> errorSelector,
            IEqualityComparer<TKey> comparer) => _resultImplementation.JoinAsync(inner, outerKeySelector,
            innerKeySelector, resultSelector, errorSelector, comparer);

        public IAsyncResult<TResult, ErrorModel> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, ErrorModel> inner, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<ErrorModel> errorSelector) =>
            _resultImplementation.JoinAsync(inner, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector);

        public IResult<TResult, ErrorModel> Map<TResult>(Func<T, TResult> selector) =>
            _resultImplementation.Map(selector);

        public IAsyncResult<TResult, ErrorModel> MapAsync<TResult>(Func<T, Task<TResult>> selector) =>
            _resultImplementation.MapAsync(selector);

        public IResult<T, TErrorResult> MapError<TErrorResult>(Func<ErrorModel, TErrorResult> selector) =>
            _resultImplementation.MapError(selector);

        public IAsyncResult<T, TErrorResult> MapErrorAsync<TErrorResult>(
            Func<ErrorModel, Task<TErrorResult>> selector) => _resultImplementation.MapErrorAsync(selector);

        public TResult Match<TResult>(Func<T, TResult> selector, Func<ErrorModel, TResult> errorSelector) =>
            _resultImplementation.Match(selector, errorSelector);

        public void Match(Action<T> action, Action<ErrorModel> errorAction) =>
            _resultImplementation.Match(action, errorAction);

        public IResult<TResult, ErrorModel> SafeCast<TResult>(Func<T, ErrorModel> errorSelector) =>
            _resultImplementation.SafeCast<TResult>(errorSelector);

        public IResult<TResult, ErrorModel> Zip<TOther, TResult>(IResult<TOther, ErrorModel> other,
            Func<T, TOther, TResult> selector) => _resultImplementation.Zip(other, selector);

        public IAsyncResult<TResult, ErrorModel> ZipAsync<TOther, TResult>(IAsyncResult<TOther, ErrorModel> other,
            Func<T, TOther, TResult> selector) => _resultImplementation.ZipAsync(other, selector);
    }

    public class ErrorModel {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}