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
    /// <summary>
    ///     Provides a set of static methods for <see cref="IAsyncResult{T,TError}" />.
    /// </summary>
    public static class AsyncResult {
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
        public static TaskAwaiter<IResult<T, TError>> GetAwaiter<T, TError>(this IAsyncResult<T, TError> source)
            => Mapper(source).GetAwaiter();

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
        public static TaskAwaiter<IEither<T, TError>> GetAwaiter<T, TError>(this IAsyncEither<T, TError> source)
            => source.ToTaskEither().GetAwaiter();

        private static async Task<IResult<T, TError>> Mapper<T, TError>(IAsyncResult<T, TError> asyncResult)
            => await asyncResult.Either.HasValue.ConfigureAwait(false)
                ? Result.Value<T, TError>(asyncResult.Either.Value)
                : Result.Error<T, TError>(asyncResult.Either.Error);

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
            AsyncResult<T, IReadOnlyList<TError>>.Factory(EitherMethods.MultipleAsync(source.Either.ToTaskEither(),
                validations.Select(x => x.Compose(y => y.Either.ToTaskEither())(source)).ToArray()));

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
        ///     Converts a <see cref="IResult{T,TError}" /> into a <see cref="IAsyncResult{T,TError}" />.
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
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this IResult<T, TError> result)
            => ToAsyncResult(Task.FromResult(result));

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
            EitherMethods.YieldValues(await result.Either.ToTaskEither().ConfigureAwait(false));

        /// <inheritdoc cref="ToErrorEnumerable{T,TError}" />
        public static async Task<IEnumerable<TError>>
            ToErrorEnumerable<T, TError>(this IAsyncResult<T, TError> result) =>
            EitherMethods.YieldErrors(await result.Either.ToTaskEither().ConfigureAwait(false));

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

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.Zip{TOther, TResult}(IAsyncResult{TOther, TError}, System.Func{T, TOther, TResult})" />.
        /// </summary>
        public static IAsyncResult<TResult, TError> ZipSync<T, TOther, TResult, TError>(
            this IAsyncResult<T, TError> source,
            IResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        ) => source.Zip(other.ToAsyncResult(), resultSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.Flatten{TResult, TErrorResult}(System.Func{T, IAsyncResult{TResult, TErrorResult}}, System.Func{TErrorResult, TError })" />.
        /// </summary>
        public static IAsyncResult<T, TError> FlattenSync<T, TResult, TError, TErrorResult>(
            this IAsyncResult<T, TError> source,
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        ) => source.Flatten(selector.Compose(ToAsyncResult), errorSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.Flatten{TResult}(System.Func{T, IAsyncResult{TResult, TError}})" />.
        /// </summary>
        public static IAsyncResult<T, TError> FlattenSync<T, TResult, TError>(
            this IAsyncResult<T, TError> source,
            Func<T, IResult<TResult, TError>> selector
        ) => source.Flatten(selector.Compose(ToAsyncResult));

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FullFlatMap{TResult, TErrorResult}(Func{T, IAsyncResult{TResult, TErrorResult}}, Func{TError, TErrorResult})" />.
        /// </summary>
        public static IAsyncResult<TResult, TErrorResult> FullFlatMapSync<T, TResult, TError, TErrorResult>(
            this IAsyncResult<T, TError> source,
            Func<T, IResult<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector
        ) => source.FullFlatMap(flatMapSelector.Compose(ToAsyncResult), errorSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FullFlatMap{TFlatMap,TResult,TErrorResult}(Func{T, IAsyncResult{TFlatMap, TErrorResult}}, Func{T, TFlatMap, TResult}, Func{TError, TErrorResult})" />.
        /// </summary>
        public static IAsyncResult<TResult, TErrorResult> FullFlatMapSync<T, TFlatMap, TResult, TError, TErrorResult>(
            this IAsyncResult<T, TError> source,
            Func<T, IResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        ) => source.FullFlatMap(flatMapSelector.Compose(ToAsyncResult), resultSelector, errorSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FlatMap{TResult}" />.
        /// </summary>
        public static IAsyncResult<TResult, TError> FlatMapSync<T, TResult, TError>(
            this IAsyncResult<T, TError> source,
            Func<T, IResult<TResult, TError>> flatSelector
        ) => source.FlatMap(flatSelector.Compose(ToAsyncResult));

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FlatMap{TSelector,TResult}(System.Func{T, IAsyncResult{TSelector, TError}}, System.Func{T, TSelector, TResult})" />.
        /// </summary>
        public static IAsyncResult<TResult, TError> FlatMapSync<T, TSelector, TResult, TError>(
            this IAsyncResult<T, TError> source,
            Func<T, IResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector
        ) => source.FlatMap(flatSelector.Compose(ToAsyncResult), resultSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.Join{TInner,TKey,TResult}(Lemonad.ErrorHandling.IAsyncResult{TInner,TError},System.Func{T,TKey},System.Func{TInner,TKey},System.Func{T,TInner,TResult},System.Func{TError},IEqualityComparer{TKey})" />.
        /// </summary>
        public static IAsyncResult<TResult, TError> JoinSync<T, TInner, TKey, TResult, TError>(
            this IAsyncResult<T, TError> source,
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer)
            => source
                .Join(
                    inner.ToAsyncResult(),
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    errorSelector,
                    comparer
                );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.Join{TInner,TKey,TResult}(Lemonad.ErrorHandling.IAsyncResult{TInner,TError},System.Func{T,TKey},System.Func{TInner,TKey},System.Func{T,TInner,TResult},System.Func{TError})" />.
        /// </summary>
        public static IAsyncResult<TResult, TError> JoinSync<T, TInner, TKey, TResult, TError>(
            this IAsyncResult<T, TError> source,
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector) => source
            .Join(
                inner.ToAsyncResult(),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector
            );
    }
}
