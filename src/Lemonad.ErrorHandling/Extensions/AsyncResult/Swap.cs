using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Result.Task;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        private static async Task<IResult<TError, T>> Resolve<T, TError>(IAsyncResult<T, TError> source)
            => await source.Either.HasValue.ConfigureAwait(false)
                ? ErrorHandling.Result.Error<TError, T>(source.Either.Value)
                : ErrorHandling.Result.Value<TError, T>(source.Either.Error);

        /// <summary>
        ///     Swaps the position of <typeparamref name="T" /> and <typeparamref name="TError" />>
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncResult{T,TError}" /> to perform the swap on.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of the <paramref name="source" /> parameter.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of the <paramref name="source" /> parameter.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When <paramref name="source" /> is null.
        /// </exception>
        public static IAsyncResult<TError, T> Swap<T, TError>(this IAsyncResult<T, TError> source) {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return Resolve(source).ToAsyncResult();
        }
    }
}