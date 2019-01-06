using System;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        /// Swaps the position of <typeparamref name="T"/> and <typeparamref name="TError"/>>
        /// </summary>
        /// <param name="source">
        /// The <see cref="IResult{T,TError}"/> to perform the swap on.
        /// </param>
        /// <typeparam name="T">
        /// The <typeparamref name="T"/> of the <paramref name="source"/> parameter.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The <typeparamref name="TError"/> of the <paramref name="source"/> parameter.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="source"/> is null.
        /// </exception>
        public static IResult<TError, T> Swap<T, TError>(this IResult<T, TError> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return source.Match(ErrorHandling.Result.Error<TError, T>, ErrorHandling.Result.Value<TError, T>);
        }
    }
}