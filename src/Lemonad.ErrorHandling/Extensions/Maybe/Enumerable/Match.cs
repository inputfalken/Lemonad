using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling.Extensions.Maybe.Enumerable {
    public static partial class MaybeEnumerable {
        /// <summary>
        ///     Executes <see cref="IMaybe{T}.Match" /> for each element in the sequence.
        /// </summary>
        public static IEnumerable<TResult> Match<TSource, TResult>(this IEnumerable<IMaybe<TSource>> source,
            Func<TSource, TResult> someSelector, Func<TResult> noneSelector) =>
            source.Select(x => x.Match(someSelector, noneSelector));
    }
}