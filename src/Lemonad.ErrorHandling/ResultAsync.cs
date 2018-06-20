using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    public class ResultAsync<T, TError> {
        internal Task<TError> Error { get; }
        internal Task<T> Value { get; }
        public bool HasValue { get; }
        public bool HasError { get; }

        [Pure]
        public async Task<TResult> Match<TResult>(
            Func<T, TResult> selector, Func<TError, TResult> errorSelector) {
            if (HasError)
                return errorSelector != null
                    ? errorSelector(await Error.ConfigureAwait(false))
                    : throw new ArgumentNullException(nameof(errorSelector));

            return selector != null
                ? selector(await Value.ConfigureAwait(false))
                : throw new ArgumentNullException(nameof(selector));
        }

        public async Task Match(Action<T> action, Action<TError> errorAction) {
            if (HasError)
                if (errorAction != null)
                    errorAction(await Error.ConfigureAwait(false));
                else
                    throw new ArgumentNullException(nameof(errorAction));
            else if (action != null)
                action(await Value.ConfigureAwait(false));
            else
                throw new ArgumentNullException(nameof(action));
        }
    }
}