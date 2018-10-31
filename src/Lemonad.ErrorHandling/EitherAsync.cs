using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    /// Try :awaiting <see cref="IEitherAsync{T,TError}.HasError"/> could perform await on a task with <typeparamref name="TError"/> and then assign <typeparamref name="TError"/> after.
    /// Try: awaiting <see cref="IEitherAsync{T,TError}.HasValue"/> could perform await on a task with <typeparamref name="T"/> and then assign <typeparamref name="T"/> after.
    internal class EitherAsync<T, TError> : IEitherAsync<T, TError> {
        private readonly Task<IEither<T, TError>> _either;
        public Task<bool> HasValue => ResolveValue();
        public Task<bool> HasError => ResolveError();

        public EitherAsync(Task<IEither<T, TError>> either) => _either = either;

        private async Task<bool> ResolveValue() {
            var either = await _either;
            if (either.HasValue)
                Value = either.Value;
            else
                Error = either.Error;
            return either.HasValue;
        }

        private async Task<bool> ResolveError() {
            var either = await _either;
            if (either.HasError)
                Error = either.Error;
            else
                Value = either.Value;
            return either.HasError;
        }

        public TError Error { get; private set; }

        public T Value { get; private set; }
    }
}