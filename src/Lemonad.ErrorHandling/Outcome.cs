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
    }
}