using System;
using System.Collections.Generic;
using System.Linq;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.Result {
    public static class Index {
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
                ? ErrorHandling.Result.Value<T, IReadOnlyList<TError>>(either.Value)
                : ErrorHandling.Result.Error<T, IReadOnlyList<TError>>(either.Error);
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
        ///     <typeparamref name="T" />.
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
        public static IMaybe<T> ToMaybe<T, TError>(this IResult<T, TError> source)
            => source.Either.HasValue ? ErrorHandling.Maybe.Value(source.Either.Value) : ErrorHandling.Maybe.None<T>();
    }
}