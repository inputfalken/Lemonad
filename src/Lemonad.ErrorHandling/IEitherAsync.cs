using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal.TaskExtensions;

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
    internal class EitherAsync<T, TError> : IEitherAsync<T, TError> {
        private readonly Task<IEither<T, TError>> _either;
        public Task<bool> HasValue => ResolveValue();
        public Task<bool> HasError => ResolveError();
        
        public EitherAsync(Task<IEither<T, TError>> either) => _either = either;

        private async Task<bool> ResolveValue() {
            var either = await _either;
            if (either.HasValue)
                Value = either.Value;
            return either.HasValue;
        }

        private async Task<bool> ResolveError() {
            var either = await _either;
            if (either.HasError)
                Error = either.Error;
            return either.HasError;
        }

        public TError Error { get; private set; }

        public T Value { get; private set; }
    }
}