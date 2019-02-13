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

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.Do(Action)"/>.
        /// </summary>
        IAsyncMaybe<T> Do(Action action);

        /// <inheritdoc cref="IMaybe{T}.DoAsync(Func{Task})"/>
        IAsyncMaybe<T> DoAsync(Func<Task> action);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.DoWith(Action{T})"/>.
        /// </summary>
        IAsyncMaybe<T> DoWith(Action<T> action);

        /// <inheritdoc cref="IMaybe{T}.DoWithAsync(Func{T, Task})"/>
        IAsyncMaybe<T> DoWithAsync(Func<T, Task> someAction);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.Filter(Func{T, bool})"/>.
        /// </summary>
        IAsyncMaybe<T> Filter(Func<T, bool> predicate);

        /// <inheritdoc cref="IMaybe{T}.FilterAsync(Func{T, Task{bool}})"/>
        IAsyncMaybe<T> FilterAsync(Func<T, Task<bool>> predicate);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.Flatten{TResult}(Func{T, IMaybe{TResult}})"/>.
        /// </summary>
        IAsyncMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector);

        /// <inheritdoc cref="IMaybe{T}.FlattenAsync{TResult}(Func{T, IAsyncMaybe{TResult}})"/>
        IAsyncMaybe<T> FlattenAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.IsNoneWhen(Func{T, bool})"/>.
        /// </summary>
        IAsyncMaybe<T> IsNoneWhen(Func<T, bool> predicate);

        /// <inheritdoc cref="IMaybe{T}.IsNoneWhenAsync(Func{T, Task{bool}})"/>
        IAsyncMaybe<T> IsNoneWhenAsync(Func<T, Task<bool>> predicate);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.FlatMap{TResult}(Func{T, IMaybe{TResult}})"/>.
        /// </summary>
        IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> selector);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.FlatMap{TFlatMap, TResult}(Func{T, IMaybe{TFlatMap}}, Func{T,TFlatMap, TResult})"/>.
        /// </summary>
        IAsyncMaybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        );

        /// <inheritdoc cref="IMaybe{T}.FlatMapAsync{TResult}(Func{T, IAsyncMaybe{TResult}})"/>
        IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, IAsyncMaybe<TResult>> selector);

        /// <inheritdoc cref="IMaybe{T}.FlatMapAsync{TSelector, TResult}(Func{T, IAsyncMaybe{TSelector}}, Func{T, TSelector, TResult})"/>
        IAsyncMaybe<TResult> FlatMapAsync<TFlatMap, TResult>(
            Func<T, IAsyncMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector
        );

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.FlatMap{TResult}(Func{T, Nullable{TResult}})"/>.
        /// </summary>
        IAsyncMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> selector) where TResult : struct;

        /// <inheritdoc cref="IMaybe{T}.FlatMapAsync{TResult}(Func{T, Task{Nullable{TResult}}})"/>
        IAsyncMaybe<TResult> FlatMapAsync<TResult>(Func<T, Task<TResult?>> selector) where TResult : struct;

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.FlatMap{TFlatMap, TResult}(Func{T, Nullable{TFlatMap}}, Func{T,TFlatMap, TResult})"/>.
        /// </summary>
        IAsyncMaybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct;

        /// <inheritdoc cref="IMaybe{T}.FlatMapAsync{TSelector, TResult}(Func{T, Task{Nullable{TSelector}}}, Func{T, TSelector, TResult})"/>
        IAsyncMaybe<TResult> FlatMapAsync<TSelector, TResult>(
            Func<T, Task<TSelector?>> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct;

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.Map{TResult}(Func{T, TResult})"/>.
        /// </summary>
        IAsyncMaybe<TResult> Map<TResult>(Func<T, TResult> selector);

        /// <inheritdoc cref="IMaybe{T}.MapAsync{TResult}(Func{T, Task{TResult}})"/>
        IAsyncMaybe<TResult> MapAsync<TResult>(Func<T, Task<TResult>> selector);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.Match"/>.
        /// </summary>
        Task Match(Action<T> someAction, Action noneAction);

        /// <summary>
        /// An asynchronous version of <see cref="IMaybe{T}.Match{TResult}"/>.
        /// </summary>
        Task<TResult> Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector);
    }
}