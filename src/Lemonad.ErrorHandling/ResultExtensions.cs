using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    public static class ResultExtensions {
        /// <summary>
        ///     Creates a <see cref="Result{T,TError}" /> with <typeparamref name="TError" />.
        /// </summary>
        /// <param name="error">
        ///     The type of the <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="Result{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static Result<T, TError> Error<T, TError>(TError error) => error;

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="Result{T,TError}" /> to an <see cref="IEnumerable{T}" /> of
        ///     <typeparamref name="TError" />.
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="Result{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the values in <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the errors in <see cref="Result{T,TError}" />.
        /// </typeparam>
        public static IEnumerable<TError> Errors<T, TError>(
            this IEnumerable<Result<T, TError>> enumerable) => enumerable.SelectMany(x => x.ToErrorEnumerable());

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Result{T,TError}" /> to evaluate.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the error.
        /// </typeparam>
        [Pure]
        public static T Match<T, TError>(this Result<T, TError> source) where TError : T =>
            source.Match(x => x, x => x);

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Result{T,TError}" /> to evaluate.
        /// </param>
        /// <param name="selector">
        ///     A function to map <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">
        ///     The value type of the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type of the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned from function <paramref name="selector" />>
        /// </typeparam>
        [Pure]
        public static TResult Match<T, TResult, TError>(this Result<T, TError> source, Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Result{T,TError}" /> to evaluate.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the error.
        /// </typeparam>
        public static Task<T> Match<T, TError>(this AsyncResult<T, TError> source) where TError : T =>
            source.Match(x => x, x => x);

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Result{T,TError}" /> to evaluate.
        /// </param>
        /// <param name="selector">
        ///     A function to map <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">
        ///     The value type of the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type of the <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned from function <paramref name="selector" />>
        /// </typeparam>
        [Pure]
        public static Task<TResult> Match<T, TResult, TError>(this AsyncResult<T, TError> source,
            Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        /// <summary>
        ///     Converts the <see cref="Task" /> with <see cref="Result{T,TError}" /> into <see cref="AsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="result">
        ///     The  <see cref="Result{T,TError}" /> wrapped in a <see cref="Task{TResult}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static AsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<Result<T, TError>> result) => result;

        /// <summary>
        ///     Converts an <see cref="Result{T,TError}" /> into an <see cref="AsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="result">
        ///     The  <see cref="Result{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static AsyncResult<T, TError> ToAsyncResult<T, TError>(this Result<T, TError> result) =>
            Task.FromResult(result);

        [Pure]
        public static AsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T> source, Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            async Task<Result<T, TError>> Factory(Task<T> x, Func<T, bool> y, Func<T, TError> z) =>
                (await x.ConfigureAwait(false)).ToResult(y, z);

            return Factory(source, predicate, errorSelector);
        }

        [Pure]
        public static AsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T?> source,
            Func<TError> errorSelector)
            where T : struct {
            async Task<Result<T, TError>> Factory(Task<T?> x, Func<TError> y) =>
                (await x.ConfigureAwait(false)).ToResult(y);

            return Factory(source, errorSelector);
        }

        public static AsyncResult<T, TError> ToAsyncResultError<T, TError>(this Task<TError> source,
            Func<TError, bool> predicate,
            Func<TError, T> valueSelector) {
            async Task<Result<T, TError>> Factory(Task<TError> x, Func<TError, bool> y, Func<TError, T> z) =>
                (await x.ConfigureAwait(false)).ToResultError(y, z);

            return Factory(source, predicate, valueSelector);
        }

        /// <summary>
        ///     Treat <typeparamref name="T" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Result{T,TError}" /> with LINQ's API.
        /// </summary>
        /// <param name="result"></param>
        public static IEnumerable<T> ToEnumerable<T, TError>(this Result<T, TError> result) => YieldValues(result);

        /// <inheritdoc cref="ToEnumerable{T,TError}(Result{T,TError})" />
        public static async Task<IEnumerable<T>> ToEnumerable<T, TError>(this AsyncResult<T, TError> result) =>
            (await result.TaskResult.ConfigureAwait(false)).ToEnumerable();

        /// <summary>
        ///     Treat <typeparamref name="TError" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Result{T,TError}" /> with LINQs API.
        /// </summary>
        /// <param name="result"></param>
        public static IEnumerable<TError> ToErrorEnumerable<T, TError>(this Result<T, TError> result) =>
            YieldErrors(result);

        /// <inheritdoc cref="ToErrorEnumerable{T,TError}(Result{T,TError})" />
        public static async Task<IEnumerable<TError>>
            ToErrorEnumerable<T, TError>(this AsyncResult<T, TError> result) =>
            (await result.TaskResult.ConfigureAwait(false)).ToErrorEnumerable();

        /// <summary>
        ///     Converts an <see cref="Maybe{T}" /> to an <see cref="Result{T,TError}" /> with the value <typeparamref name="T" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="Maybe{T}" /> to convert.
        /// </param>
        /// <typeparam name="T">
        ///     The type in the <see cref="Maybe{T}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> from the <see cref="Result{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static Maybe<T> ToMaybe<T, TError>(this Result<T, TError> source) =>
            source.Either.HasValue ? source.Either.Value : Maybe<T>.None;

        /// <summary>
        ///     Converts an <see cref="Nullable{T}" /> to an <see cref="Result{T,TError}" /> with the value
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
        public static Result<T, TError> ToResult<T, TError>(this T? source, Func<TError> errorSelector)
            where T : struct =>
            // ReSharper disable once PossibleInvalidOperationException
            source.ToResult(x => x.HasValue, x => errorSelector == null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : errorSelector())
                .Map(x => x.Value);

        /// <summary>
        ///     Creates an <see cref="Result{T,TError}" /> based on a predicate function combined with an
        ///     <paramref name="errorSelector" /> for <typeparamref name="TError" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type in <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type in the <see cref="Result{T,TError}" />.
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
        public static Result<T, TError> ToResult<T, TError>(this T source, Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return predicate(source)
                ? Value<T, TError>(source)
                : errorSelector == null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : Error<T, TError>(errorSelector(source));
        }

        /// <summary>
        ///     Creates an <see cref="Result{T,TError}" /> based on a predicate function combined with an
        ///     <paramref name="valueSelector" /> for <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">
        ///     The value type in <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type in the <see cref="Result{T,TError}" />.
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
        public static Result<T, TError> ToResultError<T, TError>(this TError source,
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
        ///     Creates a <see cref="Result{T,TError}" /> with <typeparamref name="T" />.
        /// </summary>
        /// <param name="element">
        ///     The type of the <typeparamref name="T" />.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="Result{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static Result<T, TError> Value<T, TError>(T element) => element;

        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> of <see cref="Result{T,TError}" /> to an <see cref="IEnumerable{T}" /> of
        ///     <typeparamref name="T" />.
        /// </summary>
        /// <param name="enumerable">
        ///     The <see cref="IEnumerable{T}" /> of <see cref="Result{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the values in <see cref="Result{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The type of the errors in <see cref="Result{T,TError}" />.
        /// </typeparam>
        public static IEnumerable<T> Values<T, TError>(
            this IEnumerable<Result<T, TError>> enumerable) =>
            enumerable.SelectMany(x => x.ToEnumerable());

        private static IEnumerable<TError> YieldErrors<T, TError>(Result<T, TError> result) {
            if (result.Either.HasError)
                yield return result.Either.Error;
        }

        private static IEnumerable<T> YieldValues<T, TError>(Result<T, TError> result) {
            if (result.Either.HasValue)
                yield return result.Either.Value;
        }

        internal static Maybe<TSource> NullCheckedMaybe<TSource>(TSource source) =>
            source.IsNull() ? Maybe<TSource>.None : source.ToMaybe();
    }
}