using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Provides a set of static methods for <see cref="IMaybe{T}" />.
    /// </summary>
    public static class Maybe {
        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have no value.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> who will have no value.
        /// </returns>
        public static IMaybe<TSource> None<TSource>() => Maybe<TSource>.None;

        /// <summary>
        ///     Treat <typeparamref name="TSource" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Maybe{T}" /> with LINQ's API.
        /// </summary>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IMaybe<TSource> source) {
            if (source.HasValue)
                yield return source.Value;
        }

        /// <summary>
        ///     Works like <see cref="Value{TSource}(TSource)" /> but with an <paramref name="predicate" /> to test the element.
        /// </summary>
        /// <param name="source">
        ///     The element to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test the element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource source, Func<TSource, bool> predicate) {
            if (predicate != null)
                return predicate(source) ? Value(source) : None<TSource>();
            throw new ArgumentNullException(nameof(predicate));
        }

        /// <summary>
        ///     Converts an <see cref="Nullable{T}" /> to an <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Nullable{T}" /> to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource? source) where TSource : struct =>
            source.HasValue ? Value(source.Value) : None<TSource>();

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have no value.
        /// </summary>
        /// <param name="item">
        ///     The value that will be considered to not have a value.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> who will have no value.
        /// </returns>
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource item) => None<TSource>();

        /// <summary>
        ///     Works like <see cref="ToMaybeNone{TSource}(TSource)" /> but with an <paramref name="predicate" /> to test the
        ///     element.
        /// </summary>
        /// <param name="source">
        ///     The element to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test the element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource source, Func<TSource, bool> predicate) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(source) ? None<TSource>() : Value(source);
        }

        /// <summary>
        ///     Converts an <see cref="Maybe{T}" /> to an <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Maybe{T}" /> to be passed into <see cref="Result{T,TError}" />.
        /// </param>
        /// <param name="errorSelector">
        ///     A function to be executed if there is no value inside the <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type inside the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type representing an error for the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        public static IResult<T, TError> ToResult<T, TError>(this IMaybe<T> source,
            Func<TError> errorSelector) => errorSelector == null
            ? throw new ArgumentNullException(nameof(errorSelector))
            : source.ToResult(x => x.HasValue, x => errorSelector()).Map(x => x.Value);

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have the value <paramref name="item" />.
        /// </summary>
        /// <param name="item">
        ///     The value of <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> whose value will be <paramref name="item" />.
        /// </returns>
        public static IMaybe<TSource> Value<TSource>(TSource item) => Maybe<TSource>.Create(item);
    }
}
