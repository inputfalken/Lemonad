using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Represents a data structure commonly used for error-handling where only one value can be present.
    ///     Either it's <typeparamref name="TError" /> or it's <typeparamref name="T" />. Which makes it possible to handle
    ///     error without throwing exceptions.
    ///     Inspired the Either a b' structure from Haskell and The 'Result&lt;T, TError&gt;' structure from FSharp.
    /// </summary>
    /// <typeparam name="T">
    ///     The type which is considered as successful.
    /// </typeparam>
    /// <typeparam name="TError">
    ///     The type which is considered as failure.
    /// </typeparam>
    public interface IResult<out T, TError> {
        /// <summary>
        ///     Gets the <see cref="IEither{T,TError}" /> from the <see cref="Result{T,TError}" /> instance.
        /// </summary>
        IEither<T, TError> Either { get; }

        /// <summary>
        ///     Casts <typeparamref name="T" /> into <typeparamref name="TResult" />.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="T" /> has been cast to
        ///     <typeparamref name="TResult" />.
        /// </returns>
        IResult<TResult, TError> Cast<TResult>();

        /// <summary>
        ///     Casts <typeparamref name="TError" /> into <typeparamref name="TResult" />.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to <typeparamref name="TError" /> cast to.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="TError" /> has been cast to
        ///     <typeparamref name="TResult" />.
        /// </returns>
        IResult<T, TResult> CastError<TResult>();

        /// <summary>
        ///     Executes <paramref name="action" />.
        /// </summary>
        /// <param name="action">
        ///     Is executed no matter what state <see cref="Result{T,TError}" /> is in.
        /// </param>
        /// <returns>
        ///     <see cref="Result{T,TError}" /> with side effects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     When <paramref name="action" /> is null.
        /// </exception>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> who have invoked <paramref name="action" /> no matter what state the current
        ///     <see cref="Result{T,TError}" /> is in.
        /// </returns>
        IResult<T, TError> Do(Action action);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.Do(Action)" />.
        /// </summary>
        IAsyncResult<T, TError> DoAsync(Func<Task> action);

        /// <summary>
        ///     Executes the <paramref name="action" /> if <typeparamref name="T" /> is the active type.
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        ///     <see cref="Result{T,TError}" /> with side effects.
        /// </returns>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whom may have invoked the <paramref name="action" /> if the current
        ///     <see cref="Result{T,TError}" /> is in a valid state.
        /// </returns>
        IResult<T, TError> DoWith(Action<T> action);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.DoWith(Action{T})" />.
        /// </summary>
        IAsyncResult<T, TError> DoWithAsync(Func<T, Task> action);

        /// <summary>
        ///     Executes the <paramref name="action" /> if <typeparamref name="TError" /> is the active type.
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        ///     <see cref="Result{T,TError}" /> with side effects.
        /// </returns>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whom may have invoked the <paramref name="action" /> if the current
        ///     <see cref="Result{T,TError}" /> is in a an invalid state.
        /// </returns>
        IResult<T, TError> DoWithError(Action<TError> action);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.DoWithError(Action{TError})" />.
        /// </summary>
        IAsyncResult<T, TError> DoWithErrorAsync(Func<TError, Task> action);

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <typeparamref name="T" /> is the active type.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed when the <paramref name="predicate" /> is false. The in parameter is a <see cref="Maybe{T}" /> since
        ///     the <typeparamref name="T" /> could be unsafe in this context.
        /// </param>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="T" /> has been tested by the
        ///     <paramref name="predicate" />
        ///     if the current <see cref="Result{T,TError}" /> is in valid state.
        /// </returns>
        IResult<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FilterAsync" />.
        /// </summary>
        IAsyncResult<T, TError> FilterAsync(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector);

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" /> who shares the same <typeparamref name="TError" />.
        ///     And maps <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as an return type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The return type of the function <paramref name="flatSelector" />.
        /// </typeparam>
        IResult<TResult, TError> FlatMap<TResult>(Func<T, IResult<TResult, TError>> flatSelector);

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" /> who shares the same <typeparamref name="TError" />.
        ///     And maps <typeparamref name="T" /> together with <typeparamref name="TSelector" /> to
        ///     <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as an return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and  <typeparamref name="TSelector" />  which can
        ///     return any type.
        /// </param>
        /// <typeparam name="TSelector">
        ///     The value retrieved from the the <see cref="Result{T,TError}" /> given by the <paramref name="flatSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type of the function  <paramref name="resultSelector" />.
        /// </typeparam>
        IResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, IResult<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector
        );

        /// <summary>
        ///     Flatmaps another <see cref="Result{T,TError}" /> but the <typeparamref name="TError" /> remains as the same type.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TErrorResult" /> to <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When any of the function parameters are null and needs to be executed.
        /// </exception>
        IResult<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> flatMapSelector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" />
        ///     And maps <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///     The error selector if <see cref="Nullable{T}" /> is null.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type of <see cref="Nullable{T}" />.
        /// </typeparam>
        IResult<TResult, TError> FlatMap<TResult>(
            Func<T, TResult?> flatMapSelector,
            Func<TError> errorSelector
        ) where TResult : struct;

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" />
        ///     And maps <typeparamref name="T" /> together with <typeparamref name="TSelector" /> to
        ///     <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatMapSelector">
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
        ///     The value retrieved from the the <see cref="Result{T,TError}" /> given by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type of the function  <paramref name="resultSelector" />.
        /// </typeparam>
        IResult<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> flatMapSelector,
            Func<T, TSelector, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TSelector : struct;

        /// <summary>
        ///     Flatmaps another <see cref="Result{T,TError}" /> but the <typeparamref name="TError" /> remains as the same type.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TFlatMap" /> which can return
        ///     any type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TErrorResult" />. to <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" /> function.
        /// </typeparam>
        /// <typeparam name="TFlatMap">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        IResult<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" />  wrapped in a <see cref="Task{TResult}" />
        ///     And maps <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///     The error selector if <see cref="Nullable{T}" /> is null.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type of <see cref="Nullable{T}" />.
        /// </typeparam>
        IAsyncResult<TResult, TError> FlatMapAsync<TResult>(
            Func<T, Task<TResult?>> flatMapSelector,
            Func<TError> errorSelector
        ) where TResult : struct;

        /// <summary>
        ///     Flatten a <see cref="Nullable{T}" /> wrapped in a <see cref="Task{TResult}" />
        ///     And maps <typeparamref name="T" /> together with <typeparamref name="TSelector" /> to
        ///     <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatMapSelector">
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
        ///     The value retrieved from the the <see cref="Result{T,TError}" /> given by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type of the function  <paramref name="resultSelector" />.
        /// </typeparam>
        IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, Task<TSelector?>> flatMapSelector,
            Func<T, TSelector, TResult> resultSelector,
            Func<TError> errorSelector
        ) where TSelector : struct;

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FlatMapAsync{TResult}" />.
        /// </summary>
        IAsyncResult<TResult, TError> FlatMapAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.FlatMapAsync{TSelector,TResult}(Func{T, IAsyncResult{TSelector, TError}}, Func{T, TSelector, TResult})" />
        ///     .
        /// </summary>
        IAsyncResult<TResult, TError> FlatMapAsync<TSelector, TResult>(
            Func<T, IAsyncResult<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector
        );

        IAsyncResult<TResult, TError> FlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        IAsyncResult<TResult, TError> FlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> selector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" />.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Result{T,TError}" /> returned is not the the result from <paramref name="selector" />.
        /// </remarks>
        /// <param name="selector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///     Maps the error to from <typeparamref name="TErrorResult" /> to <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="TResult">
        ///     The value of the <see cref="Result{T,TError}" /> returned by the function <paramref name="selector" />.
        /// </typeparam>
        /// <typeparam name="TErrorResult">
        ///     The error of the <see cref="Result{T,TError}" /> returned by the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     The same <see cref="Result{T,TError}" /> but it's state is dependent on the <see cref="Result{T,TError}" />
        ///     returned by the <paramref name="selector" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     When any of the function parameters are null and needs to be executed.
        /// </exception>
        IResult<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" />  who shares the same <typeparamref name="TError" />.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Result{T,TError}" /> returned is not the the result from <paramref name="selector" />.
        /// </remarks>
        /// <param name="selector"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IResult<T, TError> Flatten<TResult>(Func<T, IResult<TResult, TError>> selector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FlattenAsync{TResult,TErrorResult}" />.
        /// </summary>
        IAsyncResult<T, TError> FlattenAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FlattenAsync{TResult}" />.
        /// </summary>
        IAsyncResult<T, TError> FlattenAsync<TResult>(Func<T, IAsyncResult<TResult, TError>> selector);

        /// <summary>
        ///     Casts both <typeparamref name="T" /> into <typeparamref name="TResult" /> and <typeparamref name="TError" /> into
        ///     <typeparamref name="TErrorResult" />
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast <typeparamref name="T" /> into.
        /// </typeparam>
        /// <typeparam name="TErrorResult">
        ///     The type to cast <typeparamref name="TErrorResult" /> into.
        /// </typeparam>
        IResult<TResult, TErrorResult> FullCast<TResult, TErrorResult>();

        /// <summary>
        ///     Casts both <typeparamref name="T" /> into <typeparamref name="TResult" /> and <typeparamref name="TError" /> into
        ///     <typeparamref name="TResult" />
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns></returns>
        IResult<TResult, TResult> FullCast<TResult>();

        /// <summary>
        ///     Fully flatmaps another <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TError" /> to <typeparamref name="TErrorResult" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When any of the function parameters are null and needs to be executed.
        /// </exception>
        IResult<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, IResult<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     Fully flatmaps another <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TFlatMap" /> which can return
        ///     any type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TError" /> to <typeparamref name="TErrorResult" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TFlatMap">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        /// <returns></returns>
        IResult<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, IResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FullFlatMapAsync{TFlatMap,TResult,TErrorResult}" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TFlatMap, TResult, TErrorResult>(
            Func<T, IAsyncResult<TFlatMap, TErrorResult>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.FullFlatMapAsync{TResult,TErrorResult}" />.
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullFlatMapAsync<TResult, TErrorResult>(
            Func<T, IAsyncResult<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     Maps both <typeparamref name="T" /> and <typeparamref name="TError" /> but only one is executed.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <typeparamref name="T" /> is the active type.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed if <typeparamref name="TError" /> is the active type.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The result from the function <paramref name="errorSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The result from the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     A mapped <see cref="Result{T,TError}" />.
        /// </returns>
        IResult<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.FullMapAsync{TResult,TErrorResult}(System.Func{T,System.Threading.Tasks.Task{TResult}},System.Func{TError,System.Threading.Tasks.Task{TErrorResult}})" />
        ///     .
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.FullMapAsync{TResult,TErrorResult}(System.Func{T,System.Threading.Tasks.Task{TResult}},System.Func{TError,TErrorResult})" />
        ///     .
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.FullMapAsync{TResult,TErrorResult}(System.Func{T,TResult},System.Func{TError,System.Threading.Tasks.Task{TErrorResult}})" />
        ///     .
        /// </summary>
        IAsyncResult<TResult, TErrorResult> FullMapAsync<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        );

        IResult<T, TError> IsErrorWhen(Func<T, bool> predicate, Func<T, TError> errorSelector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.IsErrorWhenAsync" />.
        /// </summary>
        IAsyncResult<T, TError> IsErrorWhenAsync(Func<T, Task<bool>> predicate, Func<T, TError> errorSelector);

        /// <summary>
        ///     Zip two <see cref="Result{T,TError}" /> when matched with a key.
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="inner">The other <see cref="Result{T,TError}" />.</param>
        /// <param name="outerKeySelector">The key selector for this <see cref="Result{T,TError}" />.</param>
        /// <param name="innerKeySelector">The key selector for the other <see cref="Result{T,TError}" />.</param>
        /// <param name="resultSelector">The selector to determine the returning <see cref="Result{T,TError}" />.</param>
        /// <param name="errorSelector">Is invoked when keys do not match.</param>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />.
        /// </returns>
        IResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        );

        /// <summary>
        ///     Zip two <see cref="Result{T,TError}" /> when matched with a key.
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="inner">The other <see cref="Result{T,TError}" />.</param>
        /// <param name="outerKeySelector">The key selector for this <see cref="Result{T,TError}" />.</param>
        /// <param name="innerKeySelector">The key selector for the other <see cref="Result{T,TError}" />.</param>
        /// <param name="resultSelector">The selector to determine the returning <see cref="Result{T,TError}" />.</param>
        /// <param name="errorSelector">Is invoked when keys do not match.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> to be used when matching keys.</param>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />.
        /// </returns>
        IResult<TResult, TError> Join<TInner, TKey, TResult>(
            IResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.JoinAsync{TInner,TKey,TResult}(Lemonad.ErrorHandling.IAsyncResult{TInner,TError},System.Func{T,TKey},System.Func{TInner,TKey},System.Func{T,TInner,TResult},System.Func{TError},System.Collections.Generic.IEqualityComparer{TKey})" />
        ///     .
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
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see
        ///         cref="IAsyncResult{T,TError}.JoinAsync{TInner,TKey,TResult}(IAsyncResult{TInner, TError}, Func{T, TKey}, Func{TInner, TKey}, Func{T, TInner, TResult}, Func{TError})" />
        ///     .
        /// </summary>
        IAsyncResult<TResult, TError> JoinAsync<TInner, TKey, TResult>(
            IAsyncResult<TInner, TError> inner,
            Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        );

        /// <summary>
        ///     Maps <typeparamref name="T" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <typeparamref name="T" /> is the active type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The result from the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />  with its <typeparamref name="T" /> mapped.
        /// </returns>
        IResult<TResult, TError> Map<TResult>(Func<T, TResult> selector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.MapAsync{TResult}" />.
        /// </summary>
        IAsyncResult<TResult, TError> MapAsync<TResult>(Func<T, Task<TResult>> selector);

        /// <summary>
        ///     Maps <typeparamref name="TError" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <typeparamref name="TError" /> is the active type.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The result from the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />  with its <typeparamref name="TError" /> mapped.
        /// </returns>
        IResult<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector);

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.MapErrorAsync{TErrorResult}" />.
        /// </summary>
        IAsyncResult<T, TErrorResult> MapErrorAsync<TErrorResult>(Func<TError, Task<TErrorResult>> selector);

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="T" />.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="TError" />.
        /// </param>
        /// <typeparam name="TResult">
        ///     The return type of <paramref name="selector" /> and <paramref name="errorSelector" />
        /// </typeparam>
        /// <returns>
        ///     Either <typeparamref name="T" /> or <typeparamref name="TError" /> as <typeparamref name="TResult" />.
        /// </returns>
        TResult Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector);

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="action">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="T" />.
        /// </param>
        /// <param name="errorAction">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="TError" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     When either <paramref name="action" /> or <paramref name="errorAction" /> and needs to be executed.
        /// </exception>
        void Match(Action<T> action, Action<TError> errorAction);

        /// <summary>
        ///     Attempts to cast <typeparamref name="T" /> into <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="errorSelector">
        ///     Is executed if the cast would fail.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="T" /> has been cast to
        ///     <typeparamref name="TResult" />.
        /// </returns>
        IResult<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector);

        /// <summary>
        ///     Merges two <see cref="Result{T,TError}" /> together to create a new <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="Result{T,TError}" />.
        /// </param>
        /// <param name="resultSelector">
        ///     The selector which will determine the type of the returning <see cref="Result{T,TError}" />.
        /// </param>
        /// <typeparam name="TOther">The type of the other <see cref="Result{T,TError}" />.</typeparam>
        /// <typeparam name="TResult">The type of the returning <see cref="Result{T,TError}" />.</typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose value is the result for merging two <see cref="Result{T,TError}" />
        ///     together.
        /// </returns>
        IResult<TResult, TError> Zip<TOther, TResult>(
            IResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        );

        /// <summary>
        ///     Lifts <see cref="IResult{T,TError}" /> into <see cref="IAsyncResult{T,TError}" /> and performs
        ///     <see cref="IAsyncResult{T,TError}.ZipAsync{TOther,TResult}" />.
        /// </summary>
        IAsyncResult<TResult, TError> ZipAsync<TOther, TResult>(
            IAsyncResult<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector
        );
    }
}