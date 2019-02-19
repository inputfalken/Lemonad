using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Extensions.Maybe.Task;

namespace Lemonad.ErrorHandling.Extensions.AsyncMaybe.Task {
    public static partial class Index {
        public static IAsyncMaybe<T> ToAsyncMaybe<T>(this Task<IAsyncMaybe<T>> source) 
            => Resolve(source).ToAsyncMaybe();

        private static async Task<IMaybe<T>> Resolve<T>(Task<IAsyncMaybe<T>> source) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            var awaitedSource = await source.ConfigureAwait(false);
            if (awaitedSource is null) throw new ArgumentNullException(nameof(awaitedSource));
            return await awaitedSource.HasValue.ConfigureAwait(false)
                ? ErrorHandling.Maybe.Value(awaitedSource.Value)
                : ErrorHandling.Maybe.None<T>();
        }
    }
}