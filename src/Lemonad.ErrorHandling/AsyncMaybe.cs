using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    public static class AsyncMaybe {
        /// <summary>
        ///     Creates a <see cref="IAsyncMaybe{T}" /> who will have no value.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IAsyncMaybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="IAsyncMaybe{T}" /> who will have no value.
        /// </returns>
        public static IAsyncMaybe<TSource> None<TSource>() => AsyncMaybe<TSource>.None;

        /// <summary>
        ///     Creates a <see cref="IAsyncMaybe{T}" /> who will have the value <paramref name="item" />.
        /// </summary>
        /// <param name="item">
        ///     The value of <see cref="IAsyncMaybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="IAsyncMaybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="IAsyncMaybe{T}" /> whose value will be <paramref name="item" />.
        /// </returns>
        public static IAsyncMaybe<TSource> Value<TSource>(TSource item) => AsyncMaybe<TSource>.Create(in item);
    }
}