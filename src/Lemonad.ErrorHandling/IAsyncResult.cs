﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Represents an asynchronous version of <see cref="IResult{T,TError}" />.
    /// </summary>
    public interface IAsyncResult<out T, TError> {
        /// <summary>
        ///     An asynchronous version of <see cref="IEither{T,TError}" />.
        /// </summary>
        IAsyncEither<T, TError> Either { get; }

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Cast{TResult}" />.
        /// </summary>
        IAsyncResult<TResult, TError> Cast<TResult>();

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.CastError{TResult}" />.
        /// </summary>
        IAsyncResult<T, TResult> CastError<TResult>();

        /// <inheritdoc cref="IResult{T,TError}.Do" />
        IAsyncResult<T, TError> Do(Action action);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Do" />.
        /// </summary>
        IAsyncResult<T, TError> DoAsync(Func<Task> action);

        /// <inheritdoc cref="IResult{T,TError}.DoWith" />
        IAsyncResult<T, TError> DoWith(Action<T> action);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.DoWith" />.
        /// </summary>
        IAsyncResult<T, TError> DoWithAsync(Func<T, Task> action);

        /// <inheritdoc cref="IResult{T,TError}.DoWithError" />
        IAsyncResult<T, TError> DoWithError(Action<TError> action);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.DoWith" />.
        /// </summary>
        IAsyncResult<T, TError> DoWithErrorAsync(Func<TError, Task> action);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Filter(Func{T, bool}, Func{T, TError})" />.
        /// </summary>
        IAsyncResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Filter(Func{T, bool}, Func{T, TError})" />
        ///     with an predicate expecting a Task&lt;bool&gt;.
        /// </summary>
        IAsyncResult<T, TError> FilterAsync(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector);

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" />
        ///     And maps <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Nullable{T}" /> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///     The error selector if <see cref="Nullable{T}" /> is null.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type of <see cref="Nullable{T}" />.
        /// </typeparam>
        IAsyncResult<TResult, TError> FlatMap<TResult>(
            Func<T, TResult?> selector,
            Func<TError> errorSelector
        ) where TResult : struct;

        IAsyncResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" />
        ///     And maps <typeparamref name="T" /> together with <typeparamref name="TSelector" /> to
        ///     <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Nullable{T}" /> as an return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and  <typeparamref name="TSelector" />  which can
        ///     return any type.
        /// </param>
        /// <param name="errorSelector">
        ///     The error selector if <see cref="Nullable{T}" /> is null.
        /// </param>
        /// <typeparam name="TSelector">
        ///     The value retrieved from the the <see cref="Result{T,TError}" /> given by the <paramref name="selector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type of the function  <paramref name="resultSelector" />.
        /// </typeparam>
        IAsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TSelector : struct;

        IAsyncResult<TResult, TError> FlatMap<TResult>(
            Func<T, IResult<TResult, TError>> selector
        );

        IAsyncResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        );

        IAsyncResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" />  wrapped in a <see cref="Task{TResult}" />
        ///     And maps <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Nullable{T}" /> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///     The error selector if <see cref="Nullable{T}" /> is null.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type of <see cref="Nullable{T}" />.
        /// </typeparam>
        IAsyncResult<TResult, TError> FlatMapAsync<TResult>(
            Func<T, Task<TResult?>> selector,
            Func<TError> errorSelector
        ) where TResult : struct;

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" /> wrapped in a <see cref="Task{TResult}" />
        ///     And maps <typeparamref name="T" /> together with <typeparamref name="TSelector" /> to
        ///     <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="selector">
        ///     A function who expects a <see cref="Nullable{T}" /> as an return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and  <typeparamref name="TSelector" />  which can
        ///     return any type.
        /// </param>
        /// <param name="errorSelector">
        ///     The error selector if <see cref="Nullable{T}" /> is null.
        /// </param>
        /// <typeparam name="TSelector">
        ///     The value retrieved from the the <see cref="Result{T,TError}" /> given by the <paramref name="selector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type of the function  <paramref name="resultSelector" />.
        /// </typeparam>
        IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, Task<TSelector?>> selector,
            Func<T, TSelector, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TSelector : struct;

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="Result{T,TError}.FlatMap{TResult}(System.Func{T,Lemonad.ErrorHandling.IResult{TResult,TError}})" />
        ///     who expects an
        ///     <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> FlatMapAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="Result{T,TError}.FlatMap{TSelector, TResult}(Func{T, IResult{TSelector, TError}}, Func{T, TSelector, TResult})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="Result{T,TError}.FlatMap{TResult, TErrorResult}(Func{T, IResult{TResult, TErrorResult}}, Func{TErrorResult, TError})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> FlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="Result{T,TError}.FlatMap{TFlatMap, TResult, TErrorResult}(Func{T, IResult{TFlatMap, TErrorResult}}, Func{T, TFlatMap, TResult}, Func{TErrorResult, TError})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        );

        IAsyncResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        IAsyncResult<T, TError> Flatten<TResult>(
            Func<T, IResult<TResult, TError>> selector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="IResult{T,TError}.Flatten{TResult,TErrorResult}(Func{T, IResult{TResult, TErrorResult}}, Func{TErrorResult, TError})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<T, TError> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Flatten{TResult}(Func{T, IResult{TResult, TError}})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<T, TError> FlattenAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.FullCast{TResult,TErrorResult}()" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>();

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.FullCast{TResult}()" />>
        /// </summary>
        IAsyncResult<TResult, TResult> FullCast<TResult>();

        IAsyncResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector
        );

        IAsyncResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="IResult{T,TError}.FullFlatMap{TFlatMap, TResult, TErrorResult}(Func{T, IResult{TFlatMap, TErrorResult}}, Func{T, TFlatMap, TResult}, Func{TError, TErrorResult})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="IResult{T,TError}.FullFlatMap{TResult, TErrorResult}(Func{T, IResult{TResult, TErrorResult}}, Func{TError, TErrorResult})" />
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.FullMap{TResult,TErrorResult}(Func{T, TResult}, Func{TError, TErrorResult})" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.FullMap{TResult,TErrorResult}(Func{T, TResult}, Func{TError, TErrorResult})" />
        ///     who expects <see cref="Task{TResult}" /> for <paramref name="selector" /> and <paramref name="errorSelector" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.FullMap{TResult,TErrorResult}(Func{T, TResult}, Func{TError, TErrorResult})" />
        ///     who expects <see cref="Task{TResult}" /> for <paramref name="selector" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.FullMap{TResult,TErrorResult}(Func{T, TResult}, Func{TError, TErrorResult})" />
        ///     who expects <see cref="Task{TResult}" /> for <paramref name="errorSelector" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.IsErrorWhen(System.Func{T,bool},System.Func{T,TError})" />.
        /// </summary>
        IAsyncResult<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<T, TError> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.IsErrorWhen(System.Func{T,bool},System.Func{T,TError})" />
        ///     with an predicate expecting a Task&lt;bool&gt;.
        /// </summary>
        IAsyncResult<T, TError> IsErrorWhenAsync(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector);

        IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector);

        IAsyncResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer);

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="IResult{T,TError}.Join{TInner,TKey,TResult}(Lemonad.ErrorHandling.IResult{TInner,TError},Func{T,TKey},Func{TInner,TKey},Func{T,TInner,TResult},Func{TError}, IEqualityComparer{TKey})" />
        ///     .
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see
        ///         cref="IResult{T,TError}.Join{TInner,TKey,TResult}(Lemonad.ErrorHandling.IResult{TInner,TError},Func{T,TKey},Func{TInner,TKey},Func{T,TInner,TResult},Func{TError})" />
        ///     .
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Map{TResult}(Func{T, TResult})" />.
        /// </summary>
        IAsyncResult<TResult, TError> Map<TResult>(Func<T, TResult> selector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Map{TResult}(Func{T, TResult})" />.
        ///     with an function expecting a <see cref="Task{TResult}" />.
        /// </summary>
        IAsyncResult<TResult, TError> MapAsync<TResult>(Func<T, Task<TResult>> selector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.MapError{TErrorResult}(Func{TError, TErrorResult})" />.
        /// </summary>
        IAsyncResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.MapError{TErrorResult}(Func{TError, TErrorResult})" />.
        ///     with an function expecting a <see cref="Task{TResult}" />.
        /// </summary>
        IAsyncResult<T, TErrorResult> MapErrorAsync<TErrorResult>(Func<TError, Task<TErrorResult>> selector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Match{TResult}(Func{T, TResult}, Func{TError, TResult})" />
        ///     who returns <see cref="Task{TResult}" />.
        /// </summary>
        Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Match(Action{T }, Action{TError})" />
        ///     who returns <see cref="Task" />
        /// </summary>
        Task Match(Action<T> action, Action<TError> errorAction);

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.SafeCast{TResult}(Func{T,TError})" />.
        /// </summary>
        IAsyncResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector);

        IAsyncResult<TResult, TError> Zip<TOther, TResult>(
            IResult<TOther, TError> other,
            Func<T, TOther, TResult> selector
        );

        /// <summary>
        ///     An asynchronous version of
        ///     <see cref="IResult{T,TError}.Zip{TOther,TResult}(IResult{TOther, TError}, Func{T, TOther, TResult})" />.
        ///     who expects an <see cref="IAsyncResult{T,TError}" /> instead of <see cref="IResult{T,TError}" />.
        /// </summary>
        IAsyncResult<TResult, TError> ZipAsync<TOther, TResult>(
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> selector
        );
    }
}