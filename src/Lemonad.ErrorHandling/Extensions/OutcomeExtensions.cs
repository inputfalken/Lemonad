using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions {
    public static class OutcomeExtensions {
        /// <summary>
        ///     Converts the <see cref="Task" /> with <see cref="Result{T,TError}" /> into <see cref="AsyncResult{T,TError}" /> with
        ///     same functionality as <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The  <see cref="Result{T,TError}" /> wrapped in a <see cref="Task{TResult}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static AsyncResult<T, TError> AsOutcome<T, TError>(this Task<Result<T, TError>> source) => source;

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
        public static Task<TResult> Match<T, TResult, TError>(this AsyncResult<T, TError> source, Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        [Pure]
        public static AsyncResult<T, TError> ToOutcome<T, TError>(this Task<T> source, Func<T, bool> predicate,
            Func<TError> errorSelector) {
            async Task<Result<T, TError>> Factory(Task<T> x, Func<T, bool> y, Func<TError> z) =>
                (await x.ConfigureAwait(false)).ToResult(y, z);

            return Factory(source, predicate, errorSelector);
        }

        [Pure]
        public static AsyncResult<T, TError> ToOutcome<T, TError>(this Task<T?> source, Func<TError> errorSelector)
            where T : struct {
            async Task<Result<T, TError>> Factory(Task<T?> x, Func<TError> y) =>
                (await x.ConfigureAwait(false)).ToResult(errorSelector);

            return Factory(source, errorSelector);
        }

        public static AsyncResult<T, TError> ToOutcome<T, TError>(this Task<T> source) => source;

        [Pure]
        public static AsyncResult<T, TError> ToOutcomeError<T, TError>(this Task<TError> source) => source;
    }
}