using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Provides a set of static methods for <see cref="IResult{T,TError}" />.
    /// </summary>
    public static class Result {
        /// <summary>
        ///     Creates a <see cref="IResult{T,TError}" /> with <typeparamref name="TError" />.
        /// </summary>
        /// <param name="error">
        ///     The type of the <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IResult<T, TError> Error<T, TError>(TError error)
            => Result<T, TError>.ErrorFactory(in error);

        /// <summary>
        ///     Creates a <see cref="IResult{T,TError}" /> with <typeparamref name="T" />.
        /// </summary>
        /// <param name="element">
        ///     The type of the <typeparamref name="T" />.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IResult{T,TError}" />.
        /// </typeparam>
        public static IResult<T, TError> Value<T, TError>(T element) => Result<T, TError>.ValueFactory(in element);
    }
}