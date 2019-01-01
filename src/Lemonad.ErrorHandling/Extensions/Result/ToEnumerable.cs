using System;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Treat <typeparamref name="T" /> as enumerable with 0-1 elements.
        ///     This is handy when combining <see cref="IResult{T,TError}" /> with LINQ's API.
        /// </summary>
        /// <param name="source"></param>
        public static IEnumerable<T> ToEnumerable<T, TError>(this IResult<T, TError> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            // This extra method is needed in order to perform exception check.
            return YieldValue(source);
        }

        private static IEnumerable<T> YieldValue<T, TError>(IResult<T, TError> source) {
            if (source.Either.HasValue)
                yield return source.Either.Value;
        }
    }
}