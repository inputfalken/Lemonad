using System;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;
using Lemonad.ErrorHandling.Internal.TaskExtensions;

namespace Lemonad.ErrorHandling.Extensions.Task {
    public static partial class Index {
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