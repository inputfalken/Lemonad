using System;

namespace Lemonad.ErrorHandling.Extensions {
    public static partial class Index {
        /// <summary>
        ///     Converts an <see cref="Nullable{T}" /> to an <see cref="IResult{T,TError}" /> with the value
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Nullable{T}" /> to convert.
        /// </param>
        /// <param name="errorSelector">
        ///     A function who returns <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the <see cref="Nullable{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type returned by the <paramref name="errorSelector" /> function.
        /// </typeparam>
        public static IResult<T, TError> ToResult<T, TError>(this T? source, Func<TError> errorSelector)
            where T : struct => source.ToResult(
                x => x.HasValue,
                x => errorSelector is null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : errorSelector()
            )
            .Map(x => x.Value);

        /// <summary>
        ///     Creates an <see cref="IResult{T,TError}" /> based on a predicate function combined with a
        ///     <paramref name="errorSelector" /> for the <see cref="TError" /> type.
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
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed when the predicate fails.
        /// </param>
        /// <returns></returns>
        public static IResult<T, TError> ToResult<T, TError>(
            this T source,
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return predicate(source)
                ? ErrorHandling.Result.Value<T, TError>(source)
                : ErrorHandling.Result.Error<T, TError>(errorSelector(source));
        }
    }
}