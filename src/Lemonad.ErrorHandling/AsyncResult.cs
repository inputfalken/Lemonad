using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Either;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    public static class AsyncResult {
        public static TaskAwaiter<IEither<T, TError>> GetAwaiter<T, TError>(this IAsyncResult<T, TError> result) =>
            result.Either.GetAwaiter();

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
        public static Task<T> Match<T, TError>(this IAsyncResult<T, TError> source) where TError : T =>
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
        public static Task<TResult> Match<T, TResult, TError>(this IAsyncResult<T, TError> source,
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
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<IResult<T, TError>> result) =>
            AsyncResult<T, TError>.Factory(result);

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
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this IResult<T, TError> result) =>
            ToAsyncResult(Task.FromResult(result));

        [Pure]
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T> source, Func<T, bool> predicate,
            Func<Maybe<T>, TError> errorSelector) {
            async Task<IResult<T, TError>> Factory(Task<T> x, Func<T, bool> y, Func<Maybe<T>, TError> z) =>
                (await x.ConfigureAwait(false)).ToResult(y, z);

            return AsyncResult<T, TError>.Factory(Factory(source, predicate, errorSelector));
        }

        [Pure]
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T?> source,
            Func<TError> errorSelector)
            where T : struct {
            async Task<IResult<T, TError>> Factory(Task<T?> x, Func<TError> y) =>
                (await x.ConfigureAwait(false)).ToResult(y);

            return AsyncResult<T, TError>.Factory(Factory(source, errorSelector));
        }

        [Pure]
        public static IAsyncResult<T, TError> ToAsyncResultError<T, TError>(this Task<TError> source,
            Func<TError, bool> predicate,
            Func<TError, T> valueSelector) {
            async Task<IResult<T, TError>> Factory(Task<TError> x, Func<TError, bool> y, Func<TError, T> z) =>
                (await x.ConfigureAwait(false)).ToResultError(y, z);

            return AsyncResult<T, TError>.Factory(Factory(source, predicate, valueSelector));
        }

        /// <inheritdoc cref="ToEnumerable{T,TError}" />
        public static async Task<IEnumerable<T>> ToEnumerable<T, TError>(this IAsyncResult<T, TError> result) =>
            EitherMethods.YieldValues(await result.Either.ConfigureAwait(false));

        /// <inheritdoc cref="ToErrorEnumerable{T,TError}" />
        public static async Task<IEnumerable<TError>>
            ToErrorEnumerable<T, TError>(this IAsyncResult<T, TError> result) =>
            EitherMethods.YieldErrors(await result.Either.ConfigureAwait(false));
    }
}