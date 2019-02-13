using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions.AsyncMaybe {
    public static partial class Index {
        /// <summary>
        ///     Returns a <see cref="IMaybe{T}" /> when awaited.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IMaybe{T}" />.
        /// </param>
        /// <typeparam name="T">
        ///     of <see cref="IMaybe{T}" />.
        ///     The <typeparamref name="T" />
        /// </typeparam>
        /// <returns></returns>
        public static TaskAwaiter<IMaybe<T>> GetAwaiter<T>(this IAsyncMaybe<T> source) =>
            source is null
                ? throw new ArgumentNullException(nameof(source))
                : Mapper(source).GetAwaiter();

        private static async Task<IMaybe<T>> Mapper<T>(IAsyncMaybe<T> source) =>
            await source.HasValue.ConfigureAwait(false)
                ? ErrorHandling.Maybe.Value(source.Value)
                : ErrorHandling.Maybe.None<T>();
    }
}