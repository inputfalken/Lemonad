using System;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>> {
        internal static Maybe<T> Identity { get; } = new Maybe<T>(default(T), false);

        public bool HasValue { get; }
        public IEnumerable<T> Enumerable => Yield(this);

        internal T Value { get; }

        // TODO add IEqualityComparer ctor overload.
        internal Maybe(T value, bool hasValue) {
            Value = value;
            HasValue = hasValue;
        }

        public bool Equals(Maybe<T> other) {
            if (!HasValue && !other.HasValue)
                return true;
            if (HasValue && other.HasValue)
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            return false;
        }

        public static implicit operator Maybe<T>(T item) => item.Some();
        
        public override bool Equals(object obj) => obj is Maybe<T> maybe && Equals(maybe);

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        public override int GetHashCode() => !HasValue ? 0 : (Value == null ? 1 : Value.GetHashCode());

        public int CompareTo(Maybe<T> other) {
            if (HasValue && !other.HasValue) return 1;
            if (!HasValue && other.HasValue) return -1;
            return Comparer<T>.Default.Compare(Value, other.Value);
        }

        public static bool operator <(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) < 0;

        public static bool operator <=(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) <= 0;

        public static bool operator >(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) > 0;

        public static bool operator >=(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) >= 0;

        public override string ToString() =>
            $"{(HasValue ? "Some" : "None")} ==> {typeof(Maybe<T>).ToHumanString()}{StringFunctions.PrettyTypeString(Value)}";

        private static IEnumerable<T> Yield(Maybe<T> maybe) {
            if (maybe.HasValue)
                yield return maybe.Value;
        }
    }
}