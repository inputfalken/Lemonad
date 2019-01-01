using System;
using System.Collections.Generic;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.Maybe {
    public static partial class Index {
        /// <summary>
        ///     Treat <typeparamref name="TSource" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Maybe{T}" /> with LINQ's API.
        /// </summary>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this IMaybe<TSource> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (source.HasValue)
                yield return source.Value;
        }
    }
}