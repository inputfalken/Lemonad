using Lemonad.ErrorHandling.Extensions.Result.Task;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Extensions.Maybe {
    public static partial class Index {
        public static IAsyncMaybe<T> ToMaybeAsync<T>(this IMaybe<T> source)
            => new AsyncMaybe<T>(
                System.Threading.Tasks.Task.FromResult(source.ToResult(Unit.Selector)).ToAsyncResult()
            );
    }
}