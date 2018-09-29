using System;
using System.Text;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Either <typeparamref name="T" /> or <typeparamref name="TError" /> can have a value.
    /// </summary>
    /// <typeparam name="T">
    ///     The value type.
    /// </typeparam>
    /// <typeparam name="TError">
    ///     The error type.
    /// </typeparam>
    public readonly struct Either<T, TError> {
        /// <summary>
        ///     Gets a value indicating whether the current <see cref="Either{T,TError}" /> object has a valid value for
        ///     <typeparamref name="T" /> in
        ///     its underlying type.
        /// </summary>
        /// <returns>
        ///     true if the current <see cref="Either{T,TError}"></see> object has a value for <typeparamref name="T" />; false if
        ///     the current
        ///     <see cref="Either{T,TError}"></see> object has a valid value for <typeparamref name="TError" />.
        /// </returns>
        public bool HasValue { get; }

        /// <summary>
        ///     Gets a value indicating whether the current <see cref="Either{T,TError}" /> object has a valid value for
        ///     <typeparamref name="TError" /> in
        ///     its underlying type.
        /// </summary>
        /// <returns>
        ///     true if the current <see cref="Either{T,TError}"></see> object has a value for <typeparamref name="TError" />;
        ///     false if the current
        ///     <see cref="Either{T,TError}"></see> object has a valid value for <typeparamref name="T" />.
        /// </returns>
        public bool HasError { get; }

        /// <summary>
        ///     Gets the <typeparamref name="TError" /> value of the current <see cref="Either{T,TError}" />.
        ///     <para></para>
        ///     <para></para>
        ///     NOTE This is only a safe operation when <see cref="HasError"/> is true.
        /// </summary>
        /// <example>
        ///     <code language="c#">
        /// if (Either.HasError)
        /// {
        ///     // Safe to use.
        ///     Console.WriteLine(Either.Error)
        /// }
        /// </code>
        /// </example>
        public TError Error { get; }

        /// <summary>
        ///     Gets the <typeparamref name="T" /> value of the current <see cref="Either{T,TError}" />. 
        ///     <para></para>
        ///     <para></para>
        ///     NOTE This is only a safe operation if <see cref="HasValue"/> is true.
        /// </summary>
        /// <example>
        ///     <code language="c#">
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

            Value = value;
            Error = error;
            var valueIsNull = Value.IsNull();
            var errorIsNull = Error.IsNull();

            // Verify that the active value can never be null.
            if (valueIsNull && hasValue) {
                var msg = new StringBuilder()
                    .Append($"{nameof(Either<T, TError>)} property ")
                    .Append($"\"{nameof(Value)}\"")
                    .Append(" cannot be null.")
                    .ToString();
                throw new ArgumentNullException(nameof(Value),msg);
            }

            // Verify that the active value can never be null.
            if (errorIsNull && hasError) {
                var msg = new StringBuilder()
                    .Append($"{nameof(Either<T, TError>)} property ")
                    .Append($"\"{nameof(Error)}\"")
                    .Append(" cannot be null.")
                    .ToString();
                throw new ArgumentNullException(nameof(Error), msg);
            }

            HasValue = hasValue;
            HasError = hasError;
        }
    }
}