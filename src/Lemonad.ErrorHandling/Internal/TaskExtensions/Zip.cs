using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal.TaskExtensions {
    public static partial class TaskExtensions {
        public static Task<T3> Zip<T, T2, T3>(this Task<T> task, Task<T2> second, Func<T, T2, T3> resultSelector) =>
            task.FlatMap(arg => second, resultSelector);

        public static Task<T> Zip<T, T2>(this Task task, Task<T2> second, Func<T2, T> resultSelector) =>
            task.FlatMap(() => second, resultSelector);

        public static Task<T> Zip<T>(this Task task, Task second, Func<T> resultSelector) =>
            task.FlatMap(() => second, resultSelector);
    }
}