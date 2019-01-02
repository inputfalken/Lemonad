using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal.TaskExtensions {
    internal static partial class TaskExtensions {
        internal static async Task FlatMap(this Task source, Func<Task> selector) {
            await source.ConfigureAwait(false);
            await selector().ConfigureAwait(false);
        }

        internal static async Task<TSource> FlatMap<TSource>(this Task source, Func<Task> selector,
            Func<TSource> resultSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            await source.ConfigureAwait(false);
            await selector().ConfigureAwait(false);
            return resultSelector();
        }

        internal static async Task<TResult> FlatMap<TResult>(this Task source,
            Func<Task<TResult>> selector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            await source.ConfigureAwait(false);
            return await selector().ConfigureAwait(false);
        }

        internal static async Task<TResult> FlatMap<TTask, TResult>(this Task source,
            Func<Task<TTask>> selector, Func<TTask, TResult> resultSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            await source.ConfigureAwait(false);
            return resultSelector(await selector().ConfigureAwait(false));
        }

        internal static async Task FlatMap<TSource>(this Task<TSource> source, Func<TSource, Task> fn) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (fn is null) throw new ArgumentNullException(nameof(fn));
            await fn(await source.ConfigureAwait(false)).ConfigureAwait(false);
        }

        internal static async Task<TResult> FlatMap<TSource, TTask, TResult>(this Task<TSource> source,
            Func<TSource, Task<TTask>> selector, Func<TSource, TTask, TResult> resultSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            var element = await source.ConfigureAwait(false);
            return resultSelector(element, await selector(element).ConfigureAwait(false));
        }

        internal static async Task<TResult> FlatMap<TSource, TResult>(this Task<TSource> source,
            Func<TSource, Task> selector, Func<TSource, TResult> resultSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));
            var element = await source.ConfigureAwait(false);
            await selector(element).ConfigureAwait(false);
            return resultSelector(element);
        }

        internal static async Task<TResult> FlatMap<TSource, TResult>(this Task<TSource> source,
            Func<TSource, Task<TResult>> selector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return await selector(await source.ConfigureAwait(false)).ConfigureAwait(false);
        }
    }
}