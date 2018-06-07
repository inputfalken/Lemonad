using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public static class Either {
        public static IEnumerable<TLeft> GetEitherLefts<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> enumerable) => enumerable.SelectMany(x => x.LeftEnumerable);

        public static IEnumerable<TRight> GetEitherRights<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> enumerable) => enumerable.SelectMany(x => x.RightEnumerable);

        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) =>
            new Either<TLeft, TRight>(default(TLeft), right, false, true);

        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) =>
            new Either<TLeft, TRight>(left, default(TRight), true, false);

        [Pure]
        public static Either<TLeft, TRight>
            ToEither<TLeft, TRight>(this Maybe<TRight> source, Func<TLeft> leftSelector) =>
            source.HasValue
                ? Right<TLeft, TRight>(source.Value)
                : (leftSelector != null
                    ? Left<TLeft, TRight>(leftSelector())
                    : throw new ArgumentNullException(nameof(leftSelector)));

        [Pure]
        public static Either<TLeft, TRight>
            ToEither<TLeft, TRight>(this TRight? source, Func<TLeft> leftSelector) where TRight : struct =>
            source.HasValue
                ? Right<TLeft, TRight>(source.Value)
                : (leftSelector != null
                    ? Left<TLeft, TRight>(leftSelector())
                    : throw new ArgumentNullException(nameof(leftSelector)));

        [Pure]
        public static Either<TLeft, TRight> ToEitherRight<TLeft, TRight>(this TRight right) =>
            Right<TLeft, TRight>(right);

        [Pure]
        public static Either<TLeft, TRight> ToEitherLeft<TLeft, TRight>(this TLeft left) => Left<TLeft, TRight>(left);

        [Pure]
        public static Maybe<TRight> ConvertToMaybe<TLeft, TRight>(this Either<TLeft, TRight> source) =>
            source.IsRight ? source.Right.Some() : Maybe<TRight>.Identity;

        [Pure]
        public static Either<TLeftResult, TRight> RightWhen<TLeftSource, TRight, TLeftResult>(
            this Either<TLeftSource, TRight> source,
            Func<TRight, bool> predicate, Func<TLeftResult> leftSelector) =>
            source.IsRight
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(source.Right)
                        ? Right<TLeftResult, TRight>(source.Right)
                        : leftSelector == null
                            ? throw new ArgumentNullException(nameof(leftSelector))
                            : Left<TLeftResult, TRight>(leftSelector())
                : leftSelector == null
                    ? throw new ArgumentNullException(nameof(leftSelector))
                    : Left<TLeftResult, TRight>(leftSelector());

        [Pure]
        public static Either<TLeftResult, TRight> LeftWhen<TLeftSource, TRight, TLeftResult>(
            this Either<TLeftSource, TRight> source, Func<TRight, bool> predicate, Func<TLeftResult> leftSelector) =>
            source.IsRight
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(source.Right)
                        ? leftSelector == null
                            ? throw new ArgumentNullException(nameof(leftSelector))
                            : Left<TLeftResult, TRight>(leftSelector())
                        : Right<TLeftResult, TRight>(source.Right)
                : leftSelector == null
                    ? throw new ArgumentNullException(nameof(leftSelector))
                    : Left<TLeftResult, TRight>(leftSelector());

        [Pure]
        public static Either<TLeftResult, TRight> LeftWhenNull<TLeftSource, TRight, TLeftResult>(
            this Either<TLeftSource, TRight> source, Func<TLeftResult> leftSelector) =>
            source.RightWhen(x => !EquailtyFunctions.IsNull(x), leftSelector);

        [Pure]
        public static Either<TLeftResult, TRightResult> Map<TLeftSource, TRightSource, TLeftResult, TRightResult>(
            this Either<TLeftSource, TRightSource> source, Func<TLeftSource, TLeftResult> leftSelector,
            Func<TRightSource, TRightResult> rightSelector) => source.IsLeft
            ? Left<TLeftResult, TRightResult>(leftSelector(source.Left))
            : Right<TLeftResult, TRightResult>(rightSelector(source.Right));
    }
}