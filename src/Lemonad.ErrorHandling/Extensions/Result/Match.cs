using System;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Evaluates the <see cref="IResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IResult{T,TError}" /> to evaluate.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the error.
        /// </typeparam>
        public static T Match<T, TError>(this IResult<T, TError> source) where TError : T
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : source.Match(x => x, x => x);

        /// <summary>
        ///     Evaluates the <see cref="IResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IResult{T,TError}" /> to evaluate.
        /// </param>
        /// <param name="selector">
        ///     A function to map <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">
        ///     The value type of the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type of the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned from function <paramref name="selector" />>
        /// </typeparam>
        public static TResult Match<T, TResult, TError>(this IResult<T, TError> source, Func<T, TResult> selector)
            where T : TError {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return source.Match(selector, x => selector((T) x));
        }
    }
}