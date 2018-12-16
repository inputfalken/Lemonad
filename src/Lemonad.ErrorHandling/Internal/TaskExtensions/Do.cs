using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal.TaskExtensions {
    internal static partial class TaskExtensions {
        internal static Task<T> Do<T>(this Task<T> task, Action<T> action) => task is null == false
            ? action is null == false
                ? Run(task, action)
                : throw new ArgumentNullException(nameof(action))
            : throw new ArgumentNullException(nameof(action));

        internal static Task Do(this Task task, Action action) => task is null == false
            ? action is null == false
                ? Run(task, action)
                : throw new ArgumentNullException(nameof(action))
            : throw new ArgumentNullException(nameof(action));

        private static async Task<T> Run<T>(Task<T> task, Action<T> action) {
            var res = await task.ConfigureAwait(false);
            action(res);
            return res;
        }

        private static async Task Run(Task task, Action action) {
            await task.ConfigureAwait(false);
            action();
        }
    }
}