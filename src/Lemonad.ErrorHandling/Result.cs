using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling {
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
        [Pure]
        public static IResult<T, TError> Error<T, TError>(TError error) =>
            Result<T, TError>.ErrorFactory(in error);

        /// <summary>
        ///     Evaluates the <see cref="IResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IResult{T,TError}" /> to evaluate.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the error.
        /// </typeparam>
        [Pure]
        public static T Match<T, TError>(this IResult<T, TError> source) where TError : T =>
            source.Match(x => x, x => x);

        /// <summary>
        ///     Evaluates the <see cref="IResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IResult{T,TError}" /> to evaluate.
        /// </param>
        /// <param name="selector">
        ///     A function to map <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">
        ///     The value type of the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type of the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned from function <paramref name="selector" />>
        /// </typeparam>
        [Pure]
        public static TResult Match<T, TResult, TError>(this IResult<T, TError> source, Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        /// <summary>
        ///     Executes each function and saves all potential errors to a list which will be the <typeparamref name="TError" />.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validations">
        ///     A <see cref="IReadOnlyList{T}" /> containing <typeparamref name="TError" />.
        /// </param>
        public static IResult<T, IReadOnlyList<TError>> Multiple<T, TError>(this IResult<T, TError> source,
            params Func<IResult<T, TError>, IResult<T, TError>>[] validations) {
            var either =
                EitherMethods.Multiple(source.Either, validations.Select(x => x.Compose(y => y.Either)(source)));
            return either.HasValue
                ? Value<T, IReadOnlyList<TError>>(either.Value)
                : Error<T, IReadOnlyList<TError>>(either.Error);
        }

        /// <summary>
        ///     Converts an <see cref="Maybe{T}" /> to an <see cref="IResult{T,TError}" /> with the value <typeparamref name="T" />
        ///     .
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Maybe{T}" /> to convert.
        /// </param>
        /// <typeparam name="T">
        ///     The type in the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> from the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static IMaybe<T> ToMaybe<T, TError>(this IResult<T, TError> source) =>
            source.Either.HasValue ? Maybe.Value(source.Either.Value) : Maybe.None<T>();

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
        [Pure]
        public static IResult<T, TError> ToResult<T, TError>(this T? source, Func<TError> errorSelector)
            where T : struct =>
            // ReSharper disable once PossibleInvalidOperationException
            source.ToResult(x => x.HasValue, x => errorSelector == null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : errorSelector())
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
        [Pure]
        public static IResult<T, TError> ToResult<T, TError>(this T source, Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(source)
                ? Value<T, TError>(source)
                : errorSelector == null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : Error<T, TError>(errorSelector(source));
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
        public static IResult<T, TError> ToResultError<T, TError>(this TError source,
            Func<TError, bool> predicate,
            Func<TError, T> valueSelector) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(source)
                ? Error<T, TError>(source)
                : valueSelector == null
                    ? throw new ArgumentNullException(nameof(valueSelector))
                    : Value<T, TError>(valueSelector(source));
        }

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
        [Pure]
        public static IResult<T, TError> Value<T, TError>(T element) => Result<T, TError>.ValueFactory(in element);
    }
}