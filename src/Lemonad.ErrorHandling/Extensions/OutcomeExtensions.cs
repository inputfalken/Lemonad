using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Extensions {
    public static class OutcomeExtensions {
        /// <summary>
        ///     Converts the <see cref="Task" /> with <see cref="Result{T,TError}" /> into <see cref="Outcome{T,TError}" /> with
        ///     same functionality as <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="source">
        ///     The  <see cref="Result{T,TError}" /> wrapped in a <see cref="Task{TResult}" />.
        /// </param>
        /// <typeparam name="T">
        ///     The 'successful' value.
        /// </typeparam>
        /// <typeparam name="TError">
        ///     The 'failure' value.
        /// </typeparam>
        public static Outcome<T, TError> ToOutcome<T, TError>(this Task<Result<T, TError>> source) => source;
    }
}