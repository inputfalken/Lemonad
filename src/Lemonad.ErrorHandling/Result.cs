using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    /// <summary>
    ///     A data-structure commonly used for error-handling where only one value can be present.
    ///     Either it's <typeparamref name="TError" /> or it's <typeparamref name="T" />. Which makes it possible to handle
    ///     error without throwing exceptions.
    ///     Inspired by 'Haskell's Either a b' and FSharps 'Result&lt;T, TError&gt;'.
    ///     <para></para>
    ///     <para></para>
    ///     Null values are not allowed and therefore excessively be checked before beginning an <see cref="Result{T,TError}"/> expression chain.
    /// </summary>
    /// <typeparam name="T">
    ///     The type which is considered as successful.
    /// </typeparam>
    /// <typeparam name="TError">
    ///     The type which is considered as failure.
    /// </typeparam>
    public readonly struct Result<T, TError> : IEquatable<Result<T, TError>>, IComparable<Result<T, TError>> {
        public static implicit operator Result<T, TError>(TError error) {
            var tmp = default(T);
            return new Result<T, TError>(in tmp, in error, true, false);
        }

        public static implicit operator Result<T, TError>(T value) {
            var tmp = default(TError);
            return new Result<T, TError>(in value, in tmp, false, true);
        }

        /// <summary>
        ///    Gets the <see cref="IEither{T,TError}"/> from the <see cref="Result{T,TError}"/> instance.
        /// </summary>
        public IEither<T, TError> Either { get; }

        private Result(in T value, in TError error, bool hasError, bool hasValue) =>
            Either = new NonNullEither(value, error, hasError, hasValue);

        /// <inheritdoc />
        public bool Equals(Result<T, TError> other) {
            if (!Either.HasValue && !other.Either.HasValue)
                return EqualityComparer<TError>.Default.Equals(Either.Error, other.Either.Error);

            if (Either.HasValue && other.Either.HasValue)
                return EqualityComparer<T>.Default.Equals(Either.Value, other.Either.Value);

            return false;
        }

        /// <inheritdoc />
        public override string ToString() =>
            $"{(Either.HasValue ? "Ok" : "Error")} ==> {typeof(Result<T, TError>).ToHumanString()}{StringFunctions.PrettyTypeString(Either.HasValue ? (object) Either.Value : Either.Error)}";

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

        /// <inheritdoc />
        public override bool Equals(object obj) => obj is Result<T, TError> option && Equals(option);

        /// <inheritdoc />
        public override int GetHashCode() =>
            Either.HasValue
                ? (Either.Value == null ? 1 : Either.Value.GetHashCode())
                : (Either.Error == null ? 0 : Either.Error.GetHashCode());

        /// <inheritdoc />
        public int CompareTo(Result<T, TError> other) {
            if (Either.HasValue && !other.Either.HasValue) return 1;
            if (!Either.HasValue && other.Either.HasValue) return -1;

            return Either.HasValue
                ? Comparer<T>.Default.Compare(Either.Value, other.Either.Value)
                : Comparer<TError>.Default.Compare(Either.Error, other.Either.Error);
        }

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="T" />.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="TError" />.
        /// </param>
        /// <typeparam name="TResult">
        ///     The return type of <paramref name="selector" /> and <paramref name="errorSelector" />
        /// </typeparam>
        /// <returns>
        ///     Either <typeparamref name="T" /> or <typeparamref name="TError" /> as <typeparamref name="TResult"/>.
        /// </returns>
        [Pure]
        public TResult Match<TResult>(
            Func<T, TResult> selector, Func<TError, TResult> errorSelector) {
            if (Either.HasError)
                return errorSelector != null
                    ? errorSelector(Either.Error)
                    : throw new ArgumentNullException(nameof(errorSelector));

            return selector != null
                ? selector(Either.Value)
                : throw new ArgumentNullException(nameof(selector));
        }

        /// <summary>
        ///     Evaluates the <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="action">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="T" />.
        /// </param>
        /// <param name="errorAction">
        ///     Is executed when the <see cref="Result{T,TError}" /> contains <see cref="TError" />.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     When either <paramref name="action" /> or <paramref name="errorAction" /> and needs to be executed.
        /// </exception>
        public void Match(Action<T> action, Action<TError> errorAction) {
            if (Either.HasError)
                if (errorAction != null)
                    errorAction(Either.Error);
                else
                    throw new ArgumentNullException(nameof(errorAction));
            else if (action != null)
                action(Either.Value);
            else
                throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        ///     Executes the <paramref name="action" /> if <typeparamref name="T" /> is the active type.
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        ///     <see cref="Result{T,TError}" /> with side effects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     When <paramref name="action" /> is null and needs to be executed.
        /// </exception>
        /// <returns>
        /// A <see cref="Result{T,TError}"/> whom may have invoked the <paramref name="action"/> if the current <see cref="Result{T,TError}"/> is in a valid state.
        /// </returns>
        public Result<T, TError> DoWith(Action<T> action) {
            if (Either.HasError) return this;
            if (action != null)
                action.Invoke(Either.Value);
            else
                throw new ArgumentNullException(nameof(action));

            return this;
        }

        /// <summary>
        ///     Executes  <paramref name="action" />.
        /// </summary>
        /// <param name="action">
        ///     Is executed no matter what state <see cref="Result{T,TError}" /> is in.
        /// </param>
        /// <returns>
        ///     <see cref="Result{T,TError}" /> with side effects.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     When <paramref name="action" /> is null.
        /// </exception>
        /// <returns>
        /// A <see cref="Result{T,TError}"/> who have invoked <paramref name="action"/> no matter what state the current <see cref="Result{T,TError}"/> is in.
        /// </returns>
        public Result<T, TError> Do(Action action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action();
            return this;
        }

        /// <summary>
        ///     Executes the <paramref name="action" /> if <typeparamref name="TError" /> is the active type.
        /// </summary>
        /// <param name="action">
        /// </param>
        /// <returns>
        ///     <see cref="Result{T,TError}" /> with side effects.
        /// </returns>
        /// <returns>
        /// A <see cref="Result{T,TError}"/> whom may have invoked the <paramref name="action"/> if the current <see cref="Result{T,TError}"/> is in a an invalid state.
        /// </returns>
        public Result<T, TError> DoWithError(
            Action<TError> action) {
            if (Either.HasValue) return this;
            if (action != null)
                action.Invoke(Either.Error);
            else
                throw new ArgumentNullException(nameof(action));

            return this;
        }

        /// <summary>
        ///     Filters the <typeparamref name="T" /> if <typeparamref name="T" /> is the active type.
        /// </summary>
        /// <param name="predicate">
        ///     A function to test <typeparamref name="T" />.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed when the <paramref name="predicate" /> is false. The in parameter is a <see cref="Maybe{T}" /> since
        ///     the <typeparamref name="T" /> could be unsafe in this context.
        /// </param>
        ///<returns>
        ///   A <see cref="Result{T,TError}"/> whose <typeparamref name="T"/> has been tested by the <paramref name="predicate"/>
        ///   if the current <see cref="Result{T,TError}"/> is in valid state.
        /// </returns>
        [Pure]
        public Result<T, TError> Filter(Func<T, bool> predicate, Func<Maybe<T>, TError> errorSelector) {
            if (Either.HasError) return this;
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (predicate(Either.Value)) return this;
            if (errorSelector == null)
                throw new ArgumentNullException(nameof(errorSelector));
            return ResultExtensions.Error<T, TError>(errorSelector(ResultExtensions.NullCheckedMaybe(Either.Value)));
        }

        [Pure]
        public Result<T, TError> IsErrorWhen(
            Func<T, bool> predicate, Func<Maybe<T>, TError> errorSelector) {
            return Filter(
                x => predicate == null
                    ? throw new ArgumentNullException(nameof(predicate))
                    : predicate(x) == false,
                errorSelector
            );
        }

        [Pure]
        public Result<T, TError> IsErrorWhenNull(Func<Maybe<T>, TError> errorSelector) =>
            IsErrorWhen(EqualityFunctions.IsNull, errorSelector);

        /// <summary>
        ///     Maps both <typeparamref name="T" /> and <typeparamref name="TError" /> but only one is executed.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <typeparamref name="T" /> is the active type.
        /// </param>
        /// <param name="errorSelector">
        ///     Is executed if <typeparamref name="TError" /> is the active type.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The result from the function <paramref name="errorSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The result from the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     A mapped <see cref="Result{T,TError}" />.
        /// </returns>
        [Pure]
        public Result<TResult, TErrorResult> FullMap<TResult, TErrorResult>(
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => Either.HasError
            ? ResultExtensions.Error<TResult, TErrorResult>(errorSelector != null
                ? errorSelector(Either.Error)
                : throw new ArgumentNullException(nameof(errorSelector)))
            : ResultExtensions.Value<TResult, TErrorResult>(selector != null
                ? selector(Either.Value)
                : throw new ArgumentNullException(nameof(selector)));

        /// <summary>
        ///     Maps <typeparamref name="T" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <typeparamref name="T" /> is the active type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The result from the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />  with its <typeparamref name="T" /> mapped.
        /// </returns>
        [Pure]
        public Result<TResult, TError> Map<TResult>(Func<T, TResult> selector) => Either.HasValue
            ? selector != null
                ? ResultExtensions.Value<TResult, TError>(selector(Either.Value))
                : throw new ArgumentNullException(nameof(selector))
            : ResultExtensions.Error<TResult, TError>(Either.Error);

        /// <summary>
        ///     Maps <typeparamref name="TError" />.
        /// </summary>
        /// <param name="selector">
        ///     Is executed if <typeparamref name="TError" /> is the active type.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The result from the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />  with its <typeparamref name="TError" /> mapped.
        /// </returns>
        [Pure]
        public Result<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) => Either.HasError
            ? selector != null
                ? ResultExtensions.Error<T, TErrorResult>(selector(Either.Error))
                : throw new ArgumentNullException(nameof(selector))
            : ResultExtensions.Value<T, TErrorResult>(Either.Value);

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" /> who shares the same <typeparamref name="TError" />.
        ///     And maps <typeparamref name="T" /> to <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as an return type.
        /// </param>
        /// <typeparam name="TResult">
        ///     The return type of the function <paramref name="flatSelector" />.
        /// </typeparam>
        [Pure]
        public Result<TResult, TError> FlatMap<TResult>(
            Func<T, Result<TResult, TError>> flatSelector) => Either.HasValue
            ? flatSelector?.Invoke(Either.Value) ?? throw new ArgumentNullException(nameof(flatSelector))
            : ResultExtensions.Error<TResult, TError>(Either.Error);

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" /> who shares the same <typeparamref name="TError" />.
        ///     And maps <typeparamref name="T" /> together with <typeparamref name="TSelector" /> to
        ///     <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="flatSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as an return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and  <typeparamref name="TSelector" />  which can
        ///     return any type.
        /// </param>
        /// <typeparam name="TSelector">
        ///     The value retrieved from the the <see cref="Result{T,TError}" /> given by the <paramref name="flatSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The return type of the function  <paramref name="resultSelector" />.
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
        ///     Flatmaps another <see cref="Result{T,TError}" /> but the <typeparamref name="TError" /> remains as the same type.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TErrorResult" /> to <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When any of the function parameters are null and needs to be executed.
        /// </exception>
        [Pure]
        public Result<TResult, TError> FlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            if (Either.HasError) return ResultExtensions.Error<TResult, TError>(Either.Error);
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            var okSelector = flatMapSelector(Either.Value);

            return okSelector.Either.HasValue
                ? ResultExtensions.Value<TResult, TError>(okSelector.Either.Value)
                : okSelector.MapError(errorSelector);
        }

        /// <summary>
        ///     Flatmaps another <see cref="Result{T,TError}" /> but the <typeparamref name="TError" /> remains as the same type.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TFlatMap" /> which can return
        ///     any type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TErrorResult" />. to <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" /> function.
        /// </typeparam>
        /// <typeparam name="TFlatMap">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        [Pure]
        public Result<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            FlatMap(x => flatMapSelector?.Invoke(x).Map(y => {
                    if (resultSelector == null)
                        throw new ArgumentNullException(nameof(resultSelector));
                    return resultSelector(x, y);
                }) ?? throw new ArgumentNullException(nameof(flatMapSelector)),
                errorSelector);

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" />.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Result{T,TError}" /> returned is not the the result from <paramref name="selector" />.
        /// </remarks>
        /// <param name="selector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as an return type.
        /// </param>
        /// <param name="errorSelector">
        ///     Maps the error to from <typeparamref name="TErrorResult" /> to <typeparamref name="TError" />.
        /// </param>
        /// <typeparam name="TResult">
        ///     The value of the <see cref="Result{T,TError}" /> returned by the function <paramref name="selector" />.
        /// </typeparam>
        /// <typeparam name="TErrorResult">
        ///     The error of the <see cref="Result{T,TError}" /> returned by the function <paramref name="selector" />.
        /// </typeparam>
        /// <returns>
        ///     The same <see cref="Result{T,TError}" /> but it's state is dependent on the <see cref="Result{T,TError}" />
        ///     returned by the <paramref name="selector" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     When any of the function parameters are null and needs to be executed.
        /// </exception>
        [Pure]
        public Result<T, TError> Flatten<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) {
            if (Either.HasError) return ResultExtensions.Error<T, TError>(Either.Error);
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var okSelector = selector(Either.Value);
            if (okSelector.Either.HasValue)
                return ResultExtensions.Value<T, TError>(Either.Value);
            var tmpThis = this;
            return okSelector.FullMap(x => tmpThis.Either.Value, errorSelector);
        }

        /// <summary>
        ///     Flatten another <see cref="Result{T,TError}" />  who shares the same <typeparamref name="TError" />.
        /// </summary>
        /// <remarks>
        ///     The <see cref="Result{T,TError}" /> returned is not the the result from <paramref name="selector" />.
        /// </remarks>
        /// <param name="selector"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [Pure]
        public Result<T, TError> Flatten<TResult>(Func<T, Result<TResult, TError>> selector) {
            if (Either.HasError) return ResultExtensions.Error<T, TError>(Either.Error);
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var okSelector = selector(Either.Value);
            return okSelector.Either.HasValue ? this : ResultExtensions.Error<T, TError>(okSelector.Either.Error);
        }

        /// <summary>
        ///     Executes each function and saves all potential errors to a list which will be the <typeparamref name="TError" />.
        /// </summary>
        /// <param name="validations">
        ///     A <see cref="IReadOnlyList{T}" /> containing <typeparamref name="TError" />.
        /// </param>
        public Result<T, IReadOnlyList<TError>> Multiple(
            params Func<Result<T, TError>, Result<T, TError>>[] validations) {
            var result = this;
            var errors = validations.Select(x => x(result)).Errors().ToList();
            return errors.Any()
                ? ResultExtensions.Error<T, IReadOnlyList<TError>>(errors)
                : ResultExtensions.Value<T, IReadOnlyList<TError>>(Either.Value);
        }

        /// <summary>
        ///     Fully flatmaps another <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TError" /> to <typeparamref name="TErrorResult" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <exception cref="ArgumentNullException">
        ///     When any of the function parameters are null and needs to be executed.
        /// </exception>
        [Pure]
        public Result<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) {
            if (Either.HasValue)
                return flatMapSelector?.Invoke(Either.Value) ??
                       throw new ArgumentNullException(nameof(flatMapSelector));

            return errorSelector != null
                ? ResultExtensions.Error<TResult, TErrorResult>(errorSelector(Either.Error))
                : throw new ArgumentNullException(nameof(errorSelector));
        }

        /// <summary>
        ///     Casts <typeparamref name="TError" /> into <typeparamref name="TResult" />.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to <typeparamref name="TError" /> cast to.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="TError" /> has been cast to
        ///     <typeparamref name="TResult" />.
        /// </returns>
        [Pure]
        public Result<T, TResult> CastError<TResult>() => Either.HasValue
            ? ResultExtensions.Value<T, TResult>(Either.Value)
            : ResultExtensions.Error<T, TResult>((TResult) (object) Either.Error);

        /// <summary>
        ///     Casts both <typeparamref name="T" /> into <typeparamref name="TResult" /> and <typeparamref name="TError" /> into
        ///     <typeparamref name="TErrorResult" />
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast <typeparamref name="T" /> into.
        /// </typeparam>
        /// <typeparam name="TErrorResult">
        ///     The type to cast <typeparamref name="TErrorResult" /> into.
        /// </typeparam>
        [Pure]
        public Result<TResult, TErrorResult> FullCast<TResult, TErrorResult>() => Either.HasValue
            ? ResultExtensions.Value<TResult, TErrorResult>((TResult) (object) Either.Value)
            : ResultExtensions.Error<TResult, TErrorResult>((TErrorResult) (object) Either.Error);

        /// <summary>
        ///     Casts both <typeparamref name="T" /> into <typeparamref name="TResult" /> and <typeparamref name="TError" /> into
        ///     <typeparamref name="TResult" />
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns></returns>
        [Pure]
        public Result<TResult, TResult> FullCast<TResult>() => FullCast<TResult, TResult>();

        /// <summary>
        ///     Casts <typeparamref name="T" /> into <typeparamref name="TResult" />.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="T" /> has been cast to
        ///     <typeparamref name="TResult" />.
        /// </returns>
        [Pure]
        public Result<TResult, TError> Cast<TResult>() => Either.HasError
            ? ResultExtensions.Error<TResult, TError>(Either.Error)
            : ResultExtensions.Value<TResult, TError>((TResult) (object) Either.Value);

        /// <summary>
        ///     Attempts to cast <typeparamref name="T" /> into <typeparamref name="TResult" />.
        /// </summary>
        /// <param name="errorSelector">
        ///     Is executed if the cast would fail.
        /// </param>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose <typeparamref name="T" /> has been cast to
        ///     <typeparamref name="TResult" />.
        /// </returns>
        [Pure]
        public Result<TResult, TError> SafeCast<TResult>(Func<TError> errorSelector) {
            if (Either.HasError) return ResultExtensions.Error<TResult, TError>(Either.Error);
            if (Either.Value is TResult result)
                return ResultExtensions.Value<TResult, TError>(result);

            return errorSelector != null
                ? ResultExtensions.Error<TResult, TError>(errorSelector())
                : throw new ArgumentNullException(nameof(errorSelector));
        }

        /// <summary>
        ///     Zip two <see cref="Result{T,TError}" /> when matched with a key.
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="inner">The other <see cref="Result{T,TError}" />.</param>
        /// <param name="outerKeySelector">The key selector for this <see cref="Result{T,TError}" />.</param>
        /// <param name="innerKeySelector">The key selector for the other <see cref="Result{T,TError}" />.</param>
        /// <param name="resultSelector">The selector to determine the returning <see cref="Result{T,TError}" />.</param>
        /// <param name="errorSelector">Is invoked when keys do not match.</param>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />.
        /// </returns>
        public Result<TResult, TError> Join<TInner, TKey, TResult>(
            Result<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) => Join(inner, outerKeySelector,
            innerKeySelector, resultSelector, errorSelector,
            EqualityComparer<TKey>.Default);

        /// <summary>
        ///     Zip two <see cref="Result{T,TError}" /> when matched with a key.
        /// </summary>
        /// <typeparam name="TInner"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="inner">The other <see cref="Result{T,TError}" />.</param>
        /// <param name="outerKeySelector">The key selector for this <see cref="Result{T,TError}" />.</param>
        /// <param name="innerKeySelector">The key selector for the other <see cref="Result{T,TError}" />.</param>
        /// <param name="resultSelector">The selector to determine the returning <see cref="Result{T,TError}" />.</param>
        /// <param name="errorSelector">Is invoked when keys do not match.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}" /> to be used when matching keys.</param>
        /// <returns>
        ///     A <see cref="Result{T,TError}" />.
        /// </returns>
        public Result<TResult, TError> Join<TInner, TKey, TResult>(
            Result<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) {
            if (Either.HasError) return ResultExtensions.Error<TResult, TError>(Either.Error);
            if (inner.Either.HasError) return ResultExtensions.Error<TResult, TError>(inner.Either.Error);

            foreach (var result in this.ToEnumerable().Join(
                inner.ToEnumerable(),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer
            )) return ResultExtensions.Value<TResult, TError>(result);

            return errorSelector != null
                ? ResultExtensions.Error<TResult, TError>(errorSelector())
                : throw new ArgumentNullException(nameof(errorSelector));
        }

        /// <summary>
        ///     Fully flatmaps another <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="flatMapSelector">
        ///     A function who expects a <see cref="Result{T,TError}" /> as its return type.
        /// </param>
        /// <param name="resultSelector">
        ///     A function whose in-parameters are <typeparamref name="T" /> and <typeparamref name="TFlatMap" /> which can return
        ///     any type.
        /// </param>
        /// <param name="errorSelector">
        ///     A function which maps <typeparamref name="TError" /> to <typeparamref name="TErrorResult" />.
        /// </param>
        /// <typeparam name="TErrorResult">
        ///     The error type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TFlatMap">
        ///     The value type of the <see cref="Result{T,TError}" /> returned by the <paramref name="flatMapSelector" />.
        /// </typeparam>
        /// <typeparam name="TResult">
        ///     The type returned by the function <paramref name="resultSelector" />.
        /// </typeparam>
        /// <returns></returns>
        [Pure]
        public Result<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            FullFlatMap(x => flatMapSelector?.Invoke(x).Map(y => {
                    if (resultSelector == null)
                        throw new ArgumentNullException(nameof(resultSelector));
                    return resultSelector(x, y);
                }) ?? throw new ArgumentNullException(nameof(flatMapSelector)),
                errorSelector);

        /// <summary>
        ///     Merges two <see cref="Result{T,TError}" /> together to create a new <see cref="Result{T,TError}" />.
        /// </summary>
        /// <param name="other">
        ///     The other <see cref="Result{T,TError}" />.
        /// </param>
        /// <param name="resultSelector">
        ///     The selector which will determine the type of the returning <see cref="Result{T,TError}" />.
        /// </param>
        /// <typeparam name="TOther">The type of the other <see cref="Result{T,TError}" />.</typeparam>
        /// <typeparam name="TResult">The type of the returning <see cref="Result{T,TError}" />.</typeparam>
        /// <returns>
        ///     A <see cref="Result{T,TError}" /> whose value is the result for merging two <see cref="Result{T,TError}" />
        ///     together.
        /// </returns>
        [Pure]
        public Result<TResult, TError> Zip<TOther, TResult>(Result<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) => FlatMap(_ => other, resultSelector);

        internal readonly struct NonNullEither : IEither<T, TError> {
            public bool HasValue { get; }
            public bool HasError { get; }
            public TError Error { get; }
            public T Value { get; }

            /// <summary>
            ///  Only one one <typeparamref name="T"/> and <typeparamref name="TError"/> can be available to use.
            /// </summary>
            /// <param name="value">
            /// The potential value.
            /// </param>
            /// <param name="error">
            /// The potential error.
            /// </param>
            /// <param name="hasError">
            /// Is true when <paramref name="error"/> is available.
            /// </param>
            /// <param name="hasValue">
            /// Is true when <paramref name="value"/> is available.
            /// </param>
            /// <exception cref="ArgumentException">
            /// When <see cref="HasValue"/> and <see cref="HasError"/>  are both either false or true.
            /// </exception>
            /// <exception cref="ArgumentNullException">
            /// When either <see cref="Value"/> or <see cref="Error"/> is null at the same the corresponding <see cref="Boolean"/> value check is true.
            /// </exception>
            internal NonNullEither(in T value, in TError error, bool hasError, bool hasValue) {
                if (hasError == hasValue)
                    throw new ArgumentException(
                        $"{nameof(IEither<T, TError>)} properties \"{nameof(HasError)}\": {hasError} and \"{nameof(HasValue)}\": ({hasValue}), can not both be {hasValue}."
                    );

                Value = value;
                Error = error;
                // Verify that the active value can never be null.
                if (Value.IsNull() && hasValue)
                    throw new ArgumentNullException(
                        nameof(Value),
                        $"{nameof(IEither<T, TError>)} property \"{nameof(Value)}\" cannot be null."
                    );

                // Verify that the active value can never be null.
                if (Error.IsNull() && hasError)
                    throw new ArgumentNullException(
                        nameof(Error),
                        $"{nameof(IEither<T, TError>)} property \"{nameof(Error)}\" cannot be null."
                    );

                HasValue = hasValue;
                HasError = hasError;
            }
        }
    }
}