using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Lemonad.ErrorHandling.Extensions;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     A data-structure commonly used for error-handling where value may or may not be present.
    /// </summary>
    /// <typeparam name="T">
    ///     The potential value.
    /// </typeparam>
    public readonly struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>> {
        public static Maybe<T> None { get; } = new Maybe<T>(ResultExtensions.Error<T, Unit>(default));

        /// <summary>
        ///     Is true if there's a <typeparamref name="T" /> in the current state of the <see cref="Maybe{T}" />.
        /// </summary>
        public bool HasValue { get; }

        private readonly Result<T, Unit> _result;

        /// <summary>
        ///     Treat <typeparamref name="T" /> as enumerable with 0-1 elements in.
        ///     This is handy when combining <see cref="Result{T,TError}" /> with LINQ's API.
        /// </summary>
        public IEnumerable<T> AsEnumerable => _result.AsEnumerable;

        internal T Value { get; }

        private Maybe(Result<T, Unit> result) {
            HasValue = result.HasValue;
            Value = result.Value;
            _result = result;
        }

        /// <inheritdoc />
        public bool Equals(Maybe<T> other) => other._result.Equals(_result);

        public static implicit operator Maybe<T>(T item) => new Maybe<T>(ResultExtensions.Ok<T, Unit>(item));

        public override bool Equals(object obj) => obj is Maybe<T> maybe && Equals(maybe);

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        /// <inheritdoc />
        public override int GetHashCode() => _result.GetHashCode();

        /// <inheritdoc />
        public int CompareTo(Maybe<T> other) => other._result.CompareTo(other._result);

        public static bool operator <(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) < 0;

        public static bool operator <=(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) <= 0;

        public static bool operator >(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) > 0;

        public static bool operator >=(Maybe<T> left, Maybe<T> right) => left.CompareTo(right) >= 0;

        /// <inheritdoc />
        public override string ToString() =>
            $"{(HasValue ? "Some" : "None")} ==> {typeof(Maybe<T>).ToHumanString()}{StringFunctions.PrettyTypeString(Value)}";

        /// <summary>
        ///     Evaluates the <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="someAction">
        ///     Is executed when the <see cref="Maybe{T}" /> has a value.
        /// </param>
        /// <param name="noneAction">
        ///     Is executed when he <see cref="Maybe{T}" /> has no value.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     When either <paramref name="someAction" /> or <paramref name="noneAction" /> needs to be executed.
        /// </exception>
        public void Match(Action<T> someAction, Action noneAction) => _result.Match(someAction, x => {
            if (noneAction == null)
                throw new ArgumentNullException(nameof(noneAction));
            noneAction();
        });

        /// <summary>
        ///     Evaluates the <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="someSelector">
        ///     Is executed when the <see cref="Maybe{T}" /> has a value.
        /// </param>
        /// <param name="noneSelector">
        ///     Is executed when he <see cref="Maybe{T}" /> has no value.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type returned by the functions <paramref name="someSelector" /> and <paramref name="noneSelector" />.
        /// </typeparam>
        [Pure]
        public TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector) =>
            _result.Match(someSelector, _ => {
                if (noneSelector == null)
                    throw new ArgumentNullException(nameof(noneSelector));

                return noneSelector();
            });

        /// <summary>
        ///     Maps <typeparamref name="T" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <see cref="Maybe{T}" /> has a value.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type returned from the function <paramref name="selector" />.
        /// </typeparam>
        [Pure]
        public Maybe<TResult>
            Map<TResult>(Func<T, TResult> selector) => new Maybe<TResult>(_result.Map(selector));

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        [Pure]
        public Maybe<T> Filter(Func<T, bool> predicate) => new Maybe<T>(_result.Filter(predicate, Unit.Selector));

        /// <summary>
        ///     Flamaps another <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Maybe{T}" /> as its return type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type <typeparamref name="T" /> returned from the <paramref name="flatMapSelector" /> function.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TResult>(Func<T, Maybe<TResult>> flatMapSelector) =>
            new Maybe<TResult>(_result.FlatMap(x => {
                if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
                return flatMapSelector(x)._result;
            }, Unit.AlternativeSelector));

        /// <summary>
        ///     Flamaps another <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Maybe{T}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TFlatMap" /> which can return
        ///     any type.
        /// </param>
        /// <typeparam name="TFlatMap">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, Maybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) =>
            new Maybe<TResult>(_result.FlatMap(x => {
                if (flatMapSelector == null)
                    throw new ArgumentNullException(nameof(flatMapSelector));
                return flatMapSelector(x)._result;
            }, resultSelector));

        /// <summary>
        ///     Flamaps a <see cref="Nullable{T}" />.
        /// </summary>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as its return type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type <typeparamref name="T" /> returned from the <paramref name="flatSelector" /> function.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TResult>(
            Func<T, TResult?> flatSelector) where TResult : struct =>
            HasValue ? flatSelector(Value).ToMaybe() : Maybe<TResult>.None;

        /// <summary>
        ///     Filters the <see cref="Maybe{T}" /> to see if <typeparamref name="T" /> is null.
        /// </summary>
        /// <returns>
        ///     A <see cref="Maybe{T}" /> whose <typeparamref name="T" /> has value if <typeparamref name="T" /> is not null.
        /// </returns>
        [Pure]
        public Maybe<T> IsNoneWhenNull() => new Maybe<T>(_result.IsErrorWhenNull(() => Unit.Default));

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        [Pure]
        public Maybe<T> IsNoneWhen(Func<T, bool> predicate) =>
            new Maybe<T>(_result.IsErrorWhen(predicate, Unit.Selector));

        [Pure]
        public Maybe<T> Flatten<TResult>(Func<T, Maybe<TResult>> selector) => new Maybe<T>(
            _result.Flatten(
                x => selector?.Invoke(x)._result ?? throw new ArgumentNullException(nameof(selector)),
                Unit.AlternativeSelector
            )
        );

        /// <summary>
        ///     Flamaps another <see cref="Maybe{T}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TFlatMap" /> which can return
        ///     any type.
        /// </param>
        /// <typeparam name="TFlatMap">
        ///     The value type of the <see cref="Nullable{T}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, TFlatMap?> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) where TFlatMap : struct => FlatMap(
            src => flatMapSelector(src).ToMaybe().Map(elem => resultSelector(src, elem)));
    }
}