using System;

namespace Lemonad.ErrorHandling {
    /// <summary>
    /// Contains either <typeparamref name="T"/> or <typeparamref name="TError"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The value type.
    /// </typeparam>
    /// <typeparam name="TError">
    /// The error type.
    /// </typeparam>
    public readonly struct Either<T, TError> {
        /// <summary>
        ///     Is true if there's a <typeparamref name="T" /> in the current state of the <see cref="Result{T,TError}" />.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        ///     Is true if there's a <typeparamref name="TError" /> in the current state of the <see cref="Result{T,TError}" />.
        /// </summary>
        public bool HasError { get; }

        /// <summary>
        /// The potential <typeparamref name="TError"/>.
        /// </summary>
        /// <remarks>
        /// Verify with <see cref="HasError"/> before using this, unless your certain that the error exists.
        /// </remarks>
        /// <example>
        /// <code language="c#">
        /// if (Either.HasError)
        /// {
        ///     // Safe to use.
        ///     Console.WriteLine(Either.Error)
        /// }
        /// </code>
        /// </example>
        public TError Error { get; }

        /// <summary>
        /// The potential <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>
        /// Verify with <see cref="HasValue"/> before using this, unless your certain that the value exists.
        /// </remarks>
        /// <example>
        /// <code language="c#">
        /// if (Either.HasValue)
        /// {
        ///     // Safe to use.
        ///     Console.WriteLine(Either.Value)
        /// }
        /// </code>
        /// </example>
        public T Value { get; }

        public Either(in T value, in TError error, bool hasError, bool hasValue) {
            if (hasError == hasValue)
                throw new ArgumentException(
                    $"Can never have the same value {nameof(hasError)} ({hasError}) and {nameof(hasValue)} ({hasValue})."
                );

            HasValue = hasValue;
            HasError = hasError;
            Value = value;
            Error = error;
        }
    }
}