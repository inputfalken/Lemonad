using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions;

namespace Lemonad.ErrorHandling {
    public class Outcome<T, TError> {
        internal Task<Result<T, TError>> Result { get; }

        public static implicit operator Outcome<T, TError>(Task<Result<T, TError>> result) =>
            new Outcome<T, TError>(result);

        public static implicit operator Outcome<T, TError>(T value) =>
            new Outcome<T, TError>(Task.FromResult(ResultExtensions.Ok<T, TError>(value)));

        public static implicit operator Outcome<T, TError>(Task<T> value) =>
            new Func<Task<Result<T, TError>>>(async () => await value)();

        public static implicit operator Outcome<T, TError>(Task<TError> error) =>
            new Func<Task<Result<T, TError>>>(async () => await error)();

        public static implicit operator Outcome<T, TError>(TError error) =>
            new Outcome<T, TError>(Task.FromResult(ResultExtensions.Error<T, TError>(error)));

        public Outcome(Task<Result<T, TError>> result) =>
            Result = result ?? throw new ArgumentNullException(nameof(result));

        public Outcome<T, TError> Filter(Func<T, bool> predicate, Func<TError> errorSelector) =>
            Result.Filter(predicate, errorSelector);
    }
}