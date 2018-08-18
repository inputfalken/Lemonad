using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;
using Lemonad.ErrorHandling.Extensions.Internal;

namespace Lemonad.ErrorHandling {
    public class Outcome<T, TError> {
        public Outcome(Task<Result<T, TError>> result) =>
            Result = result ?? throw new ArgumentNullException(nameof(result));

        internal Task<Result<T, TError>> Result { get; }

        public static implicit operator Outcome<T, TError>(Task<Result<T, TError>> result) =>
            new Outcome<T, TError>(result);

        public static implicit operator Outcome<T, TError>(T value) =>
            new Outcome<T, TError>(Task.FromResult(ResultExtensions.Ok<T, TError>(value)));

        public static implicit operator Outcome<T, TError>(Task<T> value) =>
            new Func<Task<Result<T, TError>>>(async () => await value)();

        public static implicit operator Outcome<T, TError>(Task<TError> error) =>
            new Func<Task<Result<T, TError>>>(async () => await error)();

        public static implicit operator Outcome<T, TError>(TError error) =>
            new Outcome<T, TError>(Task.FromResult(ResultExtensions.Error<T, TError>(error)));

        public Outcome<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) =>
            TaskResultFunctions.Filter(Result, predicate, errorSelector);

        public Outcome<T, TError> IsErrorWhen(
            Func<T, bool> predicate,
            Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhen(Result, predicate, errorSelector);

        public Outcome<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            TaskResultFunctions.IsErrorWhenNull(Result, errorSelector);

        public Outcome<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            TaskResultFunctions.Map(Result, selector);

        public Outcome<TResult, TError> Map<TResult>(Func<T, Task<TResult>> selector) =>
            TaskResultFunctions.Map(Result, selector);

        public Outcome<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            TaskResultFunctions.MapError(Result, selector);

        public Outcome<T, TError> Do(Action action) => TaskResultFunctions.Do(Result, action);

        public Outcome<T, TError> DoWithError(Action<TError> action) => TaskResultFunctions.DoWithError(Result, action);

        public Outcome<T, TError> DoWith(Action<T> action) => TaskResultFunctions.DoWith(Result, action);

        public Outcome<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => TaskResultFunctions.FullMap(Result, selector, errorSelector);

        public Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            TaskResultFunctions.Match(Result, selector, errorSelector);

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

        public Outcome<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            TaskResultFunctions.FullCast<T, TResult, TError, TErrorResult>(Result);

        public Outcome<TResult, TResult> FullCast<TResult>() => FullCast<TResult, TResult>();

        public Outcome<T, TResult> CastError<TResult>() => TaskResultFunctions.CastError<T, TError, TResult>(Result);

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