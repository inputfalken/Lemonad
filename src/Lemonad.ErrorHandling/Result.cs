using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Lemonad.ErrorHandling.Either;
using Lemonad.ErrorHandling.Internal;

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

        internal static IEither<T, TError> EitherFactory(in T value, in TError error, bool hasError, bool hasValue) =>
            new NonNullEither(in value, in error, hasError, hasValue);

        /// <summary>
        ///    Gets the <see cref="IEither{T,TError}"/> from the <see cref="Result{T,TError}"/> instance.
        /// </summary>
        public IEither<T, TError> Either { get; }

        private Result(in T value, in TError error, bool hasError, bool hasValue) =>
            Either = new NonNullEither(in value, in error, hasError, hasValue);

        private Result(IEither<T, TError> either) =>
            Either = new NonNullEither(either.Value, either.Error, either.HasError, either.HasValue);

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
        public TResult Match<TResult>(Func<T, TResult> selector, Func<TError, TResult> errorSelector) =>
            EitherMethods<T, TError>.Match(Either, selector, errorSelector);

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
        public void Match(Action<T> action, Action<TError> errorAction) =>
            EitherMethods<T, TError>.Match(Either, action, errorAction);

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
        public Result<T, TError> DoWith(Action<T> action) =>
            new Result<T, TError>(EitherMethods<T, TError>.DoWith(Either, action));

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
        public Result<T, TError> Do(Action action) =>
            new Result<T, TError>(EitherMethods<T, TError>.Do(Either, action));

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
            Action<TError> action) => new Result<T, TError>(EitherMethods<T, TError>.DoWithError(Either, action));

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
        public Result<T, TError> Filter(Func<T, bool> predicate, Func<T, TError> errorSelector) =>
            new Result<T, TError>(EitherMethods<T, TError>.Filter(Either, predicate, errorSelector));

        [Pure]
        public Result<T, TError> IsErrorWhen(
            Func<T, bool> predicate, Func<T, TError> errorSelector) =>
            new Result<T, TError>(EitherMethods<T, TError>.IsErrorWhen(Either, predicate, errorSelector));

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
        ) => new Result<TResult, TErrorResult>(EitherMethods<T, TError>.FullMap(Either, selector, errorSelector));

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
        public Result<TResult, TError> Map<TResult>(Func<T, TResult> selector) =>
            new Result<TResult, TError>(EitherMethods<T, TError>.Map(Either, selector));

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
        public Result<T, TErrorResult> MapError<TErrorResult>(Func<TError, TErrorResult> selector) =>
            new Result<T, TErrorResult>(EitherMethods<T, TError>.MapError(Either, selector));

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
            Func<T, Result<TResult, TError>> flatSelector) {
            return new Result<TResult, TError>(
                EitherMethods<T, TError>.FlatMap(Either, flatSelector.Compose(x => x.Either)));
        }

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
            Func<T, TSelector, TResult> resultSelector) => new Result<TResult, TError>(
            EitherMethods<T, TError>.FlatMap(Either, flatSelector.Compose(x => x.Either), resultSelector));

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
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            new Result<TResult, TError>(EitherMethods<T, TError>.FlatMap(Either, flatMapSelector.Compose(x => x.Either),
                errorSelector));

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
            Func<TErrorResult, TError> errorSelector) => new Result<TResult, TError>(
            EitherMethods<T, TError>.FlatMap(Either, flatMapSelector.Compose(x => x.Either), resultSelector,
                errorSelector));

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
            Func<T, Result<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) =>
            new Result<T, TError>(EitherMethods<T, TError>.Flatten(Either, selector.Compose(x => x.Either),
                errorSelector));

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
        public Result<T, TError> Flatten<TResult>(Func<T, Result<TResult, TError>> selector) =>
            new Result<T, TError>(EitherMethods<T, TError>.Flatten(Either, selector.Compose(x => x.Either)));

        /// <summary>
        ///     Executes each function and saves all potential errors to a list which will be the <typeparamref name="TError" />.
        /// </summary>
        /// <param name="validations">
        ///     A <see cref="IReadOnlyList{T}" /> containing <typeparamref name="TError" />.
        /// </param>
        public Result<T, IReadOnlyList<TError>> Multiple(
            params Func<Result<T, TError>, Result<T, TError>>[] validations) {
            var foo = this;
            var validation = validations.Select(x => x.Compose(y => y.Either)(foo)).ToArray();
            return new Result<T, IReadOnlyList<TError>>(EitherMethods<T, TError>.Multiple(Either, validation));
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
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) =>
            new Result<TResult, TErrorResult>(EitherMethods<T, TError>.FullFlatMap(Either,
                flatMapSelector.Compose(x => x.Either), errorSelector));

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
        public Result<T, TResult> CastError<TResult>() =>
            new Result<T, TResult>(EitherMethods<T, TError>.CastError<TResult>(Either));

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
        public Result<TResult, TErrorResult> FullCast<TResult, TErrorResult>() =>
            new Result<TResult, TErrorResult>(EitherMethods<T, TError>.FullCast<TResult, TErrorResult>(Either));

        /// <summary>
        ///     Casts both <typeparamref name="T" /> into <typeparamref name="TResult" /> and <typeparamref name="TError" /> into
        ///     <typeparamref name="TResult" />
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type to cast to.
        /// </typeparam>
        /// <returns></returns>
        [Pure]
        public Result<TResult, TResult> FullCast<TResult>() =>
            new Result<TResult, TResult>(EitherMethods<T, TError>.FullCast<TResult>(Either));

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
        public Result<TResult, TError> Cast<TResult>() =>
            new Result<TResult, TError>(EitherMethods<T, TError>.Cast<TResult>(Either));

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
        public Result<TResult, TError> SafeCast<TResult>(Func<T, TError> errorSelector) =>
            new Result<TResult, TError>(EitherMethods<T, TError>.SafeCast<TResult>(Either, errorSelector));

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
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) => new Result<TResult, TError>(
            EitherMethods<T, TError>.Join(Either, inner.Either, outerKeySelector, innerKeySelector, resultSelector,
                errorSelector));

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
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) =>
            new Result<TResult, TError>(EitherMethods<T, TError>.Join(Either, inner.Either, outerKeySelector,
                innerKeySelector, resultSelector, errorSelector, comparer));

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
            Func<TError, TErrorResult> errorSelector) => new Result<TResult, TErrorResult>(
            EitherMethods<T, TError>.FullFlatMap(Either, flatMapSelector.Compose(x => x.Either), resultSelector,
                errorSelector));

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
            Func<T, TOther, TResult> resultSelector) =>
            new Result<TResult, TError>(EitherMethods<T, TError>.Zip(Either, other.Either, resultSelector));

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