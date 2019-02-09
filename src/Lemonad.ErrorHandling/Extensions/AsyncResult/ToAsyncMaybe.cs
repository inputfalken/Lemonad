using System;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        public static IAsyncMaybe<T> ToAsyncMaybe<T, TError>(this IAsyncResult<T, TError> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            return new AsyncMaybe<T>(source.MapError(_ => Unit.Default));
        }
    }
}