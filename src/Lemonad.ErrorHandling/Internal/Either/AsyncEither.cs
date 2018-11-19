using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal class AsyncEither<T, TError> : IAsyncEither<T, TError> {
        private readonly Task<IEither<T, TError>> _either;
        private bool _hasError;
        private bool _hasValue;
        private bool _isAwaited;
        private T _value;
        private TError _error;

        public AsyncEither(Task<IEither<T, TError>> either) => _either = either;

        public Task<bool> HasError => Resolve(false);

        public Task<bool> HasValue => Resolve(true);

        /// Should this throw exception when <see cref="IAsyncEither{T,TError}.HasValue"/> is true?
        public TError Error {
            get => _isAwaited
                ? _error
                : throw new InvalidEitherStateException(
                    $"Can not access property '{nameof(IAsyncEither<T, TError>.Error)}' of '{nameof(IAsyncEither<T, TError>)}', before property '{nameof(IAsyncEither<T, TError>.HasError)}' or '{nameof(IAsyncEither<T, TError>.HasError)}' has been awaited."
                )
            ;
            private set => _error = value;
        }

        /// Should this throw exception when <see cref="IAsyncEither{T,TError}.HasError"/> is true?
        public T Value {
            get => _isAwaited
                ? _value
                : throw new InvalidEitherStateException(
                    $"Can not access property '{nameof(IAsyncEither<T, TError>.Value)}' of '{nameof(IAsyncEither<T, TError>)}', before property '{nameof(IAsyncEither<T, TError>.HasError)}' or '{nameof(IAsyncEither<T, TError>.HasError)}' has been awaited."
                );
            private set => _value = value;
        }

        private async Task<bool> Resolve(bool returnHasValue) {
            var either = await _either.ConfigureAwait(false);
            _isAwaited = true;
            _hasError = either.HasError;
            _hasValue = either.HasValue;
            if (either.HasValue) Value = either.Value;
            else Error = either.Error;

            return returnHasValue ? _hasValue : _hasError;
        }
    }
}