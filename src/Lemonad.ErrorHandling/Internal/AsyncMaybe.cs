using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.AsyncResult;

namespace Lemonad.ErrorHandling.Internal {
    internal class AsyncMaybe<T> : IAsyncMaybe<T> {
        private readonly IAsyncResult<T, Unit> _asyncResult;

        public AsyncMaybe(IAsyncResult<T, Unit> result) => _asyncResult = result;

        public Task<bool> HasValue => _asyncResult.Either.HasValue;

        public T Value => _asyncResult.Either.Value;

        public IAsyncMaybe<T> Do(Action action) => _asyncResult.Do(action).ToAsyncMaybe();

        public IAsyncMaybe<T> DoAsync(Func<Task> action) => _asyncResult.DoAsync(action).ToAsyncMaybe();

        public IAsyncMaybe<T> DoWith(Action<T> someAction) => _asyncResult.DoWith(someAction).ToAsyncMaybe();

        public IAsyncMaybe<T> DoWithAsync(Func<T, Task> someAction)
            => _asyncResult.DoWithAsync(someAction).ToAsyncMaybe();

        public IAsyncMaybe<T> Filter(Func<T, bool> predicate)
            => _asyncResult.Filter(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IAsyncMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector)
            => _asyncResult.Flatten(
                    x => Extensions.Maybe.Index.ToResult(selector(x), () => Unit.Default)
                )
                .ToAsyncMaybe();

        public IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector)
            => _asyncResult
                .FlattenAsync(x => Extensions.AsyncMaybe.Index.ToAsyncResult(selector(x), () => Unit.Default))
                .ToAsyncMaybe();

        public IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate) =>
            _asyncResult.IsErrorWhen(predicate, x => Unit.Default).ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> flatMapSelector)
            => _asyncResult.FlatMap(x => Extensions.Maybe.Index.ToResult(flatMapSelector(x), () => Unit.Default))
                .ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> flatMapSelector) =>
            _asyncResult
                .FlatMapAsync(x => Extensions.AsyncMaybe.Index.ToAsyncResult(flatMapSelector(x), () => Unit.Default))
                .ToAsyncMaybe();

        public IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector)
            => _asyncResult.Map(selector).ToAsyncMaybe();

        public Task Match(Action<T> someAction, Action noneAction)
            => _asyncResult.Match(someAction, _ => noneAction());

        public Task<TResult> Match<TResult>(
            Func<T, TResult> someSelector, Func<TResult> noneSelector
        ) => _asyncResult.Match(someSelector, x => noneSelector());

        public IAsyncMaybe<TResult> FlatMapAsync<TFlatMap, TResult>(
            Func<T, IAsyncMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        ) => _asyncResult
            .FlatMapAsync(x => Extensions.AsyncMaybe.Index.ToAsyncResult(flatMapSelector(x), () => Unit.Default),
                resultSelector)
            .ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMap<TFlatMap, TResult>(Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector)
            => _asyncResult
                .FlatMap(x => Extensions.Maybe.Index.ToResult(flatMapSelector(x), () => Unit.Default),
                    resultSelector).ToAsyncMaybe();
    }
}