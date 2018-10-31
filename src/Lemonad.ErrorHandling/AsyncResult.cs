using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;
using Lemonad.ErrorHandling.Internal.TaskExtensions;

namespace Lemonad.ErrorHandling {
    public static class AsyncResult {
        // This could be used for mapping.
        private static async Task<IEither<T, TError>> Map<T, TError>(IEitherAsync<T, TError> left) {
            var hasValue = await left.HasValue;
            return hasValue ? Result.Value<T, TError>(left.Value).Either : Result.Error<T, TError>(left.Error).Either;
        }
        
        /// <summary>
        ///     Creates a <see cref="IAsyncResult{T,TError}" /> with <typeparamref name="TError" />.
        /// </summary>
        /// <param name="error">
        ///     The type of the <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static IAsyncResult<T, TError> Error<T, TError>(TError error) =>
            AsyncResult<T, TError>.ErrorFactory(in error);

        public static TaskAwaiter<IEither<T, TError>> GetAwaiter<T, TError>(this IAsyncResult<T, TError> result) =>
            result.Either.GetAwaiter();

        /// <summary>
        ///     Evaluates the <see cref="IAsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncResult{T,TError}" /> to evaluate.
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
        ///     Evaluates the <see cref="IAsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncResult{T,TError}" /> to evaluate.
        /// </param>
        /// <param name="selector">
        ///     A function to map <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </param>
        /// <typeparam name="T">
        ///     The value type of the <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The error type of the <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned from function <paramref name="selector" />>
        /// </typeparam>
        [Pure]
        public static Task<TResult> Match<T, TResult, TError>(this IAsyncResult<T, TError> source,
            Func<T, TResult> selector)
            where T : TError => source.Match(selector, x => selector((T) x));

        public static IAsyncResult<T, IReadOnlyList<TError>> Multiple<T, TError>(this IAsyncResult<T, TError> source,
            params Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>>[] validations) =>
            AsyncResult<T, IReadOnlyList<TError>>.Factory(EitherMethods.MultipleAsync(source.Either,
                validations.Select(x => x.Compose(y => y.Either)(source)).ToArray()));

        /// <summary>
        ///     Converts the <see cref="Task" /> with <see cref="IAsyncResult{T,TError}" /> into
        ///     <see cref="IAsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="result">
        ///     The  <see cref="IAsyncResult{T,TError}" /> wrapped in a <see cref="Task{TResult}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<IResult<T, TError>> result) =>
            AsyncResult<T, TError>.Factory(result.Map(x => x.Either));

        /// <summary>
        ///     Converts an <see cref="IAsyncResult{T,TError}" /> into an <see cref="IAsyncResult{T,TError}" />.
        /// </summary>
        /// <param name="result">
        ///     The  <see cref="IAsyncResult{T,TError}" />.
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
            Func<T, TError> errorSelector) =>
            AsyncResult<T, TError>.Factory(source.Map(x => x.ToResult(predicate, errorSelector).Either));

        [Pure]
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T?> source,
            Func<TError> errorSelector)
            where T : struct => AsyncResult<T, TError>.Factory(source.Map(x => x.ToResult(errorSelector).Either));

        [Pure]
        public static IAsyncResult<T, TError> ToAsyncResultError<T, TError>(this Task<TError> source,
            Func<TError, bool> predicate,
            Func<TError, T> valueSelector) =>
            AsyncResult<T, TError>.Factory(source.Map(x => x.ToResultError(predicate, valueSelector).Either));

        /// <inheritdoc cref="ToEnumerable{T,TError}" />
        public static async Task<IEnumerable<T>> ToEnumerable<T, TError>(this IAsyncResult<T, TError> result) =>
            EitherMethods.YieldValues(await result.Either.ConfigureAwait(false));

        /// <inheritdoc cref="ToErrorEnumerable{T,TError}" />
        public static async Task<IEnumerable<TError>>
            ToErrorEnumerable<T, TError>(this IAsyncResult<T, TError> result) =>
            EitherMethods.YieldErrors(await result.Either.ConfigureAwait(false));

        /// <summary>
        ///     Creates a <see cref="IAsyncResult{T,TError}" /> with <typeparamref name="T" />.
        /// </summary>
        /// <param name="element">
        ///     The type of the <typeparamref name="T" />.
        /// </param>
        /// <typeparam name="T">
        ///     The <typeparamref name="T" /> of <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        [Pure]
        public static IAsyncResult<T, TError> Value<T, TError>(T element) =>
            AsyncResult<T, TError>.ValueFactory(in element);
    }
}