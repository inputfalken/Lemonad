using System;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling {
    public static class Validator {
        public static IValidator<T, TError> Value<T, TError>(T value) => new Validator<T, TError>(value);

        public static IValidator<T, TError> Value<T, TError>(
            T value,
            Func<T, bool> predicate,
            Func<T, TError> errorSelector
        ) => new Validator<T, TError>(value).Validate(predicate, errorSelector);

        public static IValidator<T, TError> Value<T, TError>(
            T value,
            Func<T, bool> predicate,
            TError errorSelector
        ) => new Validator<T, TError>(value).Validate(predicate, errorSelector);
    }
}