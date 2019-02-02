using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Result.Task;

namespace Lemonad.ErrorHandling.Extensions.AsyncMaybe {
    public static class Index {
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(
            this IAsyncMaybe<T> source,
            Func<TError> errorSelector
        ) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));

            async Task<IResult<T, TError>> Run(
                IAsyncMaybe<T> asyncMaybe,
                Func<TError> func
            ) {
                if (await asyncMaybe.HasValue.ConfigureAwait(false))
                    return ErrorHandling.Result.Value<T, TError>(asyncMaybe.Value);
                return ErrorHandling.Result.Error<T, TError>(func());
            }

            return Run(source, errorSelector).ToAsyncResult();
        }
    }
}