﻿using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;
using Lemonad.ErrorHandling.Extensions.AsyncResult;
using Index = Lemonad.ErrorHandling.Extensions.Maybe.Index;

namespace Lemonad.ErrorHandling.Internal {
    internal class AsyncMaybe<T> : IAsyncMaybe<T> {
        public static IAsyncMaybe<T> None = new AsyncMaybe<T>(AsyncResult.Error<T, Unit>(Unit.Default));

        private readonly IAsyncResult<T, Unit> _asyncResult;

        internal AsyncMaybe(in IAsyncResult<T, Unit> result) => _asyncResult = result;

        public IAsyncMaybe<T> Do(Action action) =>
            action == null
                ? throw new ArgumentNullException(nameof(action))
                : _asyncResult.Do(action).ToAsyncMaybe();

        public IAsyncMaybe<T> DoAsync(Func<Task> action) =>
            action == null
                ? throw new ArgumentNullException(nameof(action))
                : _asyncResult.DoAsync(action).ToAsyncMaybe();

        public IAsyncMaybe<T> DoWith(Action<T> action) =>
            action == null
                ? throw new ArgumentNullException(nameof(action))
                : _asyncResult.DoWith(action).ToAsyncMaybe();

        public IAsyncMaybe<T> DoWithAsync(Func<T, Task> someAction) => someAction is null
            ? throw new ArgumentNullException(nameof(someAction))
            : _asyncResult.DoWithAsync(someAction).ToAsyncMaybe();

        public IAsyncMaybe<T> Filter(Func<T, bool> predicate) => predicate is null
            ? throw new ArgumentNullException(nameof(predicate))
            : _asyncResult.Filter(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IAsyncMaybe<T> FilterAsync(Func<T, Task<bool>> predicate) => predicate is null
            ? throw new ArgumentNullException(nameof(predicate))
            : _asyncResult.FilterAsync(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _asyncResult.FlatMap(x => Index.ToResult(selector(x), Unit.Selector)).ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> selector) where TResult : struct =>
            selector is null
                ? throw new ArgumentNullException(nameof(selector))
                : _asyncResult.FlatMap(selector, Unit.Selector).ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return _asyncResult.FlatMap(selector, resultSelector, Unit.Selector).ToAsyncMaybe();
        }

        public IAsyncMaybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        ) {
            if (flatMapSelector is null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return _asyncResult
                .FlatMap(
                    x => Index.ToResult(flatMapSelector(x), Unit.Selector),
                    resultSelector
                ).ToAsyncMaybe();
        }

        public IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, Task<TResult?>> selector) where TResult : struct =>
            selector is null
                ? throw new ArgumentNullException(nameof(selector))
                : _asyncResult.FlatMapAsync(selector, Unit.Selector).ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMapAsync<TSelector, TResult>(
            Func<T, Task<TSelector?>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct {
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return _asyncResult.FlatMapAsync(selector, resultSelector, Unit.Selector).ToAsyncMaybe();
        }

        public IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _asyncResult
                .FlatMapAsync(x => Extensions.AsyncMaybe.Index.ToAsyncResult(selector(x), Unit.Selector))
                .ToAsyncMaybe();

        public IAsyncMaybe<TResult> FlatMapAsync<TFlatMap, TResult>(
            Func<T, IAsyncMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        ) {
            if (flatMapSelector is null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            return _asyncResult
                .FlatMapAsync(
                    x => Extensions.AsyncMaybe.Index.ToAsyncResult(flatMapSelector(x), Unit.Selector),
                    resultSelector
                ).ToAsyncMaybe();
        }

        public IAsyncMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _asyncResult.Flatten(x => Index.ToResult(selector(x), Unit.Selector)).ToAsyncMaybe();

        public IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _asyncResult
                .FlattenAsync(x => Extensions.AsyncMaybe.Index.ToAsyncResult(selector(x), Unit.Selector))
                .ToAsyncMaybe();

        public Task<bool> HasValue => _asyncResult.Either.HasValue;

        public IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate) => predicate is null
            ? throw new ArgumentNullException(nameof(predicate))
            : _asyncResult.IsErrorWhen(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IAsyncMaybe<T> IsNoneWhenAsync(Func<T, Task<bool>> predicate) =>
            predicate == null
                ? throw new ArgumentNullException(nameof(predicate))
                : _asyncResult.IsErrorWhenAsync(predicate, _ => Unit.Default).ToAsyncMaybe();

        public IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector) => selector is null
            ? throw new ArgumentNullException(nameof(selector))
            : _asyncResult.Map(selector).ToAsyncMaybe();

        public IAsyncMaybe<TResult> MapAsync<TResult>(Func<T, Task<TResult>> selector) =>
            selector == null
                ? throw new ArgumentNullException(nameof(selector))
                : _asyncResult.MapAsync(selector).ToAsyncMaybe();

        public Task Match(Action<T> someAction, Action noneAction) {
            if (someAction is null) throw new ArgumentNullException(nameof(someAction));
            if (noneAction is null) throw new ArgumentNullException(nameof(noneAction));
            return _asyncResult.Match(someAction, _ => noneAction());
        }

        public Task<TResult> Match<TResult>(
            Func<T, TResult> someSelector,
            Func<TResult> noneSelector
        ) {
            if (someSelector is null) throw new ArgumentNullException(nameof(someSelector));
            if (noneSelector is null) throw new ArgumentNullException(nameof(noneSelector));
            return _asyncResult.Match(someSelector, x => noneSelector());
        }

        public T Value {
            get {
                try {
                    return _asyncResult.Either.Value;
                }
                catch (InvalidEitherStateException) {
                    throw new InvalidMaybeStateException(
                        $"Can not access property '{nameof(IAsyncMaybe<T>.Value)}' of '{nameof(IAsyncMaybe<T>)}' before property '{nameof(IAsyncMaybe<T>.HasValue)}' has been awaited."
                    );
                }
            }
        }

        public static IAsyncMaybe<T> Create(in T element) => new AsyncMaybe<T>(AsyncResult.Value<T, Unit>(element));
    }
}