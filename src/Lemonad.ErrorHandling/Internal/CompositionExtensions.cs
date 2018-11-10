using System;

namespace Lemonad.ErrorHandling.Internal {
    internal static class CompositionExtensions {
        public static Func<T2> Compose<T1, T2>(this Func<T1> source, Func<T1, T2> selector) =>
            () => selector(source());

        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> source,
            Func<T2, T3> selector) => x => selector(source(x));
    }
}