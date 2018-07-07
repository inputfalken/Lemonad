using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    /// <summary>
    /// Async version of <see cref="Result{T,TError}"/>.
    /// </summary>
    public class Outcome<T, TError> {
        private readonly Task<Result<T, TError>> _result;

        public Outcome(Task<Result<T, TError>> result) =>
            _result = result ?? throw new ArgumentNullException(nameof(result));

        public static implicit operator Outcome<T, TError>(Task<Result<T, TError>> result) =>
            new Outcome<T, TError>(result);

        public static implicit operator Outcome<T, TError>(T value) =>
            new Outcome<T, TError>(Task.FromResult(ErrorHandling.Result.Ok<T, TError>(value)));

        public static implicit operator Outcome<T, TError>(Task<T> value) =>
            new Func<Task<Result<T, TError>>>(async () => await value)();

        public static implicit operator Outcome<T, TError>(Task<TError> error) =>
            new Func<Task<Result<T, TError>>>(async () => await error)();

        public static implicit operator Outcome<T, TError>(TError error) =>
            new Outcome<T, TError>(Task.FromResult(ErrorHandling.Result.Error<T, TError>(error)));

        [Pure]
        public Outcome<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            new Func<Task<Result<TResult, TError>>>(
                async () => (await _result.ConfigureAwait(false)).Map(selector)
            )();

        [Pure]
        public Outcome<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            new Func<Task<Result<T, TErrorResult>>>(
                async () => (await _result.ConfigureAwait(false)).MapError(selector)
            )();

        [Pure]
        public Outcome<TResult, TErrorResult> FullMap<TErrorResult, TResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) =>
            new Func<Task<Result<TResult, TErrorResult>>>(
                async () => (await _result.ConfigureAwait(false)).FullMap(selector, errorSelector)
            )();

        [Pure]
        public async Task<TResult> Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            (await _result.ConfigureAwait(false)).Match(selector, errorSelector);

        public async Task Match(Action<T> action, Action<TError> errorAction) =>
            (await _result.ConfigureAwait(false)).Match(action, errorAction);

        public Outcome<T, TError> Do(Action action) {
            return new Func<Task<Result<T, TError>>>(
                async () => (await _result.ConfigureAwait(false)).Do(action)
            )();
        }

        public Outcome<T, TError> DoWithError(Action<TError> action) {
            return new Func<Task<Result<T, TError>>>(
                async () => (await _result.ConfigureAwait(false)).DoWithError(action)
            )();
        }

        public Outcome<T, TError> DoWith(Action<T> action) =>
            new Func<Task<Result<T, TError>>>(
                async () => (await _result.ConfigureAwait(false)).DoWith(action)
            )();

        [Pure]
        public Outcome<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) =>
            new Func<Task<Result<T, TError>>>(
                async () => (await _result.ConfigureAwait(false)).Filter(predicate, errorSelector)
            )();

        [Pure]
        public Outcome<T, TError> IsErrorWhen(Func<T, bool> predicate, Func<TError> errorSelector) =>
            new Func<Task<Result<T, TError>>>(
                async () => (await _result.ConfigureAwait(false)).Filter(predicate, errorSelector)
            )();

        [Pure]
        public Outcome<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            IsErrorWhen(EquailtyFunctions.IsNull, errorSelector);

        [Pure]
        public Outcome<T, TResult> CastError<TResult>() => new Func<Task<Result<T, TResult>>>(
            async () => (await _result.ConfigureAwait(false)).CastError<TResult>()
        )();

        [Pure]
        public Outcome<TResult, TErrorResult> FullCast<TResult, TErrorResult>() {
            return new Func<Task<Result<TResult, TErrorResult>>>(
                async () => (await _result.ConfigureAwait(false)).FullCast<TResult, TErrorResult>()
            )();
        }

        [Pure]
        public Outcome<TResult, TError> Cast<TResult>() => new Func<Task<Result<TResult, TError>>>(
            async () => (await _result.ConfigureAwait(false)).Cast<TResult>()
        )();

        [Pure]
        public Outcome<TResult, TError> SafeCast<TResult>(Func<TError> errorSelector) =>
            new Func<Task<Result<TResult, TError>>>(
                async () => (await _result.ConfigureAwait(false)).SafeCast<TResult>(errorSelector)
            )();
    }
}
