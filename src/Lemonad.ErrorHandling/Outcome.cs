using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Internal;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     An asynchronous version of <see cref="Result{T,TError}" /> with the same method members.
    /// </summary>
    public readonly struct Outcome<T, TError> {
        private Outcome(Task<Result<T, TError>> result) =>
            Result = result ?? throw new ArgumentNullException(nameof(result));

        internal Task<Result<T, TError>> Result { get; }

        public static implicit operator Outcome<T, TError>(Task<Result<T, TError>> result) =>
            new Outcome<T, TError>(result);

        public static implicit operator Outcome<T, TError>(T value) =>
            new Outcome<T, TError>(Task.FromResult(ResultExtensions.Ok<T, TError>(value)));

        public Outcome<TResult, TError> Join<TInner, TKey, TResult>(
            Result<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            TaskResultFunctions.Join(Result, inner, outerKeySelector, innerKeySelector, resultSelector, errorSelector);

        public Outcome<TResult, TError> Join<TInner, TKey, TResult>(
            Task<Result<TInner, TError>> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            TaskResultFunctions.Join(Result, inner, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector);

        public Outcome<TResult, TError> Join<TInner, TKey, TResult>(
            Result<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            TaskResultFunctions.Join(Result, inner, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector, comparer);

        public Outcome<TResult, TError> Join<TInner, TKey, TResult>(
            Task<Result<TInner, TError>> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            TaskResultFunctions.Join(Result, inner, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector, comparer);

        public Outcome<TResult, TError> Join<TInner, TKey, TResult>(
            Outcome<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            TaskResultFunctions.Join(Result, inner.Result, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector, comparer);

        public Outcome<TResult, TError> Join<TInner, TKey, TResult>(
            Outcome<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            TaskResultFunctions.Join(Result, inner.Result, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector);

        [Pure]
        public Outcome<TResult, TError> Zip<TOther, TResult>(Result<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) => TaskResultFunctions.Zip(Result, other, resultSelector);

        [Pure]
        public Outcome<TResult, TError> Zip<TOther, TResult>(Task<Result<TOther, TError>> other,
            Func<T, TOther, TResult> resultSelector) => TaskResultFunctions.Zip(Result, other, resultSelector);

        [Pure]
        public Outcome<TResult, TError> Zip<TOther, TResult>(Outcome<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) => TaskResultFunctions.Zip(Result, other, resultSelector);

        private static async Task<Result<T, TError>> Factory(Task<T> foo) => await foo.ConfigureAwait(false);
        private static async Task<Result<T, TError>> ErrorFactory(Task<TError> foo) => await foo.ConfigureAwait(false);

        public static implicit operator Outcome<T, TError>(Task<T> value) => Factory(value);

        public static implicit operator Outcome<T, TError>(Task<TError> error) => ErrorFactory(error);

        public static implicit operator Outcome<T, TError>(TError error) =>
            new Outcome<T, TError>(Task.FromResult(ResultExtensions.Error<T, TError>(error)));

        /// <inheritdoc cref="Result{T,TError}.Filter(System.Func{T,bool},System.Func{TError})" />
        public Outcome<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) =>
            TaskResultFunctions.Filter(Result, predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Filter(System.Func{T,bool},System.Func{Maybe{T},TError})" />
        public Outcome<T, TError> Filter(Func<T, bool> predicate, Func<Maybe<T>, TError> errorSelector) =>
            Result.Filter(predicate, errorSelector);

        public Outcome<T, TError> Filter(Func<T, Task<bool>> predicate, Func<TError> errorSelector) =>
            FilterFactory(this, predicate, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Filter(System.Func{T,bool},System.Func{Maybe{T},TError})" />
        public Outcome<T, TError> Filter(Func<T, Task<bool>> predicate, Func<Maybe<T>, TError> errorSelector) =>
            FilterFactory(this, predicate, errorSelector);

        private static async Task<Result<T, TError>> FilterFactory(Outcome<T, TError> source,
            Func<T, Task<bool>> predicate, Func<TError> errorSelector) =>
            await (
                    await source.Result.ConfigureAwait(false)
                )
                .Filter(predicate, errorSelector).Result
                .ConfigureAwait(false);

        private static async Task<Result<T, TError>> FilterFactory(Outcome<T, TError> source,
            Func<T, Task<bool>> predicate, Func<Maybe<T>, TError> errorSelector) =>
            await (
                    await source.Result.ConfigureAwait(false)
                )
                .Filter(predicate, errorSelector).Result
                .ConfigureAwait(false);

        /// <inheritdoc cref="Result{T,TError}.HasError" />
        public Task<bool> HasError => TaskResultFunctions.HasError(Result);

        /// <inheritdoc cref="Result{T,TError}.HasValue" />
        public Task<bool> HasValue => TaskResultFunctions.HasValue(Result);

        /// <inheritdoc cref="Result{T,TError}.AsEnumerable" />
        public Task<IEnumerable<T>> AsEnumerable => TaskResultFunctions.AsEnumerable(Result);

        /// <inheritdoc cref="Result{T,TError}.AsErrorEnumerable" />
        public Task<IEnumerable<TError>> AsErrorEnumerable => TaskResultFunctions.AsErrorEnumerable(Result);

        /// <inheritdoc cref="Result{T,TError}.Multiple" />
        public Outcome<T, IReadOnlyList<TError>> Multiple(
            params Func<Result<T, TError>, Result<T, TError>>[] validations) =>
            TaskResultFunctions.Multiple(Result, validations);

        /// <inheritdoc cref="Result{T,TError}.IsErrorWhen(System.Func{T,bool},System.Func{TError})" />
        public Outcome<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(Result, predicate, errorSelector);

        public Outcome<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(Result, predicate, errorSelector);

        public Outcome<T, TError> IsErrorWhen(
            Func<T, Task<bool>> predicate,
            Func<TError> errorSelector) =>
            IsErrorWhenFactory(this, predicate, errorSelector);

        public Outcome<T, TError> IsErrorWhen(
            Func<T, Task<bool>> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            IsErrorWhenFactory(this, predicate, errorSelector);

        private static async Task<Result<T, TError>> IsErrorWhenFactory(Outcome<T, TError> source,
            Func<T, Task<bool>> predicate, Func<Maybe<T>, TError> errorSelector) =>
            await (
                    await source.Result.ConfigureAwait(false)
                )
                .IsErrorWhen(predicate, errorSelector).Result
                .ConfigureAwait(false);

        private static async Task<Result<T, TError>> IsErrorWhenFactory(Outcome<T, TError> source,
            Func<T, Task<bool>> predicate, Func<TError> errorSelector) =>
            await (
                    await source.Result.ConfigureAwait(false)
                )
                .IsErrorWhen(predicate, errorSelector).Result
                .ConfigureAwait(false);

        /// <inheritdoc cref="Result{T,TError}.IsErrorWhenNull(System.Func{TError})" />
        public Outcome<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhenNull(Result, errorSelector);

        public Outcome<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            TaskResultFunctions.Map(Result, selector);

        public Outcome<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector) => MapFactory(this, selector);

        private static async Task<Result<TResult, TError>> MapFactory<TResult>(Outcome<T, TError> source,
            Func<T, Task<TResult>> selector) => await (await source.Result.ConfigureAwait(false)).Map(selector).Result
            .ConfigureAwait(false);

        public Outcome<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            TaskResultFunctions.MapError(Result, selector);

        public Outcome<T, TErrorResult> MapError<TErrorResult>(Func<TError, Task<TErrorResult>> selector) =>
            MapErrorFactory(this, selector);

        private static async Task<Result<T, TResult>> MapErrorFactory<TResult>(Outcome<T, TError> source,
            Func<TError, Task<TResult>> selector) => await (
                await source.Result.ConfigureAwait(false)
            )
            .MapError(selector).Result
            .ConfigureAwait(false);

        /// <inheritdoc cref="Result{T,TError}.Do" />
        public Outcome<T, TError> Do(Action action) => TaskResultFunctions.Do(Result, action);

        /// <inheritdoc cref="Result{T,TError}.DoWithError" />
        public Outcome<T, TError> DoWithError(Action<TError> action) => TaskResultFunctions.DoWithError(Result, action);

        /// <inheritdoc cref="Result{T,TError}.DoWith" />
        public Outcome<T, TError> DoWith(Action<T> action) => TaskResultFunctions.DoWith(Result, action);

        /// <inheritdoc cref="Result{T,TError}.FullMap{TResult,TErrorResult}" />
        public Outcome<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => TaskResultFunctions.FullMap(Result, selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match{TResult}" />
        public Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            TaskResultFunctions.Match(Result, selector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Match" />
        public Task Match(Action<T> action, Action<TError> errorAction) =>
            TaskResultFunctions.Match(Result, action, errorAction);

        public Outcome<TResult, TError> FlatMap<TResult>(Func<T, Result<TResult, TError>> flatSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector);

        public Outcome<TResult, TError> FlatMap<TResult>(Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector);

        public Outcome<TResult, TError> FlatMap<TResult>(Func<T, Outcome<TResult, TError>> flatSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatSelector(x).Result);

        public Outcome<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector, resultSelector);

        public Outcome<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector, resultSelector);

        public Outcome<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Outcome<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatSelector(x).Result, resultSelector);

        public Outcome<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, errorSelector);

        public Outcome<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, errorSelector);

        public Outcome<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Outcome<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatMapSelector(x).Result, errorSelector);

        public Outcome<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Outcome<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Outcome<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Outcome<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatMapSelector(x).Result, resultSelector, errorSelector);

        /// <inheritdoc cref="Result{T,TError}.Cast{TResult}" />
        public Outcome<TResult, TError> Cast<TResult>() => TaskResultFunctions.Cast<T, TResult, TError>(Result);

        public Outcome<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(Result, selector, errorSelector);

        public Outcome<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Outcome<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(Result, x => selector(x).Result, errorSelector);

        public Outcome<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(Result, selector, errorSelector);

        public Outcome<T, TError> Flatten<TResult>(Func<T, Task<Result<TResult, TError>>> selector) =>
            TaskResultFunctions.Flatten(Result, selector);

        public Outcome<T, TError> Flatten<TResult>(Func<T, Result<TResult, TError>> selector) =>
            TaskResultFunctions.Flatten(Result, selector);

        public Outcome<T, TError> Flatten<TResult>(Func<T, Outcome<TResult, TError>> selector) =>
            TaskResultFunctions.Flatten(Result, x => selector(x).Result);

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult,TErrorResult}" />
        public Outcome<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            TaskResultFunctions.FullCast<T, TResult, TError, TErrorResult>(Result);

        /// <inheritdoc cref="Result{T,TError}.FullCast{TResult}" />
        public Outcome<TResult, TResult> FullCast<TResult>() => FullCast<TResult, TResult>();

        /// <inheritdoc cref="Result{T,TError}.CastError{TResult}" />
        public Outcome<T, TResult> CastError<TResult>() => TaskResultFunctions.CastError<T, TError, TResult>(Result);

        /// <inheritdoc cref="Result{T,TError}.SafeCast{TResult}" />
        public Outcome<TResult, TError> SafeCast<TResult>(Func<TError> errorSelector) =>
            TaskResultFunctions.SafeCast<T, TResult, TError>(Result, errorSelector);

        public Outcome<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Outcome<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Outcome<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, x => flatMapSelector(x).Result, resultSelector, errorSelector);

        public Outcome<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Outcome<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, errorSelector);

        public Outcome<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Outcome<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, x => flatMapSelector(x).Result, errorSelector);

        public Outcome<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, errorSelector);
    }
}
