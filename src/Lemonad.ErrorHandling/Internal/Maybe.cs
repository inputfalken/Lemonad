using System;
using Lemonad.ErrorHandling.Exceptions;

namespace Lemonad.ErrorHandling.Internal {
    internal readonly struct Maybe<T> : IMaybe<T> {
        internal static IMaybe<T> None { get; } = new Maybe<T>(default, false);
        internal static IMaybe<T> Create(in T value) => new Maybe<T>(in value, true);

        public bool HasValue { get; }

        public T Value { get; }

        private Maybe(in T value, bool hasValue) {
            Value = value;
            HasValue = hasValue;
            if (HasValue && Value.IsNull())
                throw new InvalidMaybeStateException(
                    $"{nameof(IMaybe<T>)} property \"{nameof(Value)}\" cannot be null when property \"{nameof(HasValue)}\" is expected to be true."
                );
        }

        public override string ToString() =>
            $"{(HasValue ? "Some" : "None")} ==> {typeof(Maybe<T>).ToHumanString()}{StringFunctions.PrettyTypeString(Value)}";

        public void Match(Action<T> someAction, Action noneAction) {
            if (someAction is null)
                throw new ArgumentNullException(nameof(someAction));
            if (noneAction is null)
                throw new ArgumentNullException(nameof(noneAction));
            if (HasValue) someAction(Value);
            else noneAction();
        }

        public IMaybe<T> DoWith(Action<T> someAction) {
            if (someAction is null) throw new ArgumentNullException(nameof(someAction));
            if (HasValue) someAction(Value);

            return this;
        }

        public IMaybe<T> Do(Action action) {
            if (action is null) throw new ArgumentNullException(nameof(action));
            action();
            return this;
        }

        public TResult Match<TResult>(Func<T, TResult> someSelector, Func<TResult> noneSelector) {
            if (someSelector is null)
                throw new ArgumentNullException(nameof(someSelector));
            if (noneSelector is null)
                throw new ArgumentNullException(nameof(noneSelector));
            return HasValue ? someSelector(Value) : noneSelector();
        }

        public IMaybe<TResult> Map<TResult>(Func<T, TResult> selector) {
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));
            return HasValue ? Maybe<TResult>.Create(selector(Value)) : Maybe<TResult>.None;
        }

        public IMaybe<T> Filter(Func<T, bool> predicate) {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));
            if (HasValue && predicate(Value))
                return this;
            return None;
        }

        public IMaybe<TResult> FlatMap<TResult>(Func<T, IMaybe<TResult>> flatMapSelector) => flatMapSelector is null
            ? throw new ArgumentNullException(nameof(flatMapSelector))
            : HasValue
                ? flatMapSelector(Value)
                : Maybe<TResult>.None;

        public IMaybe<TResult> FlatMap<TFlatMap, TResult>(
            Func<T, IMaybe<TFlatMap>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) {
            if (flatMapSelector is null)
                throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (!HasValue) return Maybe<TResult>.None;
            var mapSelector = flatMapSelector(Value);
            return mapSelector.HasValue
                ? Maybe<TResult>.Create(resultSelector(Value, mapSelector.Value))
                : Maybe<TResult>.None;
        }

        public IMaybe<TResult> FlatMap<TResult>(Func<T, TResult?> flatSelector) where TResult : struct {
            if (flatSelector is null)
                throw new ArgumentNullException(nameof(flatSelector));
            if (!HasValue) return Maybe<TResult>.None;
            var selector = flatSelector(Value);
            return selector.HasValue ? Maybe<TResult>.Create(selector.Value) : Maybe<TResult>.None;
        }

        public IMaybe<T> IsNoneWhen(Func<T, bool> predicate) {
            if (predicate is null)
                throw new ArgumentNullException(nameof(predicate));
            if (HasValue && predicate(Value))
                return None;
            return this;
        }

        public IMaybe<T> Flatten<TResult>(Func<T, IMaybe<TResult>> selector) {
            if (selector is null)
                throw new ArgumentNullException(nameof(selector));
            return HasValue
                ? selector(Value).HasValue
                    ? this
                    : None
                : None;
        }

        public IMaybe<TResult> FlatMap<TFlatMap, TResult>(Func<T, TFlatMap?> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector) where TFlatMap : struct {
            if (flatMapSelector is null)
                throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector is null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (!HasValue) return Maybe<TResult>.None;

            var mapSelector = flatMapSelector(Value);
            return mapSelector.HasValue
                ? Maybe<TResult>.Create(resultSelector(Value, mapSelector.Value))
                : Maybe<TResult>.None;
        }
    }
}