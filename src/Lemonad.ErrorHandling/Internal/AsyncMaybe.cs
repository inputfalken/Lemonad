using System;
using System.Threading;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;

namespace Lemonad.ErrorHandling.Internal {
    internal class AsyncMaybe<T> : IAsyncMaybe<T> {
        private readonly Task<IMaybe<T>> _maybe;
        private bool _hasValue;
        private bool _isAssigned;
        private T _value;
        private readonly SemaphoreSlim _semaphoreSlim;

        private async Task AssignProperties() {
            var maybe = await _maybe.ConfigureAwait(false);
            if (maybe.HasValue) Value = maybe.Value;
            _hasValue = maybe.HasValue;
            _isAssigned = true;
        }

        private async Task<bool> Resolve() {
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
            if (_isAssigned) return _hasValue;
            try {
                await AssignProperties().ConfigureAwait(false);
            }
            finally {
                _semaphoreSlim.Release();
            }

            return _hasValue;
        }

        public AsyncMaybe(Task<IMaybe<T>> maybe) {
            _maybe = maybe;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        public Task<bool> HasValue => _isAssigned ? Task.FromResult(_hasValue) : Resolve();

        /// Should this throw exception when
        /// <see cref="IAsyncEither{T,TError}.HasError" />
        /// is true?
        public T Value {
            get => _isAssigned
                ? _value
                : throw new InvalidMaybeStateException(
                    $"Can not access property '{nameof(IAsyncMaybe<T>.Value)}' of before '{nameof(IAsyncMaybe<T>.HasValue)}' has been awaited."
                );
            private set => _value = value;
        }

        public IAsyncMaybe<T> Do(Action action) => throw new NotImplementedException();

        public IAsyncMaybe<T> DoAsync(Func<Task> action) => throw new NotImplementedException();

        public IAsyncMaybe<T> DoWith(Action<T> someAction) => throw new NotImplementedException();

        public IAsyncMaybe<T> DoWithAsync(Func<T, Task> someAction) => throw new NotImplementedException();

        public IAsyncMaybe<T> Filter(Func<T, bool> predicate) => throw new NotImplementedException();

        public IAsyncMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector) =>
            throw new NotImplementedException();

        public IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector) =>
            throw new NotImplementedException();

        public IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate) => throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> flatMapSelector) =>
            throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> flatMapSelector) =>
            throw new NotImplementedException();

        public IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector) => throw new NotImplementedException();

        public Task Match(Action<T> someAction, Action noneAction) => throw new NotImplementedException();

        public TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector) =>
            throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMapAsync<TFlatMap, TResult>(Func<T, IAsyncMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) => throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMap<TFlatMap, TResult>(Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) => throw new NotImplementedException();
    }
}