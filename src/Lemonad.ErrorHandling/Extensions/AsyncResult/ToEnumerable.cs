using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal.Either;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        /// <inheritdoc cref="ToEnumerable{T,TError}" />
        public static async Task<IEnumerable<T>> ToEnumerable<T, TError>(this IAsyncResult<T, TError> source)
            => source is null
                ? throw new ArgumentNullException(nameof(source))
                : EitherMethods.YieldValues(await source.Either.ToTaskEither().ConfigureAwait(false));
    }
}