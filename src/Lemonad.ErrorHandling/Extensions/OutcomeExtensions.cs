using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions {
    public static class OutcomeExtensions {
        /// <summary>
        ///     Converts the <see cref="Task" /> with <see cref="Result{T,TError}" /> into <see cref="Outcome{T,TError}" /> with
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
        public static Outcome<T, TError> AsOutcome<T, TError>(this Task<Result<T, TError>> source) => source;

        public static TaskAwaiter<Result<T, TError>> GetAwaiter<T, TError>(this Outcome<T, TError> outcome) =>
            outcome.Result.GetAwaiter();

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
        public static Task<T> Match<T, TError>(this Outcome<T, TError> source) where TError : T =>
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
        public static Task<TResult> Match<T, TResult, TError>(this Outcome<T, TError> source, Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        [Pure]
        public static Outcome<T, TError> ToOutcome<T, TError>(this Task<T> source, Func<T, bool> predicate,
            Func<TError> errorSelector) {
            async Task<Result<T, TError>> Factory(Task<T> x, Func<T, bool> y, Func<TError> z) =>
                (await x.ConfigureAwait(false)).ToResult(y, z);

            return Factory(source, predicate, errorSelector);
        }

        [Pure]
        public static Outcome<T, TError> ToOutcome<T, TError>(this Task<T?> source, Func<TError> errorSelector)
            where T : struct {
            async Task<Result<T, TError>> Factory(Task<T?> x, Func<TError> y ) => (await x.ConfigureAwait(false)).ToResult(errorSelector);
            return Factory(source, errorSelector);
        }

        public static Outcome<T, TError> ToOutcome<T, TError>(this Task<T> source) => source;

        /// <summary>
        ///     Creates an <see cref="Outcome{T,TError}" /> with the value <typeparamref name="T" /> and a <see cref="Object" /> as
        ///     the error type.
        /// </summary>
        /// <param name="source">
        ///     The <typeparamref name="T" /> to convert.
        /// </param>
        /// <typeparam name="T">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        [Pure]
        public static Outcome<T, Unit> ToOutcome<T>(this Task<T> source) => source;

        [Pure]
        public static Outcome<T, TError> ToOutcomeError<T, TError>(this Task<TError> source) => source;

        /// <summary>
        ///     Creates an <see cref="Outcome{T,TError}" /> with the error <typeparamref name="TError" />.
        /// </summary>
        /// <param name="source">
        ///     The <typeparamref name="TError" /> to convert.
        /// </param>
        /// <typeparam name="TError">
        ///     The type of the <paramref name="source" />.
        /// </typeparam>
        [Pure]
        public static Outcome<Unit, TError> ToOutcomeError<TError>(this Task<TError> source) => source;
    }
}