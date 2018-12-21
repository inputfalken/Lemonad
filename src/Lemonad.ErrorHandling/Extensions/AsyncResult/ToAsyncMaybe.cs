using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.AsyncResult {
    public static partial class Index {
        public static IAsyncMaybe<T> ToAsyncMaybe<T, TError>(this IAsyncResult<T, TError> source)
            => new AsyncMaybe<T>(source.MapError(_ => Unit.Default));
    }
}