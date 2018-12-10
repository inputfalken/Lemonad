using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling.EnumerableFunctions {
    /// <summary>
    ///     Contains extension methods for <see cref="IEnumerable{T}" />.
    /// </summary>
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