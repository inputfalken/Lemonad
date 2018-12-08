using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    public interface IAsyncMaybe<out T> {
        Task<bool> HasValue { get; }
        T Value { get; }
        IAsyncMaybe<T> Do(Action action);
        IAsyncMaybe<T> DoWith(Action<T> someAction);
        IAsyncMaybe<T> Filter(Func<T, bool> predicate);
        IAsyncMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector);
        IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector);
        IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate);
        IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> flatMapSelector);
        IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> flatMapSelector);
        IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector);
        Task Match(Action<T> someAction, Action noneAction);
        TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector);

        IAsyncMaybe<TResult> FlatMapAsync<TFlatMap, TResult>(
            Func<T, IAsyncMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        );

        IAsyncMaybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        );
    }
}