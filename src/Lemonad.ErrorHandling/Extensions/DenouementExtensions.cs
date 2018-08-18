using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Internal;

namespace Lemonad.ErrorHandling.Extensions {
    public static class DenouementExtensions {
        [Pure]
        public static Denouement<TResult, TError> Map<T, TResult, TError>(this Task<Result<T, TError>> source,
            Func<T, TResult> selector) => TaskResultFunctions.Map(source, selector);

        [Pure]
        public static Denouement<T, TErrorResult> MapError<T, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<TError, TErrorResult> selector) =>
            TaskResultFunctions.MapError(source, selector);

        [Pure]
        public static Denouement<TResult, TErrorResult> FullMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => TaskResultFunctions.FullMap(source, selector, errorSelector);

        [Pure]
        public static Task<TResult> Match<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) =>
            TaskResultFunctions.Match(source, selector, errorSelector);

        public static Task Match<T, TError>(
            this Task<Result<T, TError>> source,
            Action<T> action,
            Action<TError> errorAction) =>
            TaskResultFunctions.Match(source, action, errorAction);

        public static Denouement<T, TError> Do<T, TError>(this Task<Result<T, TError>> source, Action action) =>
            TaskResultFunctions.Do(source, action);

        public static Denouement<T, TError> DoWithError<T, TError>(
            this Task<Result<T, TError>> source,
            Action<TError> action) => TaskResultFunctions.DoWithError(source, action);

        public static Denouement<T, TError> DoWith<T, TError>(
            this Task<Result<T, TError>> source,
            Action<T> action) =>
            TaskResultFunctions.DoWith(source, action);

        [Pure]
        public static Denouement<T, TError> Filter<T, TError>(
            this Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            TaskResultFunctions.Filter(source, predicate, errorSelector);

        [Pure]
        public static Denouement<T, TError> IsErrorWhen<T, TError>(
            this Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(source, predicate, errorSelector);

        [Pure]
        public static Denouement<T, TError> IsErrorWhenNull<T, TError>(
            this Task<Result<T, TError>> source,
            Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhenNull(source, errorSelector);

        [Pure]
        public static Denouement<T, TError> Flatten<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<T, Result<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(source, selector, errorSelector);

        [Pure]
        public static Task<Result<T, TError>> Flatten<T, TError, TResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> selector) => TaskResultFunctions.Flatten(source, selector);

        [Pure]
        public static Denouement<T, TError> Flatten<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source, Func<T, Task<Result<TResult, TErrorResult>>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(source, selector, errorSelector);

        [Pure]
        public static Denouement<T, TError> Flatten<T, TError, TResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> selector) =>
            TaskResultFunctions.Flatten(source, selector);

        [Pure]
        public static Denouement<TResult, TErrorResult> FullFlatMap<T, TError, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(source, flatMapSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TErrorResult> FullFlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(source, flatMapSelector, resultSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TErrorResult> FullFlatMap<T, TError, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(source, flatMapSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TErrorResult> FullFlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(source, flatMapSelector, resultSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> flatSelector) =>
            TaskResultFunctions.FlatMap(source, flatSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            TaskResultFunctions.FlatMap(source, flatSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TSelector, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(source, flatSelector, resultSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TSelector, TResult, TError>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(source, flatSelector, resultSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(source, flatMapSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TResult, TError, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(source, flatMapSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(source, flatMapSelector, resultSelector, errorSelector);

        [Pure]
        public static Denouement<TResult, TError> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            this Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(source, flatMapSelector, resultSelector, errorSelector);
    }
}