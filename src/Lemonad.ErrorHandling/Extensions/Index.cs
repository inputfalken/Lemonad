using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Task;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions {
    public static class Index {
        /// <summary>
        ///     Works like <see cref="Maybe.Value{TSource}" /> but with an <paramref name="predicate" /> to test the element.
        /// </summary>
        /// <param name="source">
        ///     The element to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test the element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource source, Func<TSource, bool> predicate)
            => predicate is null
                ? throw new ArgumentNullException(nameof(predicate))
                : predicate(source)
                    ? ErrorHandling.Maybe.Value(source)
                    : ErrorHandling.Maybe.None<TSource>();

        /// <summary>
        ///     Converts an <see cref="Nullable{T}" /> to an <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="source">
        ///     The element from the <see cref="Nullable{T}" /> to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybe<TSource>(this TSource? source) where TSource : struct
            => source.HasValue
                ? ErrorHandling.Maybe.Value(source.Value)
                : ErrorHandling.Maybe.None<TSource>();

        /// <summary>
        ///     Creates a <see cref="Maybe{T}" /> who will have no value.
        /// </summary>
        /// <param name="item">
        ///     The value that will be considered to not have a value.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> who will have no value.
        /// </returns>
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource item) => ErrorHandling.Maybe.None<TSource>();

        /// <summary>
        ///     Works like <see cref="ToMaybeNone{TSource}(TSource)" /> but with an <paramref name="predicate" /> to test the
        ///     element.
        /// </summary>
        /// <param name="source">
        ///     The element to be passed into <see cref="Maybe{T}" />.
        /// </param>
        /// <param name="predicate">
        ///     A function to test the element.
        /// </param>
        /// <typeparam name="TSource">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        public static IMaybe<TSource> ToMaybeNone<TSource>(this TSource source, Func<TSource, bool> predicate) =>
            predicate is null
                ? throw new ArgumentNullException(nameof(predicate))
                : predicate(source)
                    ? ErrorHandling.Maybe.None<TSource>()
                    : ErrorHandling.Maybe.Value(source);


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