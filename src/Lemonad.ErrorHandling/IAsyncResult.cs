using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    public interface IAsyncResult<out T, TError> {
        IAsyncEither<T, TError> Either { get; }

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}" />
        IAsyncResult<TResult, TError> Cast<TResult>();

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}" />
        IAsyncResult<T, TResult> CastError<TResult>();

        /// <inheritdoc cref="Result{T,TError}.Do" />
        IAsyncResult<T, TError> Do(Action action);

        /// <inheritdoc cref="Result{T,TError}.DoWith" />
        IAsyncResult<T, TError> DoWith(Action<T> action);

        /// <inheritdoc cref="Result{T,TError}.DoWithError" />
        IAsyncResult<T, TError> DoWithError(Action<TError> action);

        IAsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector);
        IAsyncResult<T, TError> Filter(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector);

        IAsyncResult<TResult, TError> FlatMap<TResult>(Func<T, IAsyncResult<TResult, TError>> flatSelector);

        IAsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector);

        IAsyncResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector);

        IAsyncResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector);

        IAsyncResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        IAsyncResult<T, TError> Flatten<TResult>(Func<T, IAsyncResult<TResult, TError>> selector);

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}" />
        IAsyncResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>();

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult}" />
        IAsyncResult<TResult, TResult> FullCast<TResult>();

        IAsyncResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector);

        IAsyncResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector);

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}" />
        IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        );

        IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        );

        IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        );

        IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        );

        IAsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector);

        IAsyncResult<T, TError> IsErrorWhen(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector);

        IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer);

        IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector);

        IAsyncResult<TResult, TError> Map<TResult>(Func<T, TResult> selector);
        IAsyncResult<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector);
        IAsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector);
        IAsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, Task<TErrorResult>> selector);

        /// <inheritdoc cref="Result{T,TError}.Match{TResult}" />
        Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match" />
        Task Match(Action<T> action, Action<TError> errorAction);

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}" />
        IAsyncResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector);

        IAsyncResult<TResult, TError> Zip<TOther, TResult>(
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        );
    }
}