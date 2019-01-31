using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Represents a data structure commonly used in cases where you may or may not have a value present.
    /// </summary>
    /// <typeparam name="T">
    ///     The type of to potential value.
    /// </typeparam>
    public interface IMaybe<out T> {
        /// <summary>
        ///     Gets a value indicating whether the current <see cref="Maybe{T}" /> object has a valid value of
        ///     its underlying type.
        /// </summary>
        /// <returns>
        ///     true if the current <see cref="Maybe{T}"></see> object has a value; false if the current
        ///     <see cref="Maybe{T}"></see> object has no value.
        /// </returns>
        bool HasValue { get; }

        /// <summary>
        ///     Gets the value of the current <see cref="Maybe{T}"></see> object if <see cref="HasValue" /> is true.
        /// </summary>
        /// <example>
        ///     <code language="c#">
        ///  if (Either.HasValue)
        ///  {
        ///      // Safe to use.
        ///      Console.WriteLine(Either.Value)
        ///  }
        ///  </code>
        /// </example>
        T Value { get; }

        /// <summary>
        ///     Executes <paramref name="action" /> no matter what state <see cref="IMaybe{T}" /> is in.
        /// </summary>
        IMaybe<T> Do(Action action);

        /// <summary>
        ///     Executes the <paramref name="action" /> when property <see cref="HasValue" /> is true.
        /// </summary>
        IMaybe<T> DoWith(Action<T> action);

        /// <summary>
        ///  An asynchronous version of <see cref="DoWith"/>.
        /// </summary>
        IAsyncMaybe<T> DoWithAsync(Func<T, Task> action);

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        IMaybe<T> Filter(Func<T, bool> predicate);

        /// <summary>
        /// An asynchronous version of <see cref="Filter(Func{T, bool})"/>.
        /// </summary>
        IAsyncMaybe<T> FilterAsync(Func<T, Task<bool>> predicate);

        /// <summary>
        ///     Flatmaps another <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Maybe{T}" /> as its return type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type <typeparamref name="T" /> returned from the <paramref name="selector" /> function.
        /// </typeparam>
        IMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> selector);

        /// <summary>
        ///     Flatmaps another <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Maybe{T}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TSelector" /> which can return
        ///     any type.
        /// </param>
        /// <typeparam name="TSelector">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="selector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        IMaybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, IMaybe<TSelector>> selector,
            Func<T, TSelector, TResult> resultSelector
        );

        /// <summary>
        ///     Flatmaps another <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Nullable{T}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TSelector" /> which can return
        ///     any type.
        /// </param>
        /// <typeparam name="TSelector">
        ///     The value type of the <see cref="Nullable{T}" /> returned by the <paramref name="selector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        IMaybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector
        ) where TSelector : struct;

        IMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> selector) where TResult : struct;

        IMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector);

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        IMaybe<T> IsNoneWhen(Func<T, bool> predicate);
        
        IAsyncMaybe<T> IsNoneWhenAsync(Func<T, Task<bool>> predicate);

        /// <summary>
        ///     Maps <typeparamref name="T" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <see cref="Maybe{T}" /> has a value.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type returned from the function <paramref name="selector" />.
        /// </typeparam>
        IMaybe<TResult> Map<TResult>(Func<T, TResult> selector);

        /// <summary>
        ///     Evaluates the <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="someAction">
        ///     Is executed when the <see cref="Maybe{T}" /> has a value.
        /// </param>
        /// <param name="noneAction">
        ///     Is executed when he <see cref="Maybe{T}" /> has no value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     When either <paramref name="someAction" /> or <paramref name="noneAction" /> needs to be executed.
        /// </exception>
        void Match(Action<T> someAction, Action noneAction);

        /// <summary>
        ///     Evaluates the <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="someSelector">
        ///     Is executed when the <see cref="Maybe{T}" /> has a value.
        /// </param>
        /// <param name="noneSelector">
        ///     Is executed when he <see cref="Maybe{T}" /> has no value.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type returned by the functions <paramref name="someSelector" /> and <paramref name="noneSelector" />.
        /// </typeparam>
        TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector);
    }
}