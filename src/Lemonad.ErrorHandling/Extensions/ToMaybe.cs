using System;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions {
    public static partial class Index {
        /// <summary>
        ///     Works like <see cref="Maybe.Value{TSource}" /> but with an <paramref name="predicate" /> to test the element.
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
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource source, Func<TSource, bool> predicate)
            => predicate is null
                ? throw new ArgumentNullException(nameof(predicate))
                : predicate(source)
                    ? ErrorHandling.Maybe.Value(source)
                    : ErrorHandling.Maybe.None<TSource>();

        /// <summary>
        ///     Converts an <see cref="Nullable{T}" /> to an <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Nullable{T}" /> to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource? source) where TSource : struct
            => source.HasValue
                ? ErrorHandling.Maybe.Value(source.Value)
                : ErrorHandling.Maybe.None<TSource>();
    }
}