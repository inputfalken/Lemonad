﻿using System;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     A data-structure commonly used for error-handling where value may or may not be present.
    /// </summary>
    /// <typeparam name="T">
    ///     The potential value.
    /// </typeparam>
    public readonly struct Maybe<T> : IEquatable<Maybe<T>>, IComparable<Maybe<T>> {
        public static Maybe<T> None { get; } = new Maybe<T>(ResultExtensions.Error<T, Unit>(default));
        private readonly Result<T, Unit> _result;

        /// <summary>
        ///     Gets a value indicating whether the current <see cref="Maybe{T}" /> object has a valid value of
        ///     its underlying type.
        /// </summary>
        /// <returns>
        ///     true if the current <see cref="Maybe{T}"></see> object has a value; false if the current
        ///     <see cref="Maybe{T}"></see> object has no value.
        /// </returns>
        public bool HasValue { get; }

        /// <summary>
        ///     Gets the value of the current <see cref="Maybe{T}"></see> object if <see cref="HasValue" /> is true.
        /// </summary>
        /// <example>
        ///     <code language="c#">
        ///  if (Either.HasValue)
        ///  {
        ///      // Safe to use.
        ///      Console.WriteLine(Either.Value)
        ///  }
        ///  </code>
        /// </example>
        public T Value { get; }

        private Maybe(Result<T, Unit> result) {
            HasValue = result.Either.HasValue;
            Value = result.Either.Value;
            _result = result;
        }

        /// <inheritdoc />
        public bool Equals(Maybe<T> other) => other._result.Equals(_result);

        public static implicit operator Maybe<T>(T item) => new Maybe<T>(ResultExtensions.Value<T, Unit>(item));

        public override bool Equals(object obj) => obj is Maybe<T> maybe && Equals(maybe);

        public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

        public static bool operator !=(Maybe<T> left, Maybe<T> right) => !left.Equals(right);

        /// <inheritdoc />
        public override int GetHashCode() => _result.GetHashCode();

        /// <inheritdoc />
        public int CompareTo(Maybe<T> other) => _result.CompareTo(other._result);

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

        public Maybe<T> DoWith(Action<T> someAction) => new Maybe<T>(_result.DoWith(someAction));

        public Maybe<T> Do(Action action) => new Maybe<T>(_result.Do(action));

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
            _result.Match(someSelector, _ => noneSelector == null
                ? throw new ArgumentNullException(nameof(noneSelector))
                : noneSelector()
            );

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
        public Maybe<T> Filter(Func<T, bool> predicate) => new Maybe<T>(_result.Filter(predicate, x => Unit.Default));

        /// <summary>
        ///     Flatmaps another <see cref="Maybe{T}" />.
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
        ///     Flatmaps another <see cref="Maybe{T}" />.
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
        ///     Flatmaps a <see cref="Nullable{T}" />.
        /// </summary>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Nullable{T}" /> as its return type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type <typeparamref name="T" /> returned from the <paramref name="flatSelector" /> function.
        /// </typeparam>
        [Pure]
        public Maybe<TResult> FlatMap<TResult>(Func<T, TResult?> flatSelector) where TResult : struct =>
            new Maybe<TResult>(_result.FlatMap(x => flatSelector(x).ToResult(Unit.Selector)));

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        [Pure]
        public Maybe<T> IsNoneWhen(Func<T, bool> predicate) =>
            new Maybe<T>(_result.IsErrorWhen(predicate, maybe => default));

        [Pure]
        public Maybe<T> Flatten<TResult>(Func<T, Maybe<TResult>> selector) => new Maybe<T>(
            _result.Flatten(
                x => selector?.Invoke(x)._result ?? throw new ArgumentNullException(nameof(selector)),
                Unit.AlternativeSelector
            )
        );

        /// <summary>
        ///     Flatmaps another <see cref="Maybe{T}" />.
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
            Func<T, TFlatMap, TResult> resultSelector) where TFlatMap : struct =>
            new Maybe<TResult>(_result.FlatMap(x => flatMapSelector(x).ToResult(Unit.Selector), resultSelector));
    }
}