using System;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal readonly struct NonNullableEither<T, TError> : IEither<T, TError> {
        public bool HasValue { get; }
        public bool HasError { get; }
        public TError Error { get; }
        public T Value { get; }

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
                    $"{nameof(IEither<T, TError>)} properties \"{nameof(HasError)}\": ({hasError}) and \"{nameof(HasValue)}\": ({hasValue}), can not have the same value ({hasValue})."
                );

            Value = value;
            Error = error;

            if (Value.IsNull() && hasValue)
                throw new ArgumentNullException(
                    nameof(value),
                    $"{nameof(IEither<T, TError>)} property \"{nameof(Value)}\" cannot be null when property \"{nameof(HasValue)}\" is expected to be true."
                );

            if (Error.IsNull() && hasError)
                throw new ArgumentNullException(
                    nameof(error),
                    $"{nameof(IEither<T, TError>)} property \"{nameof(Error)}\" cannot be null when property \"{nameof(hasError)}\" is expected to be true."
                );

            HasValue = hasValue;
            HasError = hasError;
        }
    }
}