using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.TaskExtensions;

namespace Lemonad.ErrorHandling.Extensions.Task {
    public static class Index {
        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T> source, Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));

            return AsyncResult<T, TError>.Factory(source.Map(x => x.ToResult(predicate, errorSelector).Either));
        }

        public static IAsyncResult<T, TError> ToAsyncResult<T, TError>(this Task<T?> source,
            Func<TError> errorSelector)
            where T : struct {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (errorSelector is null) throw new ArgumentNullException(nameof(errorSelector));
            return AsyncResult<T, TError>.Factory(source.Map(x => x.ToResult(errorSelector).Either));
        }

        public static IAsyncResult<T, TError> ToAsyncResultError<T, TError>(this Task<TError> source,
            Func<TError, bool> predicate,
            Func<TError, T> valueSelector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));
            return AsyncResult<T, TError>.Factory(source.Map(x => x.ToResultError(predicate, valueSelector).Either));
        }
    }
}