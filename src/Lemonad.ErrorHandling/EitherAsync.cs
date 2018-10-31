using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    internal class EitherAsync<T, TError> : IEitherAsync<T, TError> {
        private readonly Task<IEither<T, TError>> _either;
        public Task<bool> HasValue => ResolveValue();
        public Task<bool> HasError => ResolveError();

        public EitherAsync(Task<IEither<T, TError>> either) => _either = either;

        private async Task<bool> ResolveValue() {
            var either = await _either.ConfigureAwait(false);
            if (either.HasValue)
                Value = either.Value;
            else
                Error = either.Error;
            return either.HasValue;
        }

        private async Task<bool> ResolveError() {
            var either = await _either.ConfigureAwait(false);
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