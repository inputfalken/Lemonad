using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        /// <summary>
        ///     Returns a <see cref="IResult{T,TError}" /> when awaited.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     of <see cref="IAsyncResult{T,TError}" />.
        ///     The <typeparamref name="T" />
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        public static TaskAwaiter<IResult<T, TError>> GetAwaiter<T, TError>(this IAsyncResult<T, TError> source) =>
            source is null
                ? throw new ArgumentNullException(nameof(source))
                : Mapper(source).GetAwaiter();

        /// <summary>
        ///     Returns a <see cref="IEither{T,TError}" /> when awaited.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncEither{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     of <see cref="IAsyncEither{T,TError}" />.
        ///     The <typeparamref name="T" />
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IAsyncEither{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        public static TaskAwaiter<IEither<T, TError>> GetAwaiter<T, TError>(this IAsyncEither<T, TError> source) =>
            source?.ToTaskEither().GetAwaiter() ?? throw new ArgumentNullException(nameof(source));

        private static async Task<IResult<T, TError>> Mapper<T, TError>(IAsyncResult<T, TError> source) =>
            await source.Either.HasValue.ConfigureAwait(false)
                ? ErrorHandling.Result.Value<T, TError>(source.Either.Value)
                : ErrorHandling.Result.Error<T, TError>(source.Either.Error);
    }
}