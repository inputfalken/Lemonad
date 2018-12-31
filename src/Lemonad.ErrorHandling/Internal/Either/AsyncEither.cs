using System.Threading;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal class AsyncEither<T, TError> : IAsyncEither<T, TError> {
        private readonly Task<IEither<T, TError>> _either;
        private TError _error;
        private bool _hasError;
        private bool _hasValue;
        private bool _isAssigned;
        private readonly SemaphoreSlim _semaphoreSlim;
        private T _value;

        public AsyncEither(Task<IEither<T, TError>> either) {
            _either = either;
            _semaphoreSlim = new SemaphoreSlim(1, 1);
        }

        /// Should this throw exception when
        /// <see cref="IAsyncEither{T,TError}.HasValue" />
        /// is true?
        public TError Error {
            get => _isAssigned
                ? _error
                : throw new InvalidEitherStateException(
                    $"Can not access property '{nameof(IAsyncEither<T, TError>.Error)}' of '{nameof(IAsyncEither<T, TError>)}', before property '{nameof(IAsyncEither<T, TError>.HasError)}' or '{nameof(IAsyncEither<T, TError>.HasError)}' has been awaited."
                )
            ;
            private set => _error = value;
        }

        public Task<bool> HasError => _isAssigned ? Task.FromResult(_hasError) : Resolve(false);

        public Task<bool> HasValue => _isAssigned ? Task.FromResult(_hasValue) : Resolve(true);

        /// Should this throw exception when
        /// <see cref="IAsyncEither{T,TError}.HasError" />
        /// is true?
        public T Value {
            get => _isAssigned
                ? _value
                : throw new InvalidEitherStateException(
                    $"Can not access property '{nameof(IAsyncEither<T, TError>.Value)}' of '{nameof(IAsyncEither<T, TError>)}', before property '{nameof(IAsyncEither<T, TError>.HasError)}' or '{nameof(IAsyncEither<T, TError>.HasError)}' has been awaited."
                );
            private set => _value = value;
        }

        private async Task AssignProperties() {
            var either = await _either.ConfigureAwait(false);
            if (either.HasValue) Value = either.Value;
            else Error = either.Error;

            _hasError = either.HasError;
            _hasValue = either.HasValue;
            _isAssigned = true;
        }

        private async Task<bool> Resolve(bool returnHasValue) {
            if (_isAssigned) return returnHasValue ? _hasValue : _hasError;
            await _semaphoreSlim.WaitAsync().ConfigureAwait(false);
            try {
                await AssignProperties()
                    .ConfigureAwait(false);
            }
            finally {
                _semaphoreSlim.Release();
            }

            return returnHasValue ? _hasValue : _hasError;
        }
    }
}