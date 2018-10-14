using System;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling.Internal {
    /// <summary>
    ///     A data-structure commonly used for error-handling where value may or may not be present.
    /// </summary>
    /// <typeparam name="T">
    ///     The potential value.
    /// </typeparam>
    internal readonly struct Maybe<T> : IMaybe<T> {
        internal static IMaybe<T> None { get; } = new Maybe<T>(default, false);
        internal static IMaybe<T> Create(in T value) => new Maybe<T>(in value, true);

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

        private Maybe(in T value, bool hasValue) {
            Value = value;
            HasValue = hasValue;
            if (HasValue && Value.IsNull()) throw new ArgumentNullException(nameof(Value));
        }

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
        public void Match(Action<T> someAction, Action noneAction) {
            if (someAction == null)
                throw new ArgumentNullException(nameof(someAction));
            if (noneAction == null)
                throw new ArgumentNullException(nameof(noneAction));
            if (HasValue) someAction(Value);
            else noneAction();
        }

        public IMaybe<T> DoWith(Action<T> someAction) {
            if (someAction == null) throw new ArgumentNullException(nameof(someAction));
            if (HasValue) someAction(Value);

            return this;
        }

        public IMaybe<T> Do(Action action) {
            if (action == null) throw new ArgumentNullException(nameof(action));
            action();
            return this;
        }

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
        public TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector) {
            if (someSelector == null)
                throw new ArgumentNullException(nameof(someSelector));
            if (noneSelector == null)
                throw new ArgumentNullException(nameof(noneSelector));
            return HasValue ? someSelector(Value) : noneSelector();
        }

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
        public IMaybe<TResult> Map<TResult>(Func<T, TResult> selector) {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return HasValue ? Maybe<TResult>.Create(selector(Value)) : Maybe<TResult>.None;
        }

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        [Pure]
        public IMaybe<T> Filter(Func<T, bool> predicate) {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (HasValue && predicate(Value))
                return this;
            return None;
        }

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
        public IMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> flatMapSelector) => flatMapSelector == null
            ? throw new ArgumentNullException(nameof(flatMapSelector))
            : HasValue
                ? flatMapSelector(Value)
                : Maybe<TResult>.None;

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
        public IMaybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) {
            if (flatMapSelector == null)
                throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (HasValue) {
                var mapSelector = flatMapSelector(Value);
                if (mapSelector.HasValue) {
                    return new Maybe<TResult>(resultSelector(Value, mapSelector.Value), true);
                }
            }

            return Maybe<TResult>.None;
        }

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
        public IMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> flatSelector) where TResult : struct {
            if (flatSelector == null)
                throw new ArgumentNullException(nameof(flatSelector));
            if (!HasValue) return Maybe<TResult>.None;
            var selector = flatSelector(Value);
            return selector.HasValue ? new Maybe<TResult>(selector.Value, true) : Maybe<TResult>.None;
        }

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <see cref="Maybe{T}" /> has a value.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        [Pure]
        public IMaybe<T> IsNoneWhen(Func<T, bool> predicate) {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (HasValue && predicate(Value))
                return None;
            return this;
        }

        [Pure]
        public IMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector) {
            if (selector == null)
                throw new ArgumentNullException(nameof(selector));
            return HasValue
                ? selector(Value).HasValue
                    ? this
                    : None
                : None;
        }

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
        public IMaybe<TResult> FlatMap<TFlatMap, TResult>(Func<T, TFlatMap?> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) where TFlatMap : struct {
            if (flatMapSelector == null)
                throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (!HasValue) return Maybe<TResult>.None;

            var mapSelector = flatMapSelector(Value);
            return mapSelector.HasValue
                ? new Maybe<TResult>(resultSelector(Value, mapSelector.Value), true)
                : Maybe<TResult>.None;
        }
    }
}