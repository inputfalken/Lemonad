using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling.Extensions.Maybe.Enumerable {
    public static partial class Index {
        /// <summary>
        ///     Executes <see cref="IMaybe{T}.Match" /> for each element in the sequence.
        /// </summary>
        public static IEnumerable<TResult> Match<TSource, TResult>(
            this IEnumerable<IMaybe<TSource>> source,
            Func<TSource, TResult> valueSelector,
            Func<TResult> noneSelector
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));
            if (noneSelector is null) throw new ArgumentNullException(nameof(noneSelector));
            return source.Select(x => x.Match(valueSelector, noneSelector));
        }
    }
}