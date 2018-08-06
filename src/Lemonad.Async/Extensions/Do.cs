using System;
using System.Threading.Tasks;

namespace Lemonad.Async.Extensions {
    public static partial class TaskExtensions {
        public static Task<T> Do<T>(this Task<T> task, Action<T> action) => task != null
            ? action != null
                ? Run(task, action)
                : throw new ArgumentNullException(nameof(action))
            : throw new ArgumentNullException(nameof(action));

        private static async Task<T> Run<T>(Task<T> task, Action<T> action) {
            var res = await task.ConfigureAwait(false);
            action(res);
            return res;
        }

        public static Task Do(this Task task, Action action) => task != null
            ? action != null
                ? Run(task, action)
                : throw new ArgumentNullException(nameof(action))
            : throw new ArgumentNullException(nameof(action));

        private static async Task Run(Task task, Action action) {
            await task.ConfigureAwait(false);
            action();
        }
    }
}