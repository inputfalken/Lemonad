using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal {
    internal static class CompositionExtensions {
        public static Func<T2> Compose<T1, T2>(this Func<T1> source, Func<T1, T2> selector) {
            return () => selector(source());
        }

        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> source, Func<T2, T3> selector) {
            return x => selector(source(x));
        }

        public static Func<T1, Task<T3>>
            ComposeAsync<T1, T2, T3>(this Func<T1, Task<T2>> source, Func<T2, T3> selector) {
            return async x => selector(await source(x).ConfigureAwait(false));
        }
    }

    public static class NullableExtensions {
        public static TResult? Map<T, TResult>(this T? source, Func<T, TResult> selector)
            where T : struct where TResult : struct =>
            source.HasValue
                ? (TResult?) selector(source.Value)
                : null;
    }
}