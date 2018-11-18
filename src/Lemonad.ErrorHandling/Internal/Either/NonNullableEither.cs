using System;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal readonly struct NonNullableEither<T, TError> : IEither<T, TError> {
        private readonly TError _error;
        private readonly T _value;
        public bool HasValue { get; }
        public bool HasError { get; }

        // TODO create custom exception.
        public TError Error => HasError ? _error : throw new Exception();

        // TODO create custom exception.
        public T Value => HasValue ? _value : throw new Exception();

        /// <summary>
        ///     Only one one <typeparamref name="T" /> and <typeparamref name="TError" /> can be available to use.
        /// </summary>
        /// <param name="value">
        ///     The potential value.
        /// </param>
        /// <param name="error">
        ///     The potential error.
        /// </param>
        /// <param name="hasError">
        ///     Is true when <paramref name="error" /> is available.
        /// </param>
        /// <param name="hasValue">
        ///     Is true when <paramref name="value" /> is available.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     When <see cref="HasValue" /> and <see cref="HasError" />  are both either false or true.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     When either <see cref="Value" /> or <see cref="Error" /> is null at the same the corresponding
        ///     <see cref="Boolean" /> value check is true.
        /// </exception>
        internal NonNullableEither(in T value, in TError error, bool hasError, bool hasValue) {
            if (hasError == hasValue)
                throw new ArgumentException(
                    $"{nameof(IEither<T, TError>)} properties \"{nameof(HasError)}\": {hasError} and \"{nameof(HasValue)}\": ({hasValue}), can not both be {hasValue}."
                );

            _value = value;
            _error = error;
            // Verify that the active value can never be null.
            if (_value.IsNull() && hasValue)
                throw new ArgumentNullException(
                    nameof(Value),
                    $"{nameof(IEither<T, TError>)} property \"{nameof(Value)}\" cannot be null."
                );

            // Verify that the active value can never be null.
            if (_error.IsNull() && hasError)
                throw new ArgumentNullException(
                    nameof(Error),
                    $"{nameof(IEither<T, TError>)} property \"{nameof(Error)}\" cannot be null."
                );

            HasValue = hasValue;
            HasError = hasError;
        }
    }
}