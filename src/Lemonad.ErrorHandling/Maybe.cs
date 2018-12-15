using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Provides a set of static methods for <see cref="IMaybe{T}" />.
    /// </summary>
    public static class Maybe {
        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have no value.
        /// </summary>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> who will have no value.
        /// </returns>
        public static IMaybe<TSource> None<TSource>() => Maybe<TSource>.None;

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have the value <paramref name="item" />.
        /// </summary>
        /// <param name="item">
        ///     The value of <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> whose value will be <paramref name="item" />.
        /// </returns>
        public static IMaybe<TSource> Value<TSource>(TSource item) => Maybe<TSource>.Create(item);
    }
}