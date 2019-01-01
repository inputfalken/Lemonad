using System;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Converts an <see cref="IMaybe{T}" /> to an <see cref="IResult{T,TError}" /> with the value
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IMaybe{T}" /> to convert.
        /// </param>
        /// <typeparam name="T">
        ///     The type in the <see cref="IMaybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> from the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IMaybe<T> ToMaybe<T, TError>(this IResult<T, TError> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : source.Either.HasValue
                    ? ErrorHandling.Maybe.Value(source.Either.Value)
                    : ErrorHandling.Maybe.None<T>();
    }
}