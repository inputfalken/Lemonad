using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling.EnumerableExtensions {
    public static class ResultEnumerable {
        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" /> to an <see cref="IEnumerable{T}" />
        ///     of
        ///     <typeparamref name="TError" />.
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the values in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the errors in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IEnumerable<TError> Errors<T, TError>(this IEnumerable<IResult<T, TError>> enumerable) =>
            enumerable.SelectMany(x => x.ToErrorEnumerable());

        /// <summary>
        ///     Treat <typeparamref name="T" /> as enumerable with 0-1 elements.
        ///     This is handy when combining <see cref="IResult{T,TError}" /> with LINQ's API.
        /// </summary>
        /// <param name="result"></param>
        public static IEnumerable<T> ToEnumerable<T, TError>(this IResult<T, TError> result) {
            if (result.Either.HasValue)
                yield return result.Either.Value;
        }

        /// <summary>
        ///     Treat <typeparamref name="TError" /> as enumerable with 0-1 elements.
        ///     This is handy when combining <see cref="IResult{T,TError}" /> with LINQs API.
        /// </summary>
        /// <param name="result"></param>
        public static IEnumerable<TError> ToErrorEnumerable<T, TError>(this IResult<T, TError> result) {
            if (result.Either.HasError)
                yield return result.Either.Error;
        }

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" /> to an <see cref="IEnumerable{T}" />
        ///     of
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="IResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the values in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the errors in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IEnumerable<T> Values<T, TError>(this IEnumerable<IResult<T, TError>> enumerable) =>
            enumerable.SelectMany(x => x.ToEnumerable());
    }
}