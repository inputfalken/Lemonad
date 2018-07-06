using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    /// <summary>
    /// Async version of <see cref="Result{T,TError}"/>.
    /// </summary>
    public class Outcome<T, TError> {
        private readonly Task<Result<T, TError>> _result;

        public Outcome(Task<Result<T, TError>> result) => _result = result;
    }
}