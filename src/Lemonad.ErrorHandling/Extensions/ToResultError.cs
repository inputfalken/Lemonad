using System;

namespace Lemonad.ErrorHandling.Extensions {
    public static partial class Index {
        /// <summary>
        ///     Creates an <see cref="IResult{T,TError}" /> based on a predicate function combined with an
        ///     <paramref name="valueSelector" /> for <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type in <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type in the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <param name="source">
        ///     The starting value which will be passed into the <paramref name="predicate" />function.
        /// </param>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="TError" />.
        /// </param>
        /// <param name="valueSelector">
        ///     Is executed when the predicate fails.
        /// </param>
        public static IResult<T, TError> ToResultError<T, TError>(
            this TError source,
            Func<TError, bool> predicate,
            Func<TError, T> valueSelector) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));
            return predicate(source)
                ? ErrorHandling.Result.Error<T, TError>(source)
                : ErrorHandling.Result.Value<T, TError>(valueSelector(source));
        }
    }
}