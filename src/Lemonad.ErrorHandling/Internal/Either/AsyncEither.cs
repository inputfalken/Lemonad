using System.Threading.Tasks;
using Lemonad.ErrorHandling.Exceptions;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal class AsyncEither<T, TError> : IAsyncEither<T, TError> {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Mutations = new object();
        private readonly Task<IEither<T, TError>> _either;
        private bool _hasError;

        private bool _hasValue;
        private bool _isAwaited;
        private T _value;
        private TError _error;

        public AsyncEither(Task<IEither<T, TError>> either) => _either = either;

        public Task<bool> HasError => _isAwaited
            ? Task.FromResult(_hasError)
            : ResolveError();

        public Task<bool> HasValue => _isAwaited
            ? Task.FromResult(_hasValue)
            : ResolveValue();

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

        private async Task<bool> ResolveError() {
            var either = await _either.ConfigureAwait(false);
            lock (Mutations) {
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
        }

        private async Task<bool> ResolveValue() {
            var either = await _either.ConfigureAwait(false);
            lock (Mutations) {
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
        }
    }
}