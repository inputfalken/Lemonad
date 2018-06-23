using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>> {
        public static Maybe<T> None { get; } = new Maybe<T>(default(T), false);

        /// <summary>
        /// Is true if there's a <typeparamref name="T"/> in the current state of the <see cref="Maybe{T}"/>.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Treat <typeparamref name="T"/> as enumerable with 0-1 elements in.
        /// This is handy when combining <see cref="Result{T,TError}"/> with LINQ's API.
        /// </summary>
        public IEnumerable<T> AsEnumerable => Yield(this);

        internal T Value { get; }

        // TODO add IEqualityComparer ctor overload.
        internal Maybe(T value, bool hasValue) {
            Value = value;
            HasValue = hasValue;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override int GetHashCode() => !HasValue ? 0 : (Value == null ? 1 : Value.GetHashCode());

        /// <inheritdoc />
        public int CompareTo(Maybe<T> other) {
            if (HasValue && !other.HasValue) return 1;
            if (!HasValue && other.HasValue) return -1;
            return Comparer<T>.Default.Compare(Value, other.Value);
        }

        public static bool operator <(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) < 0;

        public static bool operator <=(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) <= 0;

        public static bool operator >(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) > 0;

        public static bool operator >=(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) >= 0;

        /// <inheritdoc />
        public override string ToString() =>
            $"{(HasValue ? "Some" : "None")} ==> {typeof(Maybe<T>).ToHumanString()}{StringFunctions.PrettyTypeString(Value)}";

        private static IEnumerable<T> Yield(Maybe<T> maybe) {
            if (maybe.HasValue)
                yield return maybe.Value;
        }

        public void Match(Action<T> someAction,
            Action noneAction) {
            if (HasValue) {
                if (someAction == null)
                    throw new ArgumentNullException(nameof(someAction));
                someAction(Value);
            }
            else {
                if (noneAction == null)
                    throw new ArgumentNullException(nameof(noneAction));
                noneAction();
            }
        }

        [Pure]
        public TResult Match<TResult>(Func<T, TResult> someSelector,
            Func<TResult> noneSelector) => someSelector == null
            ? throw new ArgumentNullException(nameof(someSelector))
            : (noneSelector == null
                ? throw new ArgumentNullException(nameof(noneSelector))
                : (HasValue ? someSelector(Value) : noneSelector()));

        [Pure]
        public Maybe<TResult>
            Map<TResult>(Func<T, TResult> selector) =>
            HasValue
                ? selector != null
                    ? Maybe.Some(selector(Value))
                    : throw new ArgumentNullException(nameof(selector))
                : Maybe<TResult>.None;

        [Pure]
        public Maybe<T> IsSomeWhen(
            Func<T, bool> predicate) => HasValue
            ? predicate != null
                ? predicate(Value)
                    ? this
                    : None
                : throw new ArgumentNullException()
            : None;

        [Pure]
        public Maybe<TResult> FlatMap<TResult>(
            Func<T, Maybe<TResult>> selector) => HasValue
            ? selector?.Invoke(Value) ?? throw new ArgumentNullException(nameof(selector))
            : Maybe<TResult>.None;

        [Pure]
        public Maybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, Maybe<TSelector>> selector,
            Func<T, TSelector, TResult> resultSelector) {
            if (HasValue)
                return selector != null
                    ? FlatMap(x => selector(x).Map(y => resultSelector != null
                        ? resultSelector(x, y)
                        : throw new ArgumentNullException(nameof(resultSelector))))
                    : throw new ArgumentNullException(nameof(selector));

            return Maybe<TResult>.None;
        }

        [Pure]
        public Maybe<TResult> FlatMap<TResult>(
            Func<T, TResult?> selector) where TResult : struct =>
            HasValue ? selector(Value).ConvertToMaybe() : Maybe<TResult>.None;

        [Pure]
        public Maybe<T> IsNoneWhenNull() =>
            IsNoneWhen(EquailtyFunctions.IsNull);

        [Pure]
        public Maybe<T> IsNoneWhen(
            Func<T, bool> predicate) => HasValue
            ? predicate != null
                ? predicate(Value) == false
                    ? this
                    : None
                : throw new ArgumentNullException(nameof(predicate))
            : None;

        [Pure]
        public Maybe<TResult> FlatMap<TResult>(Maybe<TResult> maybe) => FlatMap(_ => maybe);

        [Pure]
        public Maybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector) where TSelector : struct => FlatMap(
            src => selector(src).ConvertToMaybe().Map(elem => resultSelector(src, elem)));
    }
}
