using System;
using System.Collections.Generic;
using System.Linq;

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

        // TODO Enable when Api more extension methods are in place.
        // public static implicit operator Either<TLeft, TRight>(TRight right) => Either.Right<TLeft, TRight>(right);
        //public static implicit operator Either<TLeft, TRight>(TLeft left) => Either.Left<TLeft, TRight>(left);

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
    }
}