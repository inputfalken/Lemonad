using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///  A data-structure commonly used for error-handling where only one value can be present.
    ///  Either it's <typeparamref name="TError" /> or it's <typeparamref name="T" />. Which makes it possible to handle error without throwing exceptions.
    ///  Inspired by 'Haskells Either a b' and FSharps 'Result&lt;T, TError&gt;'.
    /// </summary>
    /// <typeparam name="T">
    /// The type which is considered as successfull.
    /// </typeparam>
    /// <typeparam name="TError">
    /// The type which is considered as failure.
    /// </typeparam>
    public readonly struct Result<T, TError> : IEquatable<Result<T, TError>>, IComparable<Result<T, TError>> {
        internal TError Error { get; }
        internal T Value { get; }

        internal Result(in T value, in TError error, in bool hasError, in bool hasValue) {
            HasValue = hasValue;
            HasError = hasError;
            Value = value;
            Error = error;
        }

        /// <summary>
        /// Is true if there's a <typeparamref name="T"/> in the current state of the <see cref="Result{T,TError}"/>.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Is true if there's a <typeparamref name="TError"/> in the current state of the <see cref="Result{T,TError}"/>.
        /// </summary>
        public bool HasError { get; }

        /// <inheritdoc />
        public bool Equals(Result<T, TError> other) {
            if (!HasValue && !other.HasValue) {
                return EqualityComparer<TError>.Default.Equals(Error, other.Error);
            }

            if (HasValue && other.HasValue) {
                return EqualityComparer<T>.Default.Equals(Value, other.Value);
            }

            return false;
        }

        /// <inheritdoc />
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

        /// <summary>
        /// Treat <typeparamref name="TError"/> as enumerable with 0-1 elements in.
        /// This is handy when combining <see cref="Result{T,TError}"/> with LINQs API.
        /// </summary>
        public IEnumerable<TError> AsErrorEnumerable => YieldErrors(this);

        /// <summary>
        /// Treat <typeparamref name="T"/> as enumerable with 0-1 elements in.
        /// This is handy when combining <see cref="Result{T,TError}"/> with LINQ's API.
        /// </summary>
        public IEnumerable<T> AsEnumerable => YieldValues(this);

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Result<T, TError> option && Equals(option);

        /// <inheritdoc />
        public override int GetHashCode() =>
            HasValue ? (Value == null ? 1 : Value.GetHashCode()) : (Error == null ? 0 : Error.GetHashCode());

        /// <inheritdoc />
        public int CompareTo(Result<T, TError> other) {
            if (HasValue && !other.HasValue) return 1;
            if (!HasValue && other.HasValue) return -1;

            return HasValue
                ? Comparer<T>.Default.Compare(Value, other.Value)
                : Comparer<TError>.Default.Compare(Error, other.Error);
        }

        /// <summary>
        /// Evaluates the <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <param name="selector">
        ///     Is exectued when the <see cref="Result{T,TError}"/> contains <see cref="T"/>.
        /// </param>
        /// <param name="errorSelector">
        /// Is exectued when the <see cref="Result{T,TError}"/> contains <see cref="TError"/>.
        /// </param>
        /// <typeparam name="TResult">
        /// The return type of <paramref name="selector" /> and <paramref name="errorSelector" />
        /// </typeparam>
        /// <returns>
        ///     Either <typeparamref name="T" /> or <typeparamref name="TError" />.
        /// </returns>
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

        /// <summary>
        /// Evaluates the <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <param name="action">
        /// Is exectued when the <see cref="Result{T,TError}"/> contains <see cref="T"/>.
        /// </param>
        /// <param name="errorAction">
        /// Is exectued when the <see cref="Result{T,TError}"/> contains <see cref="TError"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// When either <paramref name="action"/> or <paramref name="errorAction"/> and needs to be executed.
        /// </exception>
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

        /// <summary>
        /// Executes the <paramref name="action" /> if <typeparamref name="T" /> is the active type.
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        /// <see cref="Result{T,TError}"/> with side effects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="action"/> is null and needs to be exectued.
        /// </exception>
        public Result<T, TError> DoWith(Action<T> action) {
            if (HasValue)
                if (action != null)
                    action.Invoke(Value);
                else
                    throw new ArgumentNullException(nameof(action));

            return this;
        }

        /// <summary>
        /// Exectues  <paramref name="action"/>.
        /// </summary>
        /// <param name="action">
        ///  Is executed no matter what state <see cref="Result{T,TError}"/> is in.
        /// </param>
        /// <returns>
        /// <see cref="Result{T,TError}"/> with side effects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="action"/> is null.
        /// </exception>
        public Result<T, TError> Do(Action action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action();
            return this;
        }

        /// <summary>
        /// Executes the <paramref name="action" /> if <typeparamref name="TError" /> is the active type.
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        /// <see cref="Result{T,TError}"/> with side effects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When <paramref name="action"/> is null and needs to be exectued.
        /// </exception>
        public Result<T, TError> DoWithError(
            Action<TError> action) {
            if (HasError)
                if (action != null)
                    action.Invoke(Error);
                else
                    throw new ArgumentNullException(nameof(action));

            return this;
        }

        /// <summary>
        ///  Filters the <typeparamref name="T"/> if <typeparamref name="T"/> is the active type.
        /// </summary>
        /// <param name="predicate">
        /// A function to test <typeparamref name="T"/>.
        /// </param>
        /// <param name="errorSelector">
        /// Is executed when the <paramref name="predicate"/> function returns false.
        /// </param>
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

        /// <summary>
        ///  Filters the <typeparamref name="T"/> if <typeparamref name="T"/> is the active type.
        /// </summary>
        /// <param name="predicate">
        /// A function to test <typeparamref name="T"/>.
        /// </param>
        /// <param name="errorSelector">
        /// Is executed when the <paramref name="predicate"/> function is true.
        /// </param>
        /// <returns>
        /// A filtered <see cref="Result{T,TError}"/>.
        /// </returns>
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

        /// <summary>
        ///  Filters the <typeparamref name="T"/> by checking for null if <typeparamref name="T"/> is the active type.
        /// </summary>
        /// <param name="errorSelector">
        /// Is executed when <typeparamref name="T"/> is null.
        /// </param>
        /// <returns>
        /// A filtered <see cref="Result{T,TError}"/>.
        /// </returns>
        [Pure]
        public Result<T, TError> IsErrorWhenNull(Func<TError> errorSelector) =>
            IsErrorWhen(EquailtyFunctions.IsNull, errorSelector);

        /// <summary>
        /// Maps both <typeparamref name="T"/> and <typeparamref name="TError"/> but only one is executed.
        /// </summary>
        /// <param name="selector">
        /// Is executed if <typeparamref name="T"/> is the active type.
        /// </param>
        /// <param name="errorSelector">
        /// Is executed if <typeparamref name="TError"/> is the active type.
        /// </param>
        /// <typeparam name="TErrorResult">
        /// The result from the function <paramref name="errorSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The result from the function <paramref name="selector"/>.
        /// </typeparam>
        /// <returns>
        /// A mapped <see cref="Result{T,TError}"/>.
        /// </returns>
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

        /// <summary>
        /// Maps <typeparamref name="T"/>.
        /// </summary>
        /// <param name="selector">
        /// Is executed if <typeparamref name="T"/> is the active type.
        /// </param>
        /// <typeparam name="TResult">
        /// The result from the function <paramref name="selector"/>.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Result{T,TError}"/>  with its <typeparamref name="T"/> mapped.
        /// </returns>
        [Pure]
        public Result<TResult, TError> Map<TResult>(Func<T, TResult> selector) => HasValue
            ? selector != null
                ? Result.Ok<TResult, TError>(selector(Value))
                : throw new ArgumentNullException(nameof(selector))
            : Result.Error<TResult, TError>(Error);

        /// <summary>
        /// Maps <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="selector">
        /// Is executed if <typeparamref name="TError"/> is the active type.
        /// </param>
        /// <typeparam name="TErrorResult">
        /// The result from the function <paramref name="selector"/>.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Result{T,TError}"/>  with its <typeparamref name="TError"/> mapped.
        /// </returns>
        [Pure]
        public Result<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) => HasError
            ? selector != null
                ? Result.Error<T, TErrorResult>(selector(Error))
                : throw new ArgumentNullException(nameof(selector))
            : Result.Ok<T, TErrorResult>(Value);

        /// <summary>
        /// Flatten another <see cref="Result{T,TError}"/> who shares the same <typeparamref name="TError"/>.
        /// And maps <typeparamref name="T"/> to <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="flatSelector">
        /// A function who expects a <see cref="Result{T,TError}"/> as an return type.
        /// </param>
        /// <typeparam name="TResult">
        /// The return type of the function <paramref name="flatSelector"/>.
        /// </typeparam>
        [Pure]
        public Result<TResult, TError> FlatMap<TResult>(
            Func<T, Result<TResult, TError>> flatSelector) => HasValue
            ? flatSelector?.Invoke(Value) ?? throw new ArgumentNullException(nameof(flatSelector))
            : Result.Error<TResult, TError>(Error);

        /// <summary>
        /// Flatten another <see cref="Result{T,TError}"/> who shares the same <typeparamref name="TError"/>.
        /// And maps <typeparamref name="T"/> together with <typeparamref name="TSelector"/> to <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="flatSelector">
        /// A function who expects a <see cref="Result{T,TError}"/> as an return type.
        /// </param>
        /// <param name="resultSelector">
        /// A function whose in-parameters are <typeparamref name="T"/> and  <typeparamref name="TSelector"/>  which can return any type.
        /// </param>
        /// <typeparam name="TSelector">
        /// The value retrieved from the the <see cref="Result{T,TError}"/> given by the <paramref name="flatSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The return type of the function  <paramref name="resultSelector"/>.
        /// </typeparam>
        [Pure]
        public Result<TResult, TError> FlatMap<TSelector, TResult>(
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => FlatMap(x =>
            flatSelector?.Invoke(x).Map(y => resultSelector == null
                ? throw new ArgumentNullException(nameof(resultSelector))
                : resultSelector(x, y)) ??
            throw new ArgumentNullException(nameof(flatSelector)));

        /// <summary>
        /// Flatten another <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="Result{T,TError}"/> returned is not the the result from <paramref name="selector"/>.
        /// Use <see cref="FlatMap{TResult}"/> in order to the get the result from <paramref name="selector"/>.
        /// </remarks>
        /// <param name="selector">
        /// A function who expects a <see cref="Result{T,TError}"/> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///  Maps the error to from <typeparamref name="TErrorResult"/> to <typeparamref name="TError"/>.
        /// </param>
        /// <typeparam name="TResult">
        /// The value of the <see cref="Result{T,TError}"/> returned by the function <paramref name="selector"/>.
        /// </typeparam>
        /// <typeparam name="TErrorResult">
        /// The error of the <see cref="Result{T,TError}"/> returned by the function <paramref name="selector"/>.
        /// </typeparam>
        /// <returns>
        /// The same <see cref="Result{T,TError}"/> but it's state is dependant on the <see cref="Result{T,TError}"/>
        /// returned by the <paramref name="selector"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// When any of the function parameters are null and needs to be exectued.
        /// </exception>
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

        /// <summary>
        /// Flatten another <see cref="Result{T,TError}"/>  who shares the same <typeparamref name="TError"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="Result{T,TError}"/> returned is not the the result from <paramref name="selector"/>.
        /// Use <see cref="FlatMap{TResult}"/> in order to the get the result from <paramref name="selector"/>.
        /// </remarks>
        /// <param name="selector"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Pure]
        public Result<T, TError> Flatten<TResult>(Func<T, Result<TResult, TError>> selector) {
            if (HasValue) {
                if (selector == null) throw new ArgumentNullException(nameof(selector));
                var okSelector = selector(Value);
                if (okSelector.HasValue)
                    return Value;
                return okSelector.Error;
            }

            return Result.Error<T, TError>(Error);
        }

        /// <summary>
        /// Executes each function and saves all potential errors to a list which will be the <typeparamref name="TError"/>.
        /// </summary>
        /// <param name="validations">
        /// A <see cref="IReadOnlyList{T}"/> containining <typeparamref name="TError"/>.
        /// </param>
        public Result<T, IReadOnlyList<TError>> Multiple(
            params Func<Result<T, TError>, Result<T, TError>>[] validations) {
            var result = this;
            var errors = validations.Select(x => x(result)).ToList().Errors().ToList();
            if (errors.Any())
                return errors;

            return Value;
        }

        /// <summary>
        /// Flatmaps another <see cref="Result{T,TError}"/> but the <typeparamref name="TError"/> remains as the same type.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Result{T,TError}"/> as its return type.
        /// </param>
        /// <param name="errorSelector">
        /// A function which maps <typeparamref name="TErrorResult"/> to <typeparamref name="TError"/>.
        /// </param>
        /// <typeparam name="TErrorResult">
        /// The error type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The value type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// When any of the function parameters are null and needs to be exectued.
        /// </exception>
        [Pure]
        public Result<TResult, TError> FlatMap<TErrorResult, TResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            if (HasValue) {
                if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
                var okSelector = flatMapSelector(Value);

                return okSelector.HasValue
                    ? Result.Ok<TResult, TError>(okSelector.Value)
                    : okSelector.MapError(errorSelector);
            }

            return Result.Error<TResult, TError>(Error);
        }

        /// <summary>
        /// Flatmaps another <see cref="Result{T,TError}"/> but the <typeparamref name="TError"/> remains as the same type.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Result{T,TError}"/> as its return type.
        /// </param>
        /// <param name="resultSelector">
        /// A function whose in-parameters are <typeparamref name="T"/> and <typeparamref name="TFlatMap"/> which can return any type.
        /// </param>
        /// <param name="errorSelector">
        /// A function which maps <typeparamref name="TErrorResult"/>. to <typeparamref name="TError"/>.
        /// </param>
        /// <typeparam name="TErrorResult">
        /// The error type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/> function.
        /// </typeparam>
        /// <typeparam name="TFlatMap">
        /// The value type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type returned by the function <paramref name="resultSelector"/>.
        /// </typeparam>
        [Pure]
        public Result<TResult, TError> FlatMap<TErrorResult, TFlatMap, TResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            FlatMap(x => flatMapSelector?.Invoke(x).Map(y => {
                    if (resultSelector == null)
                        throw new ArgumentNullException(nameof(resultSelector));
                    return resultSelector(x, y);
                }) ?? throw new ArgumentNullException(nameof(flatMapSelector)),
                errorSelector);

        /// <summary>
        /// Fully flatmaps another <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Result{T,TError}"/> as its return type.
        /// </param>
        /// <param name="errorSelector">
        /// A function which maps <typeparamref name="TError"/> to <typeparamref name="TErrorResult"/>.
        /// </param>
        /// <typeparam name="TErrorResult">
        /// The error type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The value type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        /// When any of the function parameters are null and needs to be exectued.
        /// </exception>
        [Pure]
        public Result<TResult, TErrorResult> FullFlatMap<TErrorResult, TResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) {
            if (HasValue) {
                return flatMapSelector?.Invoke(Value) ?? throw new ArgumentNullException(nameof(flatMapSelector));
            }

            return errorSelector != null
                ? errorSelector(Error)
                : throw new ArgumentNullException(nameof(errorSelector));
        }

        /// <summary>
        /// Casts <typeparamref name="TError"/> into <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type to <typeparamref name="TError"/> cast to.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Result{T,TError}"/> whose <typeparamref name="TError"/> has been casted to <typeparamref name="TResult"/>.
        /// </returns>
        [Pure]
        public Result<T, TResult> CastError<TResult>() {
            if (HasValue) return Value;
            return (TResult) (object) Error;
        }

        /// <summary>
        /// Casts <typeparamref name="T"/> into <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">
        /// The type to cast to.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Result{T,TError}"/> whose <typeparamref name="T"/> has been casted to <typeparamref name="TResult"/>.
        /// </returns>
        [Pure]
        public Result<TResult, TError> Cast<TResult>() {
            if (HasError) return Error;
            return (TResult) (object) Value;
        }

        /// <summary>
        /// Attempts to cast <typeparamref name="T"/> into <typeparamref name="TResult"/>.
        /// </summary>
        /// <param name="errorSelector">
        /// Is executed if the cast would fail.
        /// </param>
        /// <typeparam name="TResult">
        /// The type to cast to.
        /// </typeparam>
        /// <returns>
        /// A <see cref="Result{T,TError}"/> whose <typeparamref name="T"/> has been casted to <typeparamref name="TResult"/>.
        /// </returns>
        [Pure]
        public Result<TResult, TError> SafeCast<TResult>(Func<TError> errorSelector) {
            if (HasError) return Error;
            if (Value is TResult result)
                return result;

            return errorSelector();
        }

        /// <summary>
        /// Fully flatmaps another <see cref="Result{T,TError}"/>.
        /// </summary>
        /// <param name="flatMapSelector">
        /// A function who expects a <see cref="Result{T,TError}"/> as its return type.
        /// </param>
        /// <param name="resultSelector">
        /// A function whose in-parameters are <typeparamref name="T"/> and <typeparamref name="TFlatMap"/> which can return any type.
        /// </param>
        /// <param name="errorSelector">
        /// A function which maps <typeparamref name="TError"/> to <typeparamref name="TErrorResult"/>.
        /// </param>
        /// <typeparam name="TErrorResult">
        /// The error type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TFlatMap">
        /// The value type of the <see cref="Result{T,TError}"/> returned by the <paramref name="flatMapSelector"/>.
        /// </typeparam>
        /// <typeparam name="TResult">
        /// The type returned by the function <paramref name="resultSelector"/>.
        /// </typeparam>
        /// <returns></returns>
        [Pure]
        public Result<TResult, TErrorResult> FullFlatMap<TErrorResult, TFlatMap, TResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            FullFlatMap(x => flatMapSelector?.Invoke(x).Map(y => {
                    if (resultSelector == null)
                        throw new ArgumentNullException(nameof(resultSelector));
                    return resultSelector(x, y);
                }) ?? throw new ArgumentNullException(nameof(flatMapSelector)),
                errorSelector);
    }
}