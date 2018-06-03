using System;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling {
    public struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, IComparable<Either<TLeft, TRight>> {
        public bool HasValue { get; }

        internal static Either<TLeft, TRight> Identity { get; } =
            new Either<TLeft, TRight>(default(TLeft), default(TRight), false);

        internal Either(TLeft left, TRight right, bool hasValue) {
            Left = left;
            HasValue = hasValue;
            Right = right;
        }

        internal TRight Right { get; }
        internal TLeft Left { get; }

        public bool Equals(Either<TLeft, TRight> other) {
            if (!HasValue && !other.HasValue)
                return EqualityComparer<TLeft>.Default.Equals(Left, other.Left);

            if (HasValue && other.HasValue)
                return EqualityComparer<TRight>.Default.Equals(Right, other.Right);

            return false;
        }

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

        public override bool Equals(object obj) => obj is Either<TLeft, TRight> option && Equals(option);

        public override int GetHashCode() =>
            !HasValue ? (Left == null ? 0 : Left.GetHashCode()) : (Right == null ? 1 : Right.GetHashCode());

        public int CompareTo(Either<TLeft, TRight> other) {
            if (HasValue && !other.HasValue) return 1;
            if (!HasValue && other.HasValue) return -1;

            return HasValue
                ? Comparer<TRight>.Default.Compare(Right, other.Right)
                : Comparer<TLeft>.Default.Compare(Left, other.Left);
        }
    }
}