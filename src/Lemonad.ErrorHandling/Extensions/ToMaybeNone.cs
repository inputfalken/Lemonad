using System;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions {
    public static partial class Index {
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
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource item) => ErrorHandling.Maybe.None<TSource>();

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
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource source, Func<TSource, bool> predicate) =>
            predicate is null
                ? throw new ArgumentNullException(nameof(predicate))
                : predicate(source)
                    ? ErrorHandling.Maybe.None<TSource>()
                    : ErrorHandling.Maybe.Value(source);
    }
}