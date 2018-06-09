using System;
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

        public TResult Match<TResult>(
            Func<TLeft, TResult> leftselector, Func<TRight, TResult> rightSelector) =>
            IsRight ? rightSelector(Right) : leftselector(Left);

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
        public Either<TLeftResult, TRight> RightWhen<TLeftResult>(
            Func<TRight, bool> predicate, Func<TLeftResult> leftSelector) =>
            IsRight
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(Right)
                        ? Either.Right<TLeftResult, TRight>(Right)
                        : leftSelector == null
                            ? throw new ArgumentNullException(nameof(leftSelector))
                            : Either.Left<TLeftResult, TRight>(leftSelector())
                : leftSelector == null
                    ? throw new ArgumentNullException(nameof(leftSelector))
                    : Either.Left<TLeftResult, TRight>(leftSelector());

        [Pure]
        public Either<TLeftResult, TRight> LeftWhen<TLeftResult>(
            Func<TRight, bool> predicate, Func<TLeftResult> leftSelector) =>
            IsRight
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(Right)
                        ? leftSelector == null
                            ? throw new ArgumentNullException(nameof(leftSelector))
                            : Either.Left<TLeftResult, TRight>(leftSelector())
                        : Either.Right<TLeftResult, TRight>(Right)
                : leftSelector == null
                    ? throw new ArgumentNullException(nameof(leftSelector))
                    : Either.Left<TLeftResult, TRight>(leftSelector());

        [Pure]
        public Either<TLeftResult, TRight> LeftWhenNull<TLeftResult>(
            Func<TLeftResult> leftSelector) => RightWhen(x => !EquailtyFunctions.IsNull(x), leftSelector);

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

        public Either<TLeft, TRightResult> FlatMap<TRightResult>(
            Func<TRight, Either<TLeft, TRightResult>> rightSelector) => IsRight
            ? rightSelector?.Invoke(Right) ?? throw new ArgumentNullException(nameof(rightSelector))
            : Either.Left<TLeft, TRightResult>(Left);

        public Either<TLeftResult, TRightResult> MapLeftWithFlatMap<TLeftResult, TRightResult>(
            Func<TLeft, TLeftResult> lefSelector,
            Func<TRight, Either<TLeftResult, TRightResult>> rightSelector) {
            if (IsRight) return rightSelector?.Invoke(Right) ?? throw new ArgumentNullException(nameof(rightSelector));

            return lefSelector == null
                ? throw new ArgumentNullException(nameof(lefSelector))
                : Either.Left<TLeftResult, TRightResult>(lefSelector(Left));
        }

        public Either<TLeftResult, TRightResult> MapLeftWithFlatMap<TRightSelector, TLeftResult,
            TRightResult>(
            Func<TLeft, TLeftResult> lefSelector,
            Func<TRight, Either<TLeftResult, TRightSelector>> rightSelector,
            Func<TRight, TRightSelector, TRightResult> resultSelector) {
            if (IsRight)
                return Map(lefSelector, x => rightSelector(x).MapRight(y => resultSelector(x, y)))
                    .FlatMap(x => x);

            return lefSelector == null
                ? throw new ArgumentNullException(nameof(lefSelector))
                : Either.Left<TLeftResult, TRightResult>(lefSelector(Left));
        }

        public Either<TLeft, TRightResult> FlatMap<TRightSelector, TRightResult>(
            Func<TRight, Either<TLeft, TRightSelector>> rightSelector,
            Func<TRight, TRightSelector, TRightResult> resultSelector) => FlatMap(x =>
            rightSelector?.Invoke(x).MapRight(y => resultSelector == null
                ? throw new ArgumentNullException(nameof(resultSelector))
                : resultSelector(x, y)) ??
            throw new ArgumentNullException(nameof(rightSelector)));
    }
}