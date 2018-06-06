using System;
using System.Diagnostics.Contracts;
using static Lemonad.ErrorHandling.EquailtyFunctions;

namespace Lemonad.ErrorHandling {
    public static class Either {
        // Consider using Left wen right is null & if TRight also got null as default value throw ArgumentNullException 
        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) => IsNull(right)
            ? throw new ArgumentNullException(nameof(right))
            : new Either<TLeft, TRight>(default(TLeft), right, true);

        // Consider using Right when left is null & if TRight also got null as default value throw ArgumentNullException 
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
        public static Either<TLeftResult, TRight> RightWhen<TLeftSource, TRight, TLeftResult>(
            this Either<TLeftSource, TRight> source,
            Func<TRight, bool> predicate, TLeftResult left) => source.IsRight
            ? (predicate(source.Right) ? Right<TLeftResult, TRight>(source.Right) : Left<TLeftResult, TRight>(left))
            : Left<TLeftResult, TRight>(left);

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