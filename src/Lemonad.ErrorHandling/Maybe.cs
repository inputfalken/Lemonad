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

        /// <summary>
        /// Evaluates the <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="someAction">
        /// Is executed when the <see cref="Maybe{T}"/> has a value.
        /// </param>
        /// <param name="noneAction">
        /// Is executed when he <see cref="Maybe{T}"/> has no value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="someAction"/> or <paramref name="noneAction"/> needs to be executed.
        /// </exception>
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

        /// <summary>
        /// Evaluates the <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="someSelector">
        /// Is executed when the <see cref="Maybe{T}"/> has a value.
        /// </param>
        /// <param name="noneSelector">
        /// Is executed when he <see cref="Maybe{T}"/> has no value.
        /// </param>
        /// <typeparam name="TResult">
        /// The type returned by the functions <paramref name="someSelector"/> and <paramref name="noneSelector"/>.
        /// </typeparam>
        [Pure]
        public TResult Match<TResult>(Func<T, TResult> someSelector,
            Func<TResult> noneSelector) => someSelector == null
            ? throw new ArgumentNullException(nameof(someSelector))
            : (noneSelector == null
                ? throw new ArgumentNullException(nameof(noneSelector))
                : (HasValue ? someSelector(Value) : noneSelector()));

        /// <summary>
        /// Maps <typeparamref name="T"/>.
        /// </summary>
        /// <param name="selector">
        /// Is executed if <see cref="Maybe{T}"/> has a value.
        /// </param>
        /// <typeparam name="TResult">
        /// The type returned from the function <paramref name="selector"/>.
        /// </typeparam>
        [Pure]
        public Maybe<TResult>
            Map<TResult>(Func<T, TResult> selector) =>
            HasValue
                ? selector != null
                    ? Maybe.Some(selector(Value))
                    : throw new ArgumentNullException(nameof(selector))
                : Maybe<TResult>.None;

        /// <summary>
        ///  Filters the <typeparamref name="T"/> if <see cref="Maybe{T}"/> has a value.
        /// </summary>
        /// <param name="predicate">
        /// A function to test <typeparamref name="T"/>.
        /// </param>
        [Pure]
        public Maybe<T> Filter(
            Func<T, bool> predicate) => HasValue
            ? predicate != null
                ? predicate(Value)
                    ? this
                    : None
                : throw new ArgumentNullException()
            : None;

        /// <summary>
        /// Flamaps another <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Maybe{T}"/> as its return type.
        /// </param>
        /// <typeparam name="TResult">
        /// The type <typeparamref name="T"/> returned from the <paramref name="flatMapSelector"/> function.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TResult>(
            Func<T, Maybe<TResult>> flatMapSelector) => HasValue
            ? flatMapSelector?.Invoke(Value) ?? throw new ArgumentNullException(nameof(flatMapSelector))
            : Maybe<TResult>.None;

        /// <summary>
        /// Flamaps another <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Maybe{T}"/> as its return type.
        /// </param>
        /// <param name="resultSelector">
        /// A function whose in-parameters are <typeparamref name="T"/> and <typeparamref name="TFlatMap"/> which can return any type.
        /// </param>
        /// <typeparam name="TFlatMap">
        /// The value type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type returned by the function <paramref name="resultSelector"/>.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, Maybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) {
            if (HasValue)
                return flatMapSelector != null
                    ? FlatMap(x => flatMapSelector(x).Map(y => resultSelector != null
                        ? resultSelector(x, y)
                        : throw new ArgumentNullException(nameof(resultSelector))))
                    : throw new ArgumentNullException(nameof(flatMapSelector));

            return Maybe<TResult>.None;
        }

        /// <summary>
        /// Flamaps a <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="flatSelector">
        /// A function who expects a <see cref="Nullable{T}"/> as its return type.
        /// </param>
        /// <typeparam name="TResult">
        /// The type <typeparamref name="T"/> returned from the <paramref name="flatSelector"/> function.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TResult>(
            Func<T, TResult?> flatSelector) where TResult : struct =>
            HasValue ? flatSelector(Value).ConvertToMaybe() : Maybe<TResult>.None;

        /// <summary>
        /// Filters the <see cref="Maybe{T}"/> to see if <typeparamref name="T"/> is null.
        /// </summary>
        /// <returns>
        /// A <see cref="Maybe{T}"/> whose <typeparamref name="T"/> has value if <typeparamref name="T"/> is not null.
        /// </returns>
        [Pure]
        public Maybe<T> IsNoneWhenNull() =>
            IsNoneWhen(EquailtyFunctions.IsNull);

        /// <summary>
        ///  Filters the <typeparamref name="T"/> if <see cref="Maybe{T}"/> has a value.
        /// </summary>
        /// <param name="predicate">
        /// A function to test <typeparamref name="T"/>.
        /// </param>
        [Pure]
        public Maybe<T> IsNoneWhen(
            Func<T, bool> predicate) => HasValue
            ? predicate != null
                ? predicate(Value) == false
                    ? this
                    : None
                : throw new ArgumentNullException(nameof(predicate))
            : None;

        /// <summary>
        /// Flamaps another <see cref="Maybe{T}"/>.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Nullable{T}"/> as its return type.
        /// </param>
        /// <param name="resultSelector">
        /// A function whose in-parameters are <typeparamref name="T"/> and <typeparamref name="TFlatMap"/> which can return any type.
        /// </param>
        /// <typeparam name="TFlatMap">
        /// The value type of the <see cref="Nullable{T}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type returned by the function <paramref name="resultSelector"/>.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TSelector, TResult>(
            Func<T, TSelector?> selector,
            Func<T, TSelector, TResult> resultSelector) where TSelector : struct => FlatMap(
            src => selector(src).ConvertToMaybe().Map(elem => resultSelector(src, elem)));
    }
}
