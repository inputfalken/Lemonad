using System;
using System.Diagnostics.Contracts;
using static Lemonad.ErrorHandling.EquailtyFunctions;

namespace Lemonad.ErrorHandling {
    public static class Either {
        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) => IsNull(right)
            ? throw new ArgumentNullException(nameof(right))
            : new Either<TLeft, TRight>(default(TLeft), right, true);

        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) => IsNull(left)
            ? throw new ArgumentNullException(nameof(left))
            : new Either<TLeft, TRight>(left, default(TRight), false);

        [Pure]
        public static Either<TLeft, TRight> ToEitherRight<TLeft, TRight>(this TRight right) =>
            Right<TLeft, TRight>(right);

        [Pure]
        public static Either<TLeft, TRight> ToEitherLeft<TLeft, TRight>(this TLeft left) => Left<TLeft, TRight>(left);

        [Pure]
        public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this Maybe<TRight> source, TLeft left) =>
            source.HasValue ? Right<TLeft, TRight>(source.Value) : Left<TLeft, TRight>(left);

        [Pure]
        public static Either<TLeft, TRight> ToEither<TLeft, TRight>(this TRight source, TLeft left) =>
            source == null ? Left<TLeft, TRight>(left) : Right<TLeft, TRight>(source);

        [Pure]
        public static Either<TLeft, TRight> RightWhen<TLeft, TRight>(this Either<TLeft, TRight> source,
            Func<TRight, bool> predicate, TLeft left) {
            return source.IsRight
                ? (predicate(source.Right) ? Right<TLeft, TRight>(source.Right) : Left<TLeft, TRight>(left))
                : Left<TLeft, TRight>(left);
        }

        public static Either<TLeft, TRight> RightWhen<TLeft, TRight>(this Either<TLeft, TRight> source,
            Func<TRight, bool> predicate) => RightWhen(source, predicate, source.Left);

        [Pure]
        public static Either<TLeftResult, TRightResult> Map<TLeftSource, TRightSource, TLeftResult, TRightResult>(
            this Either<TLeftSource, TRightSource> source, Func<TLeftSource, TLeftResult> leftSelector,
            Func<TRightSource, TRightResult> rightSelector) => new Either<TLeftResult, TRightResult>(
            Compose(leftSelector, source.Left),
            Compose(rightSelector, source.Right),
            source.IsRight);

        private static Func<TResult> Compose<TSource, TResult>(Func<TSource, TResult> fn, TSource source) =>
            () => fn(source);
    }
}