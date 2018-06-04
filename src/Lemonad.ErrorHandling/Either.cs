using System;
using System.Collections;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling {
    public struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, IComparable<Either<TLeft, TRight>>,
        IEnumerable<TRight> {
        public bool IsRight { get; }
        public bool IsLeft { get; }
        internal bool IsNeither { get; }

        internal Either(TLeft left, TRight right, bool? isRight) {
            if (isRight.HasValue) {
                IsRight = isRight.Value;
                IsLeft = !isRight.Value;
                IsNeither = false;
            }
            else {
                IsRight = false;
                IsLeft = false;
                IsNeither = true;
            }

            Left = left;
            Right = right;
        }

        internal TRight Right { get; }
        internal TLeft Left { get; }

        public bool Equals(Either<TLeft, TRight> other) {
            if (!IsRight && !other.IsRight)
                return EqualityComparer<TLeft>.Default.Equals(Left, other.Left);

            if (IsRight && other.IsRight)
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

        private static IEnumerable<TRight> Yield(Either<TLeft, TRight> either) {
            if (either.IsRight)
                yield return either.Right;
        }

        public IEnumerator<TRight> GetEnumerator() => Yield(this).GetEnumerator();

        public override bool Equals(object obj) => obj is Either<TLeft, TRight> option && Equals(option);

        public override int GetHashCode() =>
            !IsRight ? (Left == null ? 0 : Left.GetHashCode()) : (Right == null ? 1 : Right.GetHashCode());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int CompareTo(Either<TLeft, TRight> other) {
            if (IsRight && !other.IsRight) return 1;
            if (!IsRight && other.IsRight) return -1;

            return IsRight
                ? Comparer<TRight>.Default.Compare(Right, other.Right)
                : Comparer<TLeft>.Default.Compare(Left, other.Left);
        }
    }
}