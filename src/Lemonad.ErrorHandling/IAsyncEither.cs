using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    public interface IAsyncEither<out T, out TError> {
        /// <summary>
        ///     Represents a bool to indicate whether property <see cref="Value" /> is available to use.
        /// </summary>
        Task<bool> HasValue { get; }

        /// <summary>
        ///     Represents a bool to indicate whether property <see cref="Error" /> is available to use.
        /// </summary>
        Task<bool> HasError { get; }

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