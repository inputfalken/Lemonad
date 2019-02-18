using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Maybe;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.AsyncMaybe {
    public static partial class Index {
        /// <summary>
        ///     Treat <typeparamref name="TSource" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Maybe{T}" /> with LINQ's API.
        /// </summary>
        public static async Task<IEnumerable<TSource>> ToEnumerable<TSource>(this IAsyncMaybe<TSource> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            var maybe = await source;
            return maybe.ToEnumerable();
        }
    }
}