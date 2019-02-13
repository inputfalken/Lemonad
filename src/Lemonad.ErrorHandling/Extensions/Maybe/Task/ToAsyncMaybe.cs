using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Result.Task;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.TaskExtensions;

namespace Lemonad.ErrorHandling.Extensions.Maybe.Task {
    public static class Index {
        public static IAsyncMaybe<T> ToAsyncMaybe<T>(this Task<IMaybe<T>> source) =>
            new AsyncMaybe<T>(source.Map(y => y.ToResult(Unit.Selector)).ToAsyncResult());
    }
}