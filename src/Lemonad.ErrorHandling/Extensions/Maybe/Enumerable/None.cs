using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling.Extensions.Maybe.Enumerable {
    public static partial class MaybeEnumerable {
        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="IMaybe{T}" /> into an <see cref="IEnumerable{T}" /> of
        ///     <typeparamref name="TResult" /> for each element which do not have a value.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="IMaybe{T}" />.
        /// </param>
        /// <param name="selector">
        ///     A function to return a value for each <see cref="IMaybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IMaybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type returned by the function <paramref name="selector" />.
        /// </typeparam>
        public static IEnumerable<TResult> None<TSource, TResult>(
            this IEnumerable<IMaybe<TSource>> source,
            Func<TResult> selector
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return source.Where(x => x.HasValue == false).Select(_ => selector());
        }
    }
}