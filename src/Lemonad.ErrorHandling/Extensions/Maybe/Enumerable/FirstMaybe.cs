using System;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling.Extensions.Maybe.Enumerable {
    public static partial class MaybeEnumerable {
        /// <summary>
        ///     Works just like <see cref="System.Linq.Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
        ///     but returns a <see cref="IMaybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IEnumerable{T}" />.
        /// </typeparam>
        /// <returns>
        ///     Returns the first element of a sequence inside a <see cref="IMaybe{T}" />.
        /// </returns>
        public static IMaybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source) {
            switch (source) {
                case IList<TSource> list when list.Count > 0:
                    return ErrorHandling.Maybe.Value(list[0]);
                case IReadOnlyList<TSource> readOnlyList when readOnlyList.Count > 0:
                    return ErrorHandling.Maybe.Value(readOnlyList[0]);
                default:
                    using (var e = source.GetEnumerator()) {
                        return e.MoveNext()
                            ? ErrorHandling.Maybe.Value(e.Current)
                            : ErrorHandling.Maybe.None<TSource>();
                    }
            }
        }

        /// <summary>
        ///     Works just like <see cref="System.Linq.Enumerable.FirstOrDefault{TSource}(System.Collections.Generic.IEnumerable{TSource})" />
        ///     but returns a <see cref="IMaybe{T}" />.
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
        ///     <see cref="IMaybe{T}" />.
        /// </returns>
        public static IMaybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate) {
            foreach (var element in source)
                if (predicate(element))
                    return ErrorHandling.Maybe.Value(element);

            return ErrorHandling.Maybe.None<TSource>();
        }
    }
}