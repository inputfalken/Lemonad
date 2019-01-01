using System;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.Maybe {
    public static partial class Index {
        /// <summary>
        ///     Converts an <see cref="Maybe{T}" /> to an <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Maybe{T}" /> to be passed into <see cref="Result{T,TError}" />.
        /// </param>
        /// <param name="errorSelector">
        ///     A function to be executed if there is no value inside the <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type inside the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type representing an error for the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        public static IResult<T, TError> ToResult<T, TError>(this IMaybe<T> source, Func<TError> errorSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return errorSelector is null
                ? throw new ArgumentNullException(nameof(errorSelector))
                : source.ToResult(x => x.HasValue, x => errorSelector()).Map(x => x.Value);
        }
    }
}