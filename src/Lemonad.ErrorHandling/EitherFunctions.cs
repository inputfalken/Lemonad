using System;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public static class Either {
        [Pure]
        private static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) =>
            new Either<TLeft, TRight>(default(TLeft), right, true);

        [Pure]
        private static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) =>
            new Either<TLeft, TRight>(left, default(TRight), false);

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
            Func<TRight, bool> predicate, TLeft onPredicateFailure) => source.IsRight
            ? (predicate(source.Right)
                ? Right<TLeft, TRight>(source.Right)
                : Left<TLeft, TRight>(onPredicateFailure))
            : Left<TLeft, TRight>(onPredicateFailure);

        public static Either<TLeft, TRight> RightWhen<TLeft, TRight>(this Either<TLeft, TRight> source,
            Func<TRight, bool> predicate) {
            if (source.Left.Equals(default(TLeft)))
                throw new ArgumentException("The either does not have a Left value.");
            return RightWhen(source, predicate, source.Left);
        }

        [Pure]
        public static Either<TLeftResult, TRightResult> Map<TLeftSource, TRightSource, TLeftResult, TRightResult>(
            this Either<TLeftSource, TRightSource> source, Func<TLeftSource, TLeftResult> leftSelector,
            Func<TRightSource, TRightResult> rightSelector) => source.IsRight
            ? Right<TLeftResult, TRightResult>(rightSelector(source.Right))
            : Left<TLeftResult, TRightResult>(leftSelector(source.Left));
    }
}