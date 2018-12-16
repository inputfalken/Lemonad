using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal.TaskExtensions {
    internal static partial class TaskExtensions {
        internal static async Task<TResult> Map<TSource, TResult>(this Task<TSource> task,
            Func<TSource, TResult> selector) {
            if (task is null) throw new ArgumentNullException(nameof(task));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return selector(await task.ConfigureAwait(false));
        }

        internal static async Task<TResult> Map<TResult>(this Task task,
            Func<TResult> selector) {
            if (task is null) throw new ArgumentNullException(nameof(task));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            await task.ConfigureAwait(false);
            return selector();
        }
    }
}