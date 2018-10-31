﻿using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    internal class EitherAsync<T, TError> : IEitherAsync<T, TError> {
        private readonly Task<IEither<T, TError>> _either;
        public Task<bool> HasValue => ResolveValue();
        public Task<bool> HasError => ResolveError();
        private bool _hasValue;
        private bool _hasError;
        private bool _isAwaited;

        public EitherAsync(Task<IEither<T, TError>> either) => _either = either;

        private async Task<bool> ResolveValue() {
            // Avoid redundant awaits
            if (_isAwaited) return _hasValue;
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

        private async Task<bool> ResolveError() {
            // Avoid redundant awaits
            if (_isAwaited) return _hasError;
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