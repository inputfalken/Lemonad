using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal {
    public struct AsyncMaybe<T> : IAsyncMaybe<T> {
        public Task<bool> HasValue => throw new NotImplementedException();

        public T Value => throw new NotImplementedException();

        public IAsyncMaybe<T> Do(Action action) => throw new NotImplementedException();

        public IAsyncMaybe<T> DoWith(Action<T> someAction) => throw new NotImplementedException();

        public IAsyncMaybe<T> Filter(Func<T, bool> predicate) => throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IAsyncMaybe<TResult>> flatMapSelector) => throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMap<TFlatMap, TResult>(Func<T, IAsyncMaybe<TFlatMap>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector) => throw new NotImplementedException();

        public IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> flatSelector) where TResult : struct => throw new NotImplementedException();

        public IAsyncMaybe<T> Flatten<TResult>(Func<T, IAsyncMaybe<TResult>> selector) => throw new NotImplementedException();

        public IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate) => throw new NotImplementedException();

        public IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector) => throw new NotImplementedException();

        public Task Match(Action<T> someAction, Action noneAction) => throw new NotImplementedException();

        public TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector) => throw new NotImplementedException();
    }
}