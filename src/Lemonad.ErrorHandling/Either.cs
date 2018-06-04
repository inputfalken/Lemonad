using System;
using System.Collections;
using System.Collections.Generic;

namespace Lemonad.ErrorHandling {
    public struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, IComparable<Either<TLeft, TRight>>,
        IEnumerable<TRight> {
        private readonly TRight _right;
        private readonly TLeft _left;
        private readonly Lazy<TRight> _lazyRight;
        private Lazy<TLeft> _lazyLeft;

        internal Either(TLeft left, TRight right, bool isRight) {
            IsRight = isRight;
            IsLeft = !isRight;
            _left = left;
            _right = right;
            _lazyLeft = null;
            _lazyRight = null;
        }

        public bool IsRight { get; }
        public bool IsLeft { get; }

        public TLeft Left => _lazyLeft != null ? _lazyLeft.Value : _left;
        public TRight Right => _lazyRight != null ? _lazyRight.Value : _right;

        internal Either(Func<TLeft> left, Func<TRight> right, bool isRight) {
            IsRight = isRight;
            IsLeft = !isRight;
            _right = default(TRight);
            _left = default(TLeft);
            _lazyRight = new Lazy<TRight>(right);
            _lazyLeft = new Lazy<TLeft>(left);
        }

        public bool Equals(Either<TLeft, TRight> other) {
            if (!IsRight && !other.IsRight)
                return EqualityComparer<TLeft>.Default.Equals(_left, other._left);

            if (IsRight && other.IsRight)
                return EqualityComparer<TRight>.Default.Equals(_right, other._right);

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

        // TODO Enable when Api more extension methods are in place.
        // public static implicit operator Either<TLeft, TRight>(TRight right) => Either.Right<TLeft, TRight>(right);
        //public static implicit operator Either<TLeft, TRight>(TLeft left) => Either.Left<TLeft, TRight>(left);

        private static IEnumerable<TRight> Yield(Either<TLeft, TRight> either) {
            if (either.IsRight)
                yield return either._right;
        }

        public IEnumerator<TRight> GetEnumerator() => Yield(this).GetEnumerator();

        public override bool Equals(object obj) => obj is Either<TLeft, TRight> option && Equals(option);

        public override int GetHashCode() =>
            !IsRight ? (_left == null ? 0 : _left.GetHashCode()) : (_right == null ? 1 : _right.GetHashCode());

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int CompareTo(Either<TLeft, TRight> other) {
            if (IsRight && !other.IsRight) return 1;
            if (!IsRight && other.IsRight) return -1;

            return IsRight
                ? Comparer<TRight>.Default.Compare(_right, other._right)
                : Comparer<TLeft>.Default.Compare(_left, other._left);
        }
    }
}