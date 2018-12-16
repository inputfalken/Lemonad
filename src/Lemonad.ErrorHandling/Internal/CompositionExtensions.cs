using System;

namespace Lemonad.ErrorHandling.Internal {
    internal static class CompositionExtensions {
        public static Func<T2> Compose<T1, T2>(this Func<T1> source, Func<T1, T2> selector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return () => selector(source());
        }

        public static Func<T1, T3> Compose<T1, T2, T3>(this Func<T1, T2> source, Func<T2, T3> selector) {
            if (source is null) throw new ArgumentNullException(nameof(source));
            if (selector is null) throw new ArgumentNullException(nameof(selector));
            return x => selector(source(x));
        }
    }
}