using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    internal class EitherAsync<T, TError> : IEitherAsync<T, TError> {
        private readonly Task<IEither<T, TError>> _either;

        private bool _hasValue;
        private bool _hasError;
        private bool _isAwaited;

        public Task<bool> HasValue => _isAwaited
            ? Task.FromResult(_hasValue)
            : AwaitValue();

        public Task<bool> HasError => _isAwaited
            ? Task.FromResult(_hasError)
            : AwaitError();

        public EitherAsync(Task<IEither<T, TError>> either) => _either = either;

        private async Task<bool> AwaitValue() {
            var either = await _either.ConfigureAwait(false);
            _isAwaited = true;
            if (either.HasValue) {
                _hasValue = true;
                Value = either.Value;
            }
            else {
                _hasError = true;
                Error = either.Error;
            }

            return either.HasValue;
        }

        private async Task<bool> AwaitError() {
            var either = await _either.ConfigureAwait(false);
            _isAwaited = true;
            if (either.HasError) {
                _hasError = true;
                Error = either.Error;
            }
            else {
                _hasValue = true;
                Value = either.Value;
            }

            return either.HasError;
        }

        public TError Error { get; private set; }

        public T Value { get; private set; }
    }
}