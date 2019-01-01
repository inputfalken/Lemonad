using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.TaskExtensions;

namespace Lemonad.ErrorHandling.Extensions.Result.Task {
    public static partial class Index {
        /// <summary>
        ///     Converts the <see cref="Task" /> with <see cref="IAsyncResult{T,TError}" /> into
        ///     <see cref="IAsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The  <see cref="IAsyncResult{T,TError}" /> wrapped in a <see cref="Task{TResult}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<IResult<T, TError>> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : AsyncResult<T, TError>.Factory(source.Map(x => x.Either));
    }
}