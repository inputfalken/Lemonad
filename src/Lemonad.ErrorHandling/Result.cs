using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
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
        public static IResult<T, TError> Error<T, TError>(TError error)
            => Result<T, TError>.ErrorFactory(in error);

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
        public static T Match<T, TError>(this IResult<T, TError> source) where TError : T
            => source.Match(x => x, x => x);

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
        public static IResult<T, IReadOnlyList<TError>> Multiple<T, TError>(
            this IResult<T, TError> source,
            params Func<IResult<T, TError>, IResult<T, TError>>[] validations
        ) {
            var either = EitherMethods.Multiple(
                source.Either,
                validations.Select(x => x.Compose(y => y.Either)(source)
                )
            );
            return either.HasValue
                ? Value<T, IReadOnlyList<TError>>(either.Value)
                : Error<T, IReadOnlyList<TError>>(either.Error);
        }

        /// <summary>
        ///     Treat <typeparamref name="T" /> as enumerable with 0-1 elements.
        ///     This is handy when combining <see cref="IResult{T,TError}" /> with LINQ's API.
        /// </summary>
        /// <param name="result"></param>
        public static IEnumerable<T> ToEnumerable<T, TError>(this IResult<T, TError> result) {
            if (result.Either.HasValue)
                yield return result.Either.Value;
        }

        /// <summary>
        ///     Treat <typeparamref name="TError" /> as enumerable with 0-1 elements.
        ///     This is handy when combining <see cref="IResult{T,TError}" /> with LINQs API.
        /// </summary>
        /// <param name="result"></param>
        public static IEnumerable<TError> ToErrorEnumerable<T, TError>(this IResult<T, TError> result) {
            if (result.Either.HasError)
                yield return result.Either.Error;
        }

        /// <summary>
        ///     Converts an <see cref="IMaybe{T}" /> to an <see cref="IResult{T,TError}" /> with the value
        ///     <typeparamref name="T" />
        ///     .
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IMaybe{T}" /> to convert.
        /// </param>
        /// <typeparam name="T">
        ///     The type in the <see cref="IMaybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> from the <see cref="IResult{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static IMaybe<T> ToMaybe<T, TError>(this IResult<T, TError> source)
            => source.Either.HasValue ? Maybe.Value(source.Either.Value) : Maybe.None<T>();

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
            where T : struct => source.ToResult(
                x => x.HasValue,
                x => errorSelector == null
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
        [Pure]
        public static IResult<T, TError> ToResult<T, TError>(
            this T source,
            Func<T, bool> predicate,
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
        public static IResult<T, TError> ToResultError<T, TError>(
            this TError source,
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

        public static IAsyncResult<TResult, TError> MapAsync<T, TError, TResult>(
            this IResult<T, TError> source,
            Func<T, Task<TResult>> selector
        ) => source.ToAsyncResult().Map(selector);

        public static IAsyncResult<T, TError> FilterAsync<T, TError>(
            this IResult<T, TError> source,
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector
        ) => source.ToAsyncResult().Filter(predicate, errorSelector);

        public static IAsyncResult<TResult, TError> FlatMapAsync<T, TResult, TError>(
            this IResult<T, TError> source,
            Func<T, IAsyncResult<TResult, TError>> flatSelector
        ) => source.ToAsyncResult().FlatMap(flatSelector);

        public static IAsyncResult<TResult, TError> FlatMapAsync<T, TSelector, TResult, TError>(
            this IResult<T, TError> source,
            Func<T, IAsyncResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector
        ) => source.ToAsyncResult().FlatMap(flatSelector, resultSelector);

        public static IAsyncResult<TResult, TError> Zip<T, TOther, TResult, TError>(
            this IResult<T, TError> source,
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        ) => source.ToAsyncResult().Zip(other, resultSelector);

        public static IAsyncResult<T, TError> FlattenAsync<T, TResult, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<T, IAsyncResult<T, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) => source.ToAsyncResult().Flatten(selector, errorSelector);

        public static IAsyncResult<T, TError> FlattenAsync<T, TResult, TError>(
            this IResult<T, TError> source,
            Func<T, IAsyncResult<TResult, TError>> selector
        ) => source.ToAsyncResult().Flatten(selector);

        public static IAsyncResult<T, TErrorResult> MapErrorAsync<T, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<TError, Task<TErrorResult>> selector
        ) => source.ToAsyncResult().MapError(selector);

        public static IAsyncResult<T, TError> IsErrorWhenAsync<T, TError>(
            this IResult<T, TError> source,
            Func<T, TError> errorSelector,
            Func<T, Task<bool>> predicate
        ) => source.ToAsyncResult().IsErrorWhen(predicate, errorSelector);

        public static IAsyncResult<TResult, TErrorResult> FullMapAsync<T, TResult, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) => source.ToAsyncResult().FullMap(selector, errorSelector);

        public static IAsyncResult<TResult, TErrorResult> FullMapAsync<T, TResult, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) => source.ToAsyncResult().FullMap(selector, errorSelector);

        public static IAsyncResult<TResult, TErrorResult> FullMapAsync<T, TResult, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) => source.ToAsyncResult().FullMap(selector, errorSelector);

        public static IAsyncResult<T, TError> DoAsync<T, TError>(
            this IResult<T, TError> source,
            Action action
        ) => source.ToAsyncResult().Do(action);

        public static IAsyncResult<T, TError> DoWithAsync<T, TError>(
            this IResult<T, TError> source,
            Action<T> action
        ) => source.ToAsyncResult().DoWith(action);

        public static IAsyncResult<T, TError> DoWithErrorAsync<T, TError>(
            this IResult<T, TError> source,
            Action<TError> action
        ) => source.ToAsyncResult().DoWithError(action);

        public static IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<T, TFlatMap, TResult, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) => source.ToAsyncResult().FullFlatMap(flatMapSelector, resultSelector, errorSelector);

        public static IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<T, TResult, TError, TErrorResult>(
            this IResult<T, TError> source,
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector
        ) => source.ToAsyncResult().FullFlatMap(flatMapSelector, errorSelector);

        public static IAsyncResult<TResult, TError> ZipAsync<T, TOther, TResult, TError>(
            this IResult<T, TError> source,
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        ) => source.ToAsyncResult().Zip(other, resultSelector);
    }
}
