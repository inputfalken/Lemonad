using System;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal {
    internal static class CompositionExtensions {
        public static Func<T2> Compose<T1, T2>(
            this Func<T1> source,
            Func<T1, T2> selector
        ) => () => selector(source());

        public static Func<T1, T3> Compose<T1, T2, T3>(
            this Func<T1, T2> source,
            Func<T2, T3> selector
        ) => x => selector(source(x));

        public static Func<T1, Task<T3>> ComposeAsync<T1, T2, T3>(
            this Func<T1, Task<T2>> source,
            Func<T2, T3> selector
        ) => async x => selector(await source(x).ConfigureAwait(false));
    }
}