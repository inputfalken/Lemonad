﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, IComparable<Either<TLeft, TRight>> {
        internal TRight Right { get; }
        internal TLeft Left { get; }

        internal Either(TLeft left, TRight right, bool isLeft, bool isRight) {
            IsRight = isRight;
            IsLeft = isLeft;
            Left = left;
            Right = right;
        }

        public bool IsRight { get; }
        public bool IsLeft { get; }

        public bool Equals(Either<TLeft, TRight> other) {
            if (!IsRight && !other.IsRight)
                return EqualityComparer<TLeft>.Default.Equals(Left, other.Left);

            if (IsRight && other.IsRight)
                return EqualityComparer<TRight>.Default.Equals(Right, other.Right);

            return false;
        }

        public override string ToString() =>
            $"{(IsRight ? "Right" : "Left")} ==> {typeof(Either<TLeft, TRight>).ToHumanString()}{StringFunctions.PrettyTypeString(IsRight ? (object) Right : Left)}";

        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => left.Equals(right);
        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) => !left.Equals(right);

        public static bool operator <(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.CompareTo(right) < 0;

        public static bool operator <=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.CompareTo(right) > 0;

        public static bool operator >=(Either<TLeft, TRight> left, Either<TLeft, TRight> right) =>
            left.CompareTo(right) >= 0;

        public static implicit operator Either<TLeft, TRight>(TRight right) =>
            Either.Right<TLeft, TRight>(right);

        public static implicit operator Either<TLeft, TRight>(TLeft left) => Either.Left<TLeft, TRight>(left);

        private static IEnumerable<TRight> YieldRight(Either<TLeft, TRight> either) {
            if (either.IsRight)
                yield return either.Right;
        }

        private static IEnumerable<TLeft> YieldLeft(Either<TLeft, TRight> either) {
            if (either.IsLeft)
                yield return either.Left;
        }

        public IEnumerable<TLeft> LeftEnumerable => YieldLeft(this);
        public IEnumerable<TRight> RightEnumerable => YieldRight(this);

        public override bool Equals(object obj) => obj is Either<TLeft, TRight> option && Equals(option);

        public override int GetHashCode() =>
            !IsRight ? (Left == null ? 0 : Left.GetHashCode()) : (Right == null ? 1 : Right.GetHashCode());

        public int CompareTo(Either<TLeft, TRight> other) {
            if (IsRight && !other.IsRight) return 1;
            if (!IsRight && other.IsRight) return -1;

            return IsRight
                ? Comparer<TRight>.Default.Compare(Right, other.Right)
                : Comparer<TLeft>.Default.Compare(Left, other.Left);
        }

        [Pure]
        public TResult Match<TResult>(
            Func<TLeft, TResult> leftselector, Func<TRight, TResult> rightSelector) {
            if (IsRight)
                return rightSelector != null
                    ? rightSelector(Right)
                    : throw new ArgumentNullException(nameof(rightSelector));

            return leftselector != null ? leftselector(Left) : throw new ArgumentNullException(nameof(leftselector));
        }

        public void Match(Action<TLeft> leftAction, Action<TRight> rightAction) {
            if (IsRight)
                if (rightAction != null)
                    rightAction(Right);
                else
                    throw new ArgumentNullException(nameof(rightAction));
            else if (leftAction != null)
                leftAction(Left);
            else
                throw new ArgumentNullException(nameof(leftAction));
        }

        public Either<TLeft, TRight> DoWhenRight(Action<TRight> action) {
            if (IsRight)
                if (action != null)
                    action.Invoke(Right);
                else
                    throw new ArgumentNullException(nameof(action));

            return this;
        }

        public Either<TLeft, TRight> Do(Action action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action();
            return this;
        }

        public Either<TLeft, TRight> DoWhenLeft(
            Action<TLeft> action) {
            if (IsLeft)
                if (action != null)
                    action.Invoke(Left);
                else
                    throw new ArgumentNullException(nameof(action));

            return this;
        }

        [Pure]
        public Either<TLeft, TRight> IsRightWhen(
            Func<TRight, bool> predicate, Func<TLeft> leftSelector) =>
            IsRight
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(Right)
                        ? Either.Right<TLeft, TRight>(Right)
                        : leftSelector == null
                            ? throw new ArgumentNullException(nameof(leftSelector))
                            : Either.Left<TLeft, TRight>(leftSelector())
                : leftSelector == null
                    ? throw new ArgumentNullException(nameof(leftSelector))
                    : Either.Left<TLeft, TRight>(Left);

        [Pure]
        public Either<TLeft, TRight> IsLeftWhen(
            Func<TRight, bool> predicate, Func<TLeft> leftSelector) =>
            IsRight
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(Right)
                        ? leftSelector == null
                            ? throw new ArgumentNullException(nameof(leftSelector))
                            : Either.Left<TLeft, TRight>(leftSelector())
                        : Either.Right<TLeft, TRight>(Right)
                : leftSelector == null
                    ? throw new ArgumentNullException(nameof(leftSelector))
                    : Either.Left<TLeft, TRight>(Left);

        [Pure]
        public Either<TLeft, TRight> LeftWhenNull(
            Func<TLeft> leftSelector) => IsLeftWhen(EquailtyFunctions.IsNull, leftSelector);

        [Pure]
        public Either<TLeft, TRight> RightWhenNull(
            Func<TLeft> leftSelector) => IsRightWhen(EquailtyFunctions.IsNull, leftSelector);

        [Pure]
        public Either<TLeftResult, TRightResult> Map<TLeftResult, TRightResult>(
            Func<TLeft, TLeftResult> leftSelector,
            Func<TRight, TRightResult> rightSelector) => IsLeft
            ? Either.Left<TLeftResult, TRightResult>(leftSelector != null
                ? leftSelector(Left)
                : throw new ArgumentNullException(nameof(leftSelector)))
            : Either.Right<TLeftResult, TRightResult>(rightSelector != null
                ? rightSelector(Right)
                : throw new ArgumentNullException(nameof(rightSelector)));

        [Pure]
        public Either<TLeft, TRightResult> MapRight<TRightResult>(Func<TRight, TRightResult> rightSelector) => IsRight
            ? rightSelector != null
                ? Either.Right<TLeft, TRightResult>(rightSelector(Right))
                : throw new ArgumentNullException(nameof(rightSelector))
            : Either.Left<TLeft, TRightResult>(Left);

        [Pure]
        public Either<TLeftResult, TRight> MapLeft<TLeftResult>(Func<TLeft, TLeftResult> leftSelector) => IsLeft
            ? leftSelector != null
                ? Either.Left<TLeftResult, TRight>(leftSelector(Left))
                : throw new ArgumentNullException(nameof(leftSelector))
            : Either.Right<TLeftResult, TRight>(Right);

        [Pure]
        public Either<TLeft, TRightResult> FlatMap<TRightResult>(
            Func<TRight, Either<TLeft, TRightResult>> rightSelector) => IsRight
            ? rightSelector?.Invoke(Right) ?? throw new ArgumentNullException(nameof(rightSelector))
            : Either.Left<TLeft, TRightResult>(Left);

        [Pure]
        public Either<TLeft, TRight> Flatten<TLeftResult, TRightResult>(
            Func<TRight, Either<TLeftResult, TRightResult>> rightSelector, Func<TLeftResult, TLeft> leftSelector) {
            if (IsRight) {
                if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));
                var selector = rightSelector(Right);
                if (selector.IsRight)
                    return Either.Right<TLeft, TRight>(Right);
                var thiss = this;
                return selector.Map(leftSelector, _ => thiss.Right);
            }

            return Either.Left<TLeft, TRight>(Left);
        }

        [Pure]
        public Either<TLeft, TRightResult> FlatMapRight<TLeftResult, TRightResult>(
            Func<TRight, Either<TLeftResult, TRightResult>> rightSelector, Func<TLeftResult, TLeft> leftSelector) {
            if (IsRight) {
                if (rightSelector == null) throw new ArgumentNullException(nameof(rightSelector));
                var selector = rightSelector(Right);
                return selector.IsRight
                    ? Either.Right<TLeft, TRightResult>(selector.Right)
                    : selector.MapLeft(leftSelector);
            }

            return Either.Left<TLeft, TRightResult>(Left);
        }

        [Pure]
        public Either<TLeft, TRightResult> FlatMap<TRightSelector, TRightResult>(
            Func<TRight, Either<TLeft, TRightSelector>> rightSelector,
            Func<TRight, TRightSelector, TRightResult> resultSelector) => FlatMap(x =>
            rightSelector?.Invoke(x).MapRight(y => resultSelector == null
                ? throw new ArgumentNullException(nameof(resultSelector))
                : resultSelector(x, y)) ??
            throw new ArgumentNullException(nameof(rightSelector)));
    }
}
