using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    /// <summary>
    /// Asynchronous wrapper of <see cref="Result{T,TError}"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type which is considered as successfull.
    /// </typeparam>
    /// <typeparam name="TError">
    /// The type which is considered as failure.
    /// </typeparam>
    public struct ResultAsync<T, TError> {
        public static implicit operator ResultAsync<T, TError>(Task<Result<T, TError>> value) =>
            new ResultAsync<T, TError>(value);

        public static implicit operator ResultAsync<T, TError>(T value) => Task.FromResult(value);
        public static implicit operator ResultAsync<T, TError>(TError error) => Task.FromResult(error);

        public static implicit operator ResultAsync<T, TError>(Task<T> value) {
            async Task<Result<T, TError>> Task() => await value.ConfigureAwait(false);
            return Task();
        }

        public static implicit operator ResultAsync<T, TError>(Task<TError> error) {
            async Task<Result<T, TError>> Task() => await error.ConfigureAwait(false);
            return Task();
        }

        private readonly Task<Result<T, TError>> _result;

        public ResultAsync<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) {
            async Task<Result<T, TError>> Task(Task<Result<T, TError>> x) =>
                (await x.ConfigureAwait(false)).Filter(predicate, errorSelector);

            return Task(_result);
        }

        public ResultAsync(Task<Result<T, TError>> result) => _result = result;

        public ResultAsync<TResult, TError> FlatMap<TErrorResult, TFlatMap, TResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) {
            async Task<Result<TResult, TError>> Task(Task<Result<T, TError>> x) =>
                (await x.ConfigureAwait(false)).FlatMap(flatMapSelector, resultSelector, errorSelector);

            return Task(_result);
        }
    }
}