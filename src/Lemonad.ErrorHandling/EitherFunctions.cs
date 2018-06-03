using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public static class Either {
        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) =>
            new Either<TLeft, TRight>(left, default(TRight), false);
    }
}