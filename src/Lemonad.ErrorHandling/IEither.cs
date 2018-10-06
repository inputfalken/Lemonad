namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     Represents a data object who can either have <typeparamref name="T" /> or <typeparamref name="TError" /> present.
    /// </summary>
    /// <typeparam name="T">
    ///     Represents the type given by a successful operation.
    /// </typeparam>
    /// <typeparam name="TError">
    ///     Represents the type given by a failed operation.
    /// </typeparam>
    public interface IEither<out T, out TError> {
        /// <summary>
        ///     Represents a bool to indicate whether property <see cref="Value" /> is available to use.
        /// </summary>
        bool HasValue { get; }

        /// <summary>
        ///     Represents a bool to indicate whether property <see cref="Error" /> is available to use.
        /// </summary>
        bool HasError { get; }

        /// <summary>
        ///     Represents a property whose value is available when <see cref="HasError" /> is true.
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
        TError Error { get; }

        /// <summary>
        ///     Represents a property whose value is available when <see cref="HasValue" /> is true.
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
        T Value { get; }
    }
}