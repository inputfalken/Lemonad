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

        [Pure]
        public Outcome<TResult, TError> Map<TResult>(Func<T, TResult> selector) => new Outcome<TResult, TError>(
            new Func<Task<Result<TResult, TError>>>(async () => (await _result.ConfigureAwait(false)).Map(selector))()
        );
    }
}