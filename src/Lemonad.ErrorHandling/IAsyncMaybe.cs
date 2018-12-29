using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Represents an asynchronous version of <see cref="IMaybe{T}" />.
    /// </summary>
    public interface IAsyncMaybe<out T> {
        /// <summary>
        ///     An asynchronous version of <see cref="IMaybe{T}.HasValue" />.
        /// </summary>
        Task<bool> HasValue { get; }

        /// <summary>
        ///     Represents a property whose value is available when <see cref="HasValue" /> is true.
        /// </summary>
        /// <remarks>
        ///     You must await property <see cref="HasValue"/> before accessing this property.
        /// </remarks>
        /// <example>
        ///<code language="c#">
        ///     if (await maybe.HasValue)
        ///     {
        ///          // Safe to use.
        ///         Console.WriteLine(maybe.Value)
        ///     }
        /// </code>
        /// </example>
        T Value { get; }

        IAsyncMaybe<T> Do(Action action);
        IAsyncMaybe<T> DoAsync(Func<Task> action);
        IAsyncMaybe<T> DoWith(Action<T> action);
        IAsyncMaybe<T> DoWithAsync(Func<T, Task> someAction);
        IAsyncMaybe<T> Filter(Func<T, bool> predicate);
        IAsyncMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector);
        IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector);
        IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate);
        IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> selector);
        IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> selector) where TResult : struct;
        IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, Task<TResult?>> selector) where TResult : struct;

        IAsyncMaybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct;

        IAsyncMaybe<TResult> FlatMapAsync<TSelector, TResult>(
            Func<T, Task<TSelector?>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct;

        IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector);
        IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector);
        Task Match(Action<T> someAction, Action noneAction);
        Task<TResult> Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector);

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