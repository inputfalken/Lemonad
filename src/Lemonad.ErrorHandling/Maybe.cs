using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>>, IEnumerable<T> {
        public bool HasValue { get; }

        internal T Value { get; }

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

        // TODO wrap value with quotes if type is string.
        public override string ToString() =>
            !HasValue ? $"Maybe<{PrettyType()}> -> NONE" : $"Maybe<{PrettyType()}> -> SOME({Value})";

        private static string PrettyType() {
            string BuildString(Type type) => type.IsGenericType
                ? $"{type.Name.Substring(0, type.Name.LastIndexOf("`", StringComparison.InvariantCulture))}<{string.Join(", ", type.GetGenericArguments().Select(BuildString))}>"
                : type.Name;

            return BuildString(typeof(T));
        }

        private static IEnumerable<T> Yield(Maybe<T> maybe) {
            if (maybe.HasValue)
                yield return maybe.Value;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => Yield(this).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => (this as IEnumerable<T>).GetEnumerator();
    }

    // TODO add null checks for all arguments...
}