using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public struct Result<T, TError> : IEquatable<Result<T, TError>>, IComparable<Result<T, TError>> {
        internal TError Error { get; }
        internal T Value { get; }

        internal Result(T value, TError error, bool hasError, bool hasValue) {
            HasValue = hasValue;
            HasError = hasError;
            Value = value;
            Error = error;
        }

        public bool HasValue { get; }
        public bool HasError { get; }

        public bool Equals(Result<T, TError> other) {
            if (!HasValue && !other.HasValue) {
                return EqualityComparer<TError>.Default.Equals(Error, other.Error);
            }

            if (HasValue && other.HasValue) {
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            return false;
        }

        public override string ToString() =>
            $"{(HasValue ? "Ok" : "Error")} ==> {typeof(Result<T, TError>).ToHumanString()}{StringFunctions.PrettyTypeString(HasValue ? (object) Value : Error)}";

        public static bool operator ==(Result<T, TError> left, Result<T, TError> right) => left.Equals(right);
        public static bool operator !=(Result<T, TError> left, Result<T, TError> right) => !left.Equals(right);

        public static bool operator <(Result<T, TError> left, Result<T, TError> right) =>
            left.CompareTo(right) < 0;

        public static bool operator <=(Result<T, TError> left, Result<T, TError> right) =>
            left.CompareTo(right) <= 0;

        public static bool operator >(Result<T, TError> left, Result<T, TError> right) =>
            left.CompareTo(right) > 0;

        public static bool operator >=(Result<T, TError> left, Result<T, TError> right) =>
            left.CompareTo(right) >= 0;

        public static implicit operator Result<T, TError>(TError error) => Result.Error<T, TError>(error);

        public static implicit operator Result<T, TError>(T value) => Result.Ok<T, TError>(value);

        private static IEnumerable<TError> YieldErrors(Result<T, TError> result) {
            if (result.HasError)
                yield return result.Error;
        }

        private static IEnumerable<T> YieldValues(Result<T, TError> result) {
            if (result.HasValue)
                yield return result.Value;
        }

        public IEnumerable<TError> Errors => YieldErrors(this);
        public IEnumerable<T> Values => YieldValues(this);

        public override bool Equals(object obj) => obj is Result<T, TError> option && Equals(option);

        public override int GetHashCode() =>
            HasValue ? (Value == null ? 1 : Value.GetHashCode()) : (Error == null ? 0 : Error.GetHashCode());

        public int CompareTo(Result<T, TError> other) {
            if (HasValue && !other.HasValue) return 1;
            if (!HasValue && other.HasValue) return -1;

            return HasValue
                ? Comparer<T>.Default.Compare(Value, other.Value)
                : Comparer<TError>.Default.Compare(Error, other.Error);
        }

        [Pure]
        public TResult Match<TResult>(
            Func<T, TResult> selector, Func<TError, TResult> errorSelector) {
            if (HasError)
                return errorSelector != null
                    ? errorSelector(Error)
                    : throw new ArgumentNullException(nameof(errorSelector));

            return selector != null
                ? selector(Value)
                : throw new ArgumentNullException(nameof(selector));
        }

        public void Match(Action<T> action, Action<TError> errorAction) {
            if (HasError)
                if (errorAction != null)
                    errorAction(Error);
                else
                    throw new ArgumentNullException(nameof(errorAction));
            else if (action != null)
                action(Value);
            else
                throw new ArgumentNullException(nameof(action));
        }

        public Result<T, TError> DoWhenOk(Action<T> action) {
            if (HasValue)
                if (action != null)
                    action.Invoke(Value);
                else
                    throw new ArgumentNullException(nameof(action));

            return this;
        }

        public Result<T, TError> Do(Action action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action();
            return this;
        }

        public Result<T, TError> DoWhenError(
            Action<TError> errorAction) {
            if (HasError)
                if (errorAction != null)
                    errorAction.Invoke(Error);
                else
                    throw new ArgumentNullException(nameof(errorAction));

            return this;
        }

        [Pure]
        public Result<T, TError> Filter(
            Func<T, bool> predicate, Func<TError> errorSelector) {
            return HasValue
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(Value)
                        ? Result.Ok<T, TError>(Value)
                        : errorSelector == null
                            ? throw new ArgumentNullException(nameof(errorSelector))
                            : Result.Error<T, TError>(errorSelector())
                : errorSelector == null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : Result.Error<T, TError>(Error);
        }

        [Pure]
        public Result<T, TError> IsErrorWhen(
            Func<T, bool> predicate, Func<TError> errorSelector) =>
            HasValue
                ? predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(Value)
                        ? errorSelector == null
                            ? throw new ArgumentNullException(nameof(errorSelector))
                            : Result.Error<T, TError>(errorSelector())
                        : Result.Ok<T, TError>(Value)
                : errorSelector == null
                    ? throw new ArgumentNullException(nameof(errorSelector))
                    : Result.Error<T, TError>(Error);

        [Pure]
        public Result<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            IsErrorWhen(EquailtyFunctions.IsNull, errorSelector);

        [Pure]
        public Result<TResult, TErrorResult> FullMap<TErrorResult, TResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => HasError
            ? Result.Error<TResult, TErrorResult>(errorSelector != null
                ? errorSelector(Error)
                : throw new ArgumentNullException(nameof(errorSelector)))
            : Result.Ok<TResult, TErrorResult>(selector != null
                ? selector(Value)
                : throw new ArgumentNullException(nameof(selector)));

        [Pure]
        public Result<TResult, TError> Map<TResult>(Func<T, TResult> selector) => HasValue
            ? selector != null
                ? Result.Ok<TResult, TError>(selector(Value))
                : throw new ArgumentNullException(nameof(selector))
            : Result.Error<TResult, TError>(Error);

        [Pure]
        public Result<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> errorSelector) => HasError
            ? errorSelector != null
                ? Result.Error<T, TErrorResult>(errorSelector(Error))
                : throw new ArgumentNullException(nameof(errorSelector))
            : Result.Ok<T, TErrorResult>(Value);

        [Pure]
        public Result<TResult, TError> FlatMap<TResult>(
            Func<T, Result<TResult, TError>> selector) => HasValue
            ? selector?.Invoke(Value) ?? throw new ArgumentNullException(nameof(selector))
            : Result.Error<TResult, TError>(Error);

        [Pure]
        public Result<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Result<TSelector, TError>> selector,
            Func<T, TSelector, TResult> resultSelector) => FlatMap(x =>
            selector?.Invoke(x).Map(y => resultSelector == null
                ? throw new ArgumentNullException(nameof(resultSelector))
                : resultSelector(x, y)) ??
            throw new ArgumentNullException(nameof(selector)));

        [Pure]
        public Result<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) {
            if (HasValue) {
                if (selector == null) throw new ArgumentNullException(nameof(selector));
                var okSelector = selector(Value);
                if (okSelector.HasValue)
                    return Result.Ok<T, TError>(Value);
                var tmpThis = this;
                return okSelector.FullMap(x => tmpThis.Value, errorSelector);
            }

            return Result.Error<T, TError>(Error);
        }

        [Pure]
        public Result<TResult, TError> FlatMap<TErrorResult, TResult>(
            Func<T, Result<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) {
            if (HasValue) {
                if (selector == null) throw new ArgumentNullException(nameof(selector));
                var okSelector = selector(Value);
                return okSelector.HasValue
                    ? Result.Ok<TResult, TError>(okSelector.Value)
                    : okSelector.MapError(errorSelector);
            }

            return Result.Error<TResult, TError>(Error);
        }

        public Result<T, IReadOnlyList<TError>> Multiple(params Func<Result<T, TError>, Result<T, TError>>[] validations) {
            var result = this;
            var errors = validations.Select(x => x(result)).ToList().Errors().ToList();
            if (errors.Any())
                return errors;

            return Value;
        }

        [Pure]
        public Result<TResult, TError> FlatMap<TErrorResult, TSelect, TResult>(
            Func<T, Result<TSelect, TErrorResult>> selector, Func<T, TSelect, TResult> resultSelecotr,
            Func<TErrorResult, TError> errorSelector) =>
            FlatMap(x => selector(x).Map(y => resultSelecotr(x, y)), errorSelector);
    }
}