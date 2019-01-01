using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        /// <inheritdoc cref="ToErrorEnumerable{T,TError}(Lemonad.ErrorHandling.IAsyncResult{T,TError})" />
        public static async Task<IEnumerable<TError>>
            ToErrorEnumerable<T, TError>(this IAsyncResult<T, TError> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : EitherMethods.YieldErrors(await source.Either.ToTaskEither().ConfigureAwait(false));
    }
}