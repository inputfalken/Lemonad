using System;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Treat <typeparamref name="TError" /> as enumerable with 0-1 elements.
        ///     This is handy when combining <see cref="IResult{T,TError}" /> with LINQs API.
        /// </summary>
        /// <param name="source"></param>
        public static IEnumerable<TError> ToErrorEnumerable<T, TError>(this IResult<T, TError> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            // This extra method is needed in order to perform exception check.
            return YieldError(source);
        }

        private static IEnumerable<TError> YieldError<T, TError>(IResult<T, TError> source) {
            if (source.Either.HasError)
                yield return source.Either.Error;
        }
    }
}