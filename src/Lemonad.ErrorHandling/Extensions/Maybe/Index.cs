using System;
using System.Collections.Generic;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.Maybe {
    public static class Index {
        /// <summary>
        ///     Flatmaps a <see cref="Nullable{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IMaybe{T}" /> to flatmap a <see cref="Nullable{T}" /> with.
        /// </param>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as its return type.
        /// </param>
        /// <typeparam name="T">
        ///     The type from <see cref="IMaybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type <typeparamref name="T" /> returned from the <paramref name="flatSelector" /> function.
        /// </typeparam>
        public static IMaybe<TResult> FlatMap<T, TResult>(this IMaybe<T> source, Func<T, TResult?> flatSelector)
            where TResult : struct {
            if (flatSelector == null)
                throw new ArgumentNullException(nameof(flatSelector));
            if (!source.HasValue) return Maybe<TResult>.None;
            var selector = flatSelector(source.Value);
            return selector.HasValue ? Maybe<TResult>.Create(selector.Value) : Maybe<TResult>.None;
        }

        /// <summary>
        ///     Treat <typeparamref name="TSource" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Maybe{T}" /> with LINQ's API.
        /// </summary>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IMaybe<TSource> source) {
            if (source.HasValue)
                yield return source.Value;
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
    }
}