using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal.Either {
    internal static class EitherExtensions {
        internal static async Task<IEither<T, TError>> ToTaskEither<T, TError>(this IAsyncEither<T, TError> asyncEither)
            => await asyncEither.HasValue.ConfigureAwait(false)
                ? Result.Value<T, TError>(asyncEither.Value).Either
                : Result.Error<T, TError>(asyncEither.Error).Either;

        internal static IAsyncEither<T, TError> ToAsyncEither<T, TError>(this Task<IEither<T, TError>> taskEither) =>
            new AsyncEither<T, TError>(taskEither);
    }
}