using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    internal static class EquailtyFunctions {
        [Pure]
        internal static bool IsNull<TElement>(TElement element) {
            var x = EqualityComparer<TElement>.Default.Equals(element, default) && element == null;
            return x;
        }
    }
}