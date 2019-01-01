using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static class Index {
        /// <summary>
        ///     Returns a <see cref="IResult{T,TError}" /> when awaited.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncResult{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     of <see cref="IAsyncResult{T,TError}" />.
        ///     The <typeparamref name="T" />
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IAsyncResult{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        public static TaskAwaiter<IResult<T, TError>> GetAwaiter<T, TError>(this IAsyncResult<T, TError> source) =>
            source is null
                ? throw new ArgumentNullException(nameof(source))
                : Mapper(source).GetAwaiter();

        /// <summary>
        ///     Returns a <see cref="IEither{T,TError}" /> when awaited.
        /// </summary>
        /// <param name="source">
        ///     The <see cref="IAsyncEither{T,TError}" />.
        /// </param>
        /// <typeparam name="T">
        ///     of <see cref="IAsyncEither{T,TError}" />.
        ///     The <typeparamref name="T" />
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The <typeparamref name="TError" /> of <see cref="IAsyncEither{T,TError}" />.
        /// </typeparam>
        /// <returns></returns>
        public static TaskAwaiter<IEither<T, TError>> GetAwaiter<T, TError>(this IAsyncEither<T, TError> source) =>
            source?.ToTaskEither().GetAwaiter() ?? throw new ArgumentNullException(nameof(source));

        private static async Task<IResult<T, TError>> Mapper<T, TError>(IAsyncResult<T, TError> source) =>
            await source.Either.HasValue.ConfigureAwait(false)
                ? ErrorHandling.Result.Value<T, TError>(source.Either.Value)
                : ErrorHandling.Result.Error<T, TError>(source.Either.Error);

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
            source is null
                ? throw new ArgumentNullException(nameof(source))
                : source.Match(x => x, x => x);

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
        public static Task<TResult> Match<T, TResult, TError>(this IAsyncResult<T, TError> source,
            Func<T, TResult> selector)
            where T : TError {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return source.Match(selector, x => selector((T) x));
        }

        public static IAsyncResult<T, IReadOnlyList<TError>> Multiple<T, TError>(
            this IAsyncResult<T, TError> source,
            params Func<IAsyncResult<T, TError>, IAsyncResult<T, TError>>[] validations
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (validations is null) throw new ArgumentNullException(nameof(validations));
            return AsyncResult<T, IReadOnlyList<TError>>.Factory(EitherMethods.MultipleAsync(
                source.Either.ToTaskEither(),
                validations.Select(x =>
                    x.Compose(y =>
                        y.Either.ToTaskEither())(source)).ToArray()));
        }

        /// <inheritdoc cref="ToEnumerable{T,TError}" />
        public static async Task<IEnumerable<T>> ToEnumerable<T, TError>(this IAsyncResult<T, TError> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : EitherMethods.YieldValues(await source.Either.ToTaskEither().ConfigureAwait(false));

        /// <inheritdoc cref="ToErrorEnumerable{T,TError}" />
        public static async Task<IEnumerable<TError>>
            ToErrorEnumerable<T, TError>(this IAsyncResult<T, TError> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : EitherMethods.YieldErrors(await source.Either.ToTaskEither().ConfigureAwait(false));
    }
}