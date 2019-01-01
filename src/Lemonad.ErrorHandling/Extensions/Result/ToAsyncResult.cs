using System;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static partial class Index {
        /// <summary>
        ///     Converts a <see cref="IResult{T,TError}" /> into a <see cref="IAsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The  <see cref="IAsyncResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this IResult<T, TError> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : Task.Index.ToAsyncResult(System.Threading.Tasks.Task.FromResult(source));
    }
}