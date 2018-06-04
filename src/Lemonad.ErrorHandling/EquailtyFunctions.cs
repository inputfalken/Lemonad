using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public static class EquailtyFunctions {
        [Pure]
        public static bool IsNull<TElement>(TElement element) =>
            EqualityComparer<TElement>.Default.Equals(element, default(TElement)) && element == null;
    }
}