using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    public static class MaybeEnumerable {
        /// <summary>
        ///     Works just like <see cref="Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
        ///     but returns a <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     Returns the first element of a sequence inside a <see cref="Maybe{T}" />.
        /// </returns>
        public static IMaybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source) {
            switch (source) {
                case IList<TSource> list when list.Count > 0:
                    return Maybe.Value(list[0]);
                case IReadOnlyList<TSource> readOnlyList when readOnlyList.Count > 0:
                    return Maybe.Value(readOnlyList[0]);
                default:
                    using (var e = source.GetEnumerator()) {
                        return e.MoveNext() ? Maybe.Value(e.Current) : Maybe.None<TSource>();
                    }
            }
        }

        /// <summary>
        ///     Works just like <see cref="Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
        ///     but returns a <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test each element for a condition.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     Returns the first element of a sequence who matches the <paramref name="predicate" /> inside a
        ///     <see cref="Maybe{T}" />.
        /// </returns>
        public static IMaybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate) {
            foreach (var element in source)
                if (predicate(element))
                    return Maybe.Value(element);

            return Maybe.None<TSource>();
        }

        /// <summary>
        ///     Executes <see cref="Maybe{T}.Match" /> for each element in the sequence.
        /// </summary>
        public static IEnumerable<TResult> Match<TSource, TResult>(this IEnumerable<IMaybe<TSource>> source,
            Func<TSource, TResult> someSelector, Func<TResult> noneSelector) =>
            source.Select(x => x.Match(someSelector, noneSelector));

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" /> into an <see cref="IEnumerable{T}" /> of
        ///     <typeparamref name="TResult" /> for each element which do not have a value.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="selector">
        ///     A function to return a value for each <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type returned by the function <paramref name="selector" />.
        /// </typeparam>
        public static IEnumerable<TResult> NoValues<TSource, TResult>(this IEnumerable<IMaybe<TSource>> source,
            Func<TResult> selector) => source.Where(x => x.HasValue == false).Select(_ => selector());

        /// <summary>
        ///     Treat <typeparamref name="TSource" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Maybe{T}" /> with LINQ's API.
        /// </summary>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IMaybe<TSource> source) {
            if (source.HasValue)
                yield return source.Value;
        }

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" /> into an <see cref="IEnumerable{T}" /> with the
        ///     value of the <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type inside the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A sequence which can contain 0-n amount of values.
        /// </returns>
        public static IEnumerable<TSource> Values<TSource>(this IEnumerable<IMaybe<TSource>> source) =>
            source.SelectMany(x => x.ToEnumerable());
    }
}