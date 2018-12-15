using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling.Extensions.Maybe.Enumerable {
    public static partial class MaybeEnumerable {
        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="IMaybe{T}" /> into an <see cref="IEnumerable{T}" /> with
        ///     the
        ///     value of the <see cref="IMaybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="IMaybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type inside the <see cref="IMaybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A sequence which can contain 0-n amount of values.
        /// </returns>
        public static IEnumerable<TSource> Values<TSource>(this IEnumerable<IMaybe<TSource>> source) =>
            source.SelectMany(x => x.ToEnumerable());
    }
}