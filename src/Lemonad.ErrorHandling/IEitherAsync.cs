using System.Threading.Tasks;

namespace Lemonad.ErrorHandling {
    public interface IEitherAsync<out T, out TError> {
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

    /// Try :awaiting <see cref="IEitherAsync{T,TError}.HasError"/> could perform await on a task with <typeparamref name="TError"/> and then assign <typeparamref name="TError"/> after.
    /// Try: awaiting <see cref="IEitherAsync{T,TError}.HasValue"/> could perform await on a task with <typeparamref name="T"/> and then assign <typeparamref name="T"/> after.
    internal readonly struct EitherAsync<T, TError> : IEitherAsync<T, TError> {
        public Task<bool> HasValue => throw new System.NotImplementedException();

        public Task<bool> HasError => throw new System.NotImplementedException();

        public TError Error => throw new System.NotImplementedException();

        public T Value => throw new System.NotImplementedException();
    }
}