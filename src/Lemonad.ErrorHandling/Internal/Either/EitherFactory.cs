using System;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal static class EitherFactory {
        internal static IEither<T1, T2> CreateError<T1, T2>(in T2 error) =>
            new NonNullableEither<T1, T2>(default, error, true, false);

        internal static IEither<T1, T2> CreateValue<T1, T2>(in T1 value) =>
            new NonNullableEither<T1, T2>(value, default, false, true);

        internal static IEither<T, TError> FromNullable<T, TError>(this T? nullable, Func<TError> errorSelector)
            where T : struct => nullable.HasValue
            ? CreateValue<T, TError>(nullable.Value)
            : CreateError<T, TError>(errorSelector());
    }
}