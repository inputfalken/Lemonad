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
            TaskResultFunctions.Filter(Result, predicate, errorSelector);

        public Denouement<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(Result, predicate, errorSelector);

        public Denouement<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhenNull(Result, errorSelector);

        public Denouement<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            TaskResultFunctions.Map(Result, selector);
        
        public Denouement<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector) =>
            TaskResultFunctions.Map(Result, selector);

        public Denouement<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            TaskResultFunctions.MapError(Result, selector);

        public Denouement<T, TError> Do(Action action) => TaskResultFunctions.Do(Result, action);

        public Denouement<T, TError> DoWithError(Action<TError> action) => TaskResultFunctions.DoWithError(Result, action);

        public Denouement<T, TError> DoWith(Action<T> action) => TaskResultFunctions.DoWith(Result, action);

        public Denouement<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => TaskResultFunctions.FullMap(Result, selector, errorSelector);

        public Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            TaskResultFunctions.Match(Result, selector, errorSelector);

        public Task Match(Action<T> action, Action<TError> errorAction) =>
            TaskResultFunctions.Match(Result, action, errorAction);

        public Denouement<TResult, TError> FlatMap<TResult>(Func<T, Result<TResult, TError>> flatSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector);

        public Denouement<TResult, TError> FlatMap<TResult>(Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector);

        public Denouement<TResult, TError> FlatMap<TResult>(Func<T, Denouement<TResult, TError>> flatSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatSelector(x).Result);

        public Denouement<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector, resultSelector);

        public Denouement<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(Result, flatSelector, resultSelector);

        public Denouement<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Denouement<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatSelector(x).Result, resultSelector);

        public Denouement<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Denouement<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatMapSelector(x).Result, errorSelector);

        public Denouement<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Denouement<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Denouement<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.FlatMap(Result, x => flatMapSelector(x).Result, resultSelector, errorSelector);

        public Denouement<TResult, TError> Cast<TResult>() => TaskResultFunctions.Cast<T, TResult, TError>(Result);

        public Denouement<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(Result, selector, errorSelector);

        public Denouement<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Denouement<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(Result, x => selector(x).Result, errorSelector);

        public Denouement<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            TaskResultFunctions.Flatten(Result, selector, errorSelector);

        public Denouement<T, TError> Flatten<TResult>(Func<T, Task<Result<TResult, TError>>> selector) =>
            TaskResultFunctions.Flatten(Result, selector);

        public Denouement<T, TError> Flatten<TResult>(Func<T, Result<TResult, TError>> selector) =>
            TaskResultFunctions.Flatten(Result, selector);

        public Denouement<T, TError> Flatten<TResult>(Func<T, Denouement<TResult, TError>> selector) =>
            TaskResultFunctions.Flatten(Result, x => selector(x).Result);

        public Denouement<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            TaskResultFunctions.FullCast<T, TResult, TError, TErrorResult>(Result);

        public Denouement<TResult, TResult> FullCast<TResult>() => FullCast<TResult, TResult>();

        public Denouement<T, TResult> CastError<TResult>() => TaskResultFunctions.CastError<T, TError, TResult>(Result);

        public Denouement<TResult, TError> SafeCast<TResult>(Func<TError> errorSelector) =>
            TaskResultFunctions.SafeCast<T, TResult, TError>(Result, errorSelector);

        public Denouement<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Denouement<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Denouement<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, x => flatMapSelector(x).Result, resultSelector, errorSelector);

        public Denouement<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, resultSelector, errorSelector);

        public Denouement<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, errorSelector);

        public Denouement<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Denouement<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, x => flatMapSelector(x).Result, errorSelector);

        public Denouement<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            TaskResultFunctions.FullFlatMap(Result, flatMapSelector, errorSelector);
    }
}