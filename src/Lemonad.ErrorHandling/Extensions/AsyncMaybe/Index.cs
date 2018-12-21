using System;

namespace Lemonad.ErrorHandling.Extensions.AsyncMaybe {
    public static class Index {
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(
            this IAsyncMaybe<T> source,
            Func<TError> errorSelector
        ) {
            if (source is null)
                throw new ArgumentNullException(nameof(source));
            if (errorSelector is null)
                throw new ArgumentNullException(nameof(errorSelector));

            // TODO solve this..
            return null;
        }
    }
}