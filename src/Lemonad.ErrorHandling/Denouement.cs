using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Internal;

namespace Lemonad.ErrorHandling {
    public class Denouement<T, TError> {
        internal Task<Result<T, TError>> Result { get; }

        public static implicit operator Denouement<T, TError>(Task<Result<T, TError>> result) =>
            new Denouement<T, TError>(result);

        public static implicit operator Denouement<T, TError>(T value) =>
            new Denouement<T, TError>(Task.FromResult(ResultExtensions.Ok<T, TError>(value)));

        public static implicit operator Denouement<T, TError>(Task<T> value) =>
            new Func<Task<Result<T, TError>>>(async () => await value)();

        public static implicit operator Denouement<T, TError>(Task<TError> error) =>
            new Func<Task<Result<T, TError>>>(async () => await error)();

        public static implicit operator Denouement<T, TError>(TError error) =>
            new Denouement<T, TError>(Task.FromResult(ResultExtensions.Error<T, TError>(error)));

        public Denouement(Task<Result<T, TError>> result) =>
            Result = result ?? throw new ArgumentNullException(nameof(result));

        public Denouement<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) =>
            Result.Filter(predicate, errorSelector);

        public Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            Result.Match(selector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TResult>(Func<T, Result<TResult, TError>> flatSelector) =>
            Result.FlatMap(flatSelector);

        public Denouement<TResult, TError> FlatMap<TResult>(Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            Result.FlatMap(flatSelector);

        public Denouement<TResult, TError> FlatMap<TResult>(Func<T, Denouement<TResult, TError>> flatSelector) =>
            Result.FlatMap(x => flatSelector(x).Result);

        public Denouement<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => Result.FlatMap(flatSelector, resultSelector);

        public Denouement<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => Result.FlatMap(flatSelector, resultSelector);

        public Denouement<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Denouement<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => Result.FlatMap(x => flatSelector(x).Result, resultSelector);

        public Denouement<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            Result.FlatMap(flatMapSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            Result.FlatMap(flatMapSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Denouement<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            Result.FlatMap(x => flatMapSelector(x).Result, errorSelector);

        public Denouement<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) => Result.FlatMap(flatMapSelector, resultSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) => Result.FlatMap(flatMapSelector, resultSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Denouement<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            Result.FlatMap(x => flatMapSelector(x).Result, resultSelector, errorSelector);

        public Denouement<TResult, TError> Cast<TResult>() => TaskResultFunctions.Cast<T, TResult, TError>(Result);
    }
}