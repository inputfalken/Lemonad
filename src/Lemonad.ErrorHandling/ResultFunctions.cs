using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public static class Result {
        /// <summary>
        /// Covnerts an <see cref="IEnumerable{T}"/> of <see cref="Result{T,TError}"/> to an <see cref="IEnumerable{T}"/> of <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="enumerable">
        /// The <see cref="IEnumerable{T}"/> of <see cref="Result{T,TError}"/>.
        /// </param>
        /// <typeparam name="T">
        /// The type of the values in <see cref="Result{T,TError}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type of the errors in <see cref="Result{T,TError}"/>.
        /// </typeparam>
        public static IEnumerable<TError> Errors<T, TError>(
            this IEnumerable<Result<T, TError>> enumerable) => enumerable.SelectMany(x => x.AsErrorEnumerable);

        /// <summary>
        /// Covnerts an <see cref="IEnumerable{T}"/> of <see cref="Result{T,TError}"/> to an <see cref="IEnumerable{T}"/> of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="enumerable">
        /// The <see cref="IEnumerable{T}"/> of <see cref="Result{T,TError}"/>.
        /// </param>
        /// <typeparam name="T">
        /// The type of the values in <see cref="Result{T,TError}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type of the errors in <see cref="Result{T,TError}"/>.
        /// </typeparam>
        public static IEnumerable<T> Values<T, TError>(
            this IEnumerable<Result<T, TError>> enumerable) => enumerable.SelectMany(x => x.AsEnumerable);

        /// <summary>
        /// Creates a <see cref="Result{T,TError}"/> with <typeparamref name="T"/>.
        /// </summary>
        /// <param name="element">
        /// The type of the <typeparamref name="T"/>.
        /// </param>
        /// <typeparam name="T">
        /// The <typeparamref name="T"/> of <see cref="Result{T,TError}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The <typeparamref name="TError"/> of <see cref="Result{T,TError}"/>.
        /// </typeparam>
        [Pure]
        public static Result<T, TError> Ok<T, TError>(T element) =>
            new Result<T, TError>(element, default, false, true);

        /// <summary>
        /// Creates a <see cref="Result{T,TError}"/> with <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="error">
        /// The type of the <typeparamref name="TError"/>.
        /// </param>
        /// <typeparam name="T">
        /// The <typeparamref name="T"/> of <see cref="Result{T,TError}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The <typeparamref name="TError"/> of <see cref="Result{T,TError}"/>.
        /// </typeparam>
        [Pure]
        public static Result<T, TError> Error<T, TError>(TError error) =>
            new Result<T, TError>(default, error, true, false);

        /// <summary>
        /// Evaluates the <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <param name="source">
        /// The <see cref="Result{T,TError}"/> to evaluate.
        /// </param>
        /// <typeparam name="T">
        /// The type of the value.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type of the error.
        /// </typeparam>
        [Pure]
        public static T Match<T, TError>(this Result<T, TError> source) where TError : T =>
            source.Match(x => x, x => x);

        /// <summary>
        /// Evaluates the <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <param name="source">
        /// The <see cref="Result{T,TError}"/> to evaluate.
        /// </param>
        /// <param name="selector">
        /// A function to map <typeparamref name="T"/> to <typeparamref name="TResult"/>.
        /// </param>
        /// <typeparam name="T">
        /// The value type of the <see cref="Result{T,TError}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The error type of the <see cref="Result{T,TError}"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type returned from function <paramref name="selector"/>>
        /// </typeparam>
        [Pure]
        public static TResult Match<T, TResult, TError>(this Result<T, TError> source, Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        /// <summary>
        /// Converts an <see cref="Nullable{T}"/> to an <see cref="Result{T,TError}"/> with the value <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source">
        /// The <see cref="Nullable{T}"/> to convert.
        /// </param>
        /// <param name="errorSelector">
        /// A function who returns <typeparamref name="TError"/>.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <see cref="Nullable{T}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type returned by the <paramref name="errorSelector"/> function.
        /// </typeparam>
        [Pure]
        public static Result<T, TError>
            ToResult<T, TError>(this T? source, Func<TError> errorSelector) where T : struct =>
            source.HasValue
                ? Ok<T, TError>(source.Value)
                : (errorSelector != null
                    ? Error<T, TError>(errorSelector())
                    : throw new ArgumentNullException(nameof(errorSelector)));

        /// <summary>
        /// Creates an <see cref="Result{T,TError}"/> with the value <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source">
        /// The <typeparamref name="T"/> to convert.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type of the error.
        /// </typeparam>
        [Pure]
        public static Result<T, TError> ToResult<T, TError>(this T source) => Ok<T, TError>(source);

        /// <summary>
        /// Creates an <see cref="Result{T,TError}"/> with the error <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="source">
        /// The  <typeparamref name="TError"/> to convert.
        /// </param>
        /// <typeparam name="T">
        /// The type of the <paramref name="source"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The type of the error.
        /// </typeparam>
        [Pure]
        public static Result<T, TError> ToResultError<T, TError>(this TError source) => Error<T, TError>(source);

        /// <summary>
        /// Converts an <see cref="Maybe{T}"/> to an <see cref="Result{T,TError}"/> with the value <typeparamref name="T"/>.
        /// </summary>
        /// <param name="source">
        /// The <see cref="Maybe{T}"/> to convert.
        /// </param>
        /// <typeparam name="T">
        /// The type in the <see cref="Maybe{T}"/>.
        /// </typeparam>
        /// <typeparam name="TError">
        /// The <typeparamref name="TError"/> from the <see cref="Result{T,TError}"/>.
        /// </typeparam>
        [Pure]
        public static Maybe<T> ToMaybe<T, TError>(this Result<T, TError> source) => source.HasValue ? source.Error.Some() : Maybe<T>.None;
    }
}