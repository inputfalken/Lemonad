using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Lemonad.ErrorHandling.Internal;

namespace Lemonad.ErrorHandling.Either {
    internal static class EitherMethods<T, TError> {
        private static IEither<T1, T2> CreateValue<T1, T2>(in T1 value) =>
            Result<T1, T2>.EitherFactory(value, default, false, true);

        private static IEither<T1, T2> CreateError<T1, T2>(in T2 error) =>
            Result<T1, T2>.EitherFactory(default, error, true, false);

        [Pure]
        public static TResult Match<TResult>(IEither<T, TError> either,
            Func<T, TResult> selector, Func<TError, TResult> errorSelector) {
            if (either.HasError)
                return errorSelector != null
                    ? errorSelector(either.Error)
                    : throw new ArgumentNullException(nameof(errorSelector));

            return selector != null
                ? selector(either.Value)
                : throw new ArgumentNullException(nameof(selector));
        }

        public static void Match(IEither<T, TError> either, Action<T> action, Action<TError> errorAction) {
            if (either.HasError)
                if (errorAction != null)
                    errorAction(either.Error);
                else
                    throw new ArgumentNullException(nameof(errorAction));
            else if (action != null)
                action(either.Value);
            else
                throw new ArgumentNullException(nameof(action));
        }

        public static IEither<T, TError> DoWith(IEither<T, TError> either, Action<T> action) {
            if (either.HasError) return either;
            if (action != null)
                action.Invoke(either.Value);
            else
                throw new ArgumentNullException(nameof(action));

            return either;
        }

        public static IEither<T, TError> Do(IEither<T, TError> either, Action action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action();
            return either;
        }

        public static IEither<T, TError> DoWithError(
            IEither<T, TError> either, Action<TError> action) {
            if (either.HasValue) return either;
            if (action != null)
                action.Invoke(either.Error);
            else
                throw new ArgumentNullException(nameof(action));

            return either;
        }

        [Pure]
        public static IEither<T, TError> Filter(IEither<T, TError> either, Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            if (errorSelector == null)
                throw new ArgumentNullException(nameof(errorSelector));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            if (either.HasError) return either;
            return predicate(either.Value)
                ? either
                : CreateError<T, TError>(errorSelector(either.Value));
        }

        [Pure]
        public static async Task<IEither<T, TError>> FilterAsyncPredicate(Task<IEither<T, TError>> source,
            Func<T, Task<bool>> predicate,
            Func<T, TError> errorSelector) {
            if (errorSelector == null)
                throw new ArgumentNullException(nameof(errorSelector));
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));
            var either = await source.ConfigureAwait(false);
            if (either.HasError) return either;
            return await predicate(either.Value).ConfigureAwait(false)
                ? either
                : CreateError<T, TError>(errorSelector(either.Value));
        }

        [Pure]
        internal static async Task<IEither<T, TError>> FilterAsync(
            Task<IEither<T, TError>> source,
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) => Filter(await source.ConfigureAwait(false), predicate, errorSelector);

        [Pure]
        public static IEither<T, TError> IsErrorWhen(IEither<T, TError> either,
            Func<T, bool> predicate, Func<T, TError> errorSelector) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (either.HasError)
                return ResultExtensions.Error<T, TError>(either.Error).Either;

            return predicate(either.Value)
                ? CreateError<T, TError>(errorSelector(either.Value))
                : CreateValue<T, TError>(either.Value);
        }

        [Pure]
        public static IEither<TResult, TErrorResult> FullMap<TResult, TErrorResult>(IEither<T, TError> either,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return either.HasError
                ? CreateError<TResult, TErrorResult>(errorSelector(either.Error))
                : CreateValue<TResult, TErrorResult>(selector(either.Value));
        }

        [Pure]
        public static IEither<TResult, TError> Map<TResult>(IEither<T, TError> either, Func<T, TResult> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return either.HasValue
                ? CreateValue<TResult, TError>(selector(either.Value))
                : CreateError<TResult, TError>(either.Error);
        }

        public static async Task<IEither<TResult, TError>> MapAsync<TResult>(Task<IEither<T, TError>> source,
            Func<T, TResult> selector) => Map(await source.ConfigureAwait(false), selector);

        [Pure]
        public static async Task<IEither<TResult, TError>> MapAsyncSelector<TResult>(Task<IEither<T, TError>> source,
            Func<T, Task<TResult>> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);
            return either.HasValue
                ? CreateValue<TResult, TError>(await selector(either.Value).ConfigureAwait(false))
                : CreateError<TResult, TError>(either.Error);
        }

        [Pure]
        public static IEither<T, TErrorResult> MapError<TErrorResult>(IEither<T, TError> either,
            Func<TError, TErrorResult> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return either.HasError
                ? CreateError<T, TErrorResult>(selector(either.Error))
                : CreateValue<T, TErrorResult>(either.Value);
        }

        [Pure]
        public static IEither<TResult, TError> FlatMap<TResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TError>> flatSelector) {
            if (flatSelector == null) throw new ArgumentNullException(nameof(flatSelector));

            return either.HasValue ? flatSelector(either.Value) : CreateError<TResult, TError>(either.Error);
        }

        [Pure]
        public static IEither<TResult, TError> FlatMap<TSelector, TResult>(IEither<T, TError> either,
            Func<T, IEither<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) {
            if (flatSelector == null) throw new ArgumentNullException(nameof(flatSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));

            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            var eitherFunc = flatSelector(either.Value);
            return eitherFunc.HasValue
                ? CreateValue<TResult, TError>(resultSelector(either.Value, eitherFunc.Value))
                : CreateError<TResult, TError>(eitherFunc.Error);
        }

        [Pure]
        public static IEither<TResult, TError> FlatMap<TResult, TErrorResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            var okSelector = flatMapSelector(either.Value);
            return okSelector.HasValue
                ? CreateValue<TResult, TError>(okSelector.Value)
                : CreateError<TResult, TError>(errorSelector(okSelector.Error));
        }

        [Pure]
        public static IEither<TResult, TError> FlatMap<TFlatMap, TResult, TErrorResult>(IEither<T, TError> either,
            Func<T, IEither<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (either.HasError)
                return CreateError<TResult, TError>(either.Error);
            var eitherSelector = flatMapSelector(either.Value);

            return eitherSelector.HasValue
                ? CreateValue<TResult, TError>(resultSelector(either.Value, eitherSelector.Value))
                : CreateError<TResult, TError>(errorSelector(eitherSelector.Error));
        }

        [Pure]
        public static IEither<T, TError> Flatten<TResult, TErrorResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (either.HasError) return CreateError<T, TError>(either.Error);
            var okSelector = selector(either.Value);
            return okSelector.HasValue
                ? either
                : CreateError<T, TError>(errorSelector(okSelector.Error));
        }

        [Pure]
        public static IEither<T, TError> Flatten<TResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TError>> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (either.HasError) return CreateError<T, TError>(either.Error);
            var okSelector = selector(either.Value);
            return okSelector.HasValue
                ? either
                : CreateError<T, TError>(okSelector.Error);
        }

        public static IEither<T, IReadOnlyList<TError>> Multiple(IEither<T, TError> either,
            IEnumerable<IEither<T, TError>> validations) {
            if (validations == null) throw new ArgumentNullException(nameof(validations));
            var all = validations.ToArray();
            if (all.Length > 0)
                throw new ArgumentException("An element must be provided for the array.", nameof(validations));

            var errors = all.Where(x => x.HasError).Select(x => x.Error).ToArray();

            return errors.Any()
                ? CreateError<T, IReadOnlyList<TError>>(errors)
                : CreateValue<T, IReadOnlyList<TError>>(either.Value);
        }

        [Pure]
        public static IEither<TResult, TErrorResult> FullFlatMap<TResult, TErrorResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));

            return either.HasValue
                ? flatMapSelector(either.Value)
                : CreateError<TResult, TErrorResult>(errorSelector(either.Error));
        }

        [Pure]
        public static IEither<T, TResult> CastError<TResult>(IEither<T, TError> either) => either.HasValue
            ? CreateValue<T, TResult>(either.Value)
            : CreateError<T, TResult>((TResult) (object) either.Error);

        [Pure]
        public static IEither<TResult, TErrorResult> FullCast<TResult, TErrorResult>(IEither<T, TError> either) =>
            either.HasValue
                ? CreateValue<TResult, TErrorResult>((TResult) (object) either.Value)
                : CreateError<TResult, TErrorResult>((TErrorResult) (object) either.Error);

        [Pure]
        public static IEither<TResult, TResult> FullCast<TResult>(IEither<T, TError> either) =>
            FullCast<TResult, TResult>(either);

        [Pure]
        public static IEither<TResult, TError> Cast<TResult>(IEither<T, TError> either) => either.HasError
            ? CreateError<TResult, TError>(either.Error)
            : CreateValue<TResult, TError>((TResult) (object) either.Value);

        [Pure]
        public static IEither<TResult, TError>
            SafeCast<TResult>(IEither<T, TError> either, Func<T, TError> errorSelector) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (either.HasError) return CreateError<TResult, TError>(either.Error);

            return either.Value is TResult result
                ? CreateValue<TResult, TError>(result)
                : CreateError<TResult, TError>(errorSelector(either.Value));
        }

        public static IEither<TResult, TError> Join<TInner, TKey, TResult>(IEither<T, TError> either,
            IEither<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) => Join(either, inner,
            outerKeySelector,
            innerKeySelector, resultSelector, errorSelector,
            EqualityComparer<TKey>.Default);

        public static IEither<TResult, TError> Join<TInner, TKey, TResult>(IEither<T, TError> either,
            IEither<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector, IEqualityComparer<TKey> comparer) {
            if (outerKeySelector == null)
                throw new ArgumentNullException(nameof(outerKeySelector));
            if (innerKeySelector == null)
                throw new ArgumentNullException(nameof(inner));
            if (resultSelector == null)
                throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector == null)
                throw new ArgumentNullException(nameof(errorSelector));

            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            if (inner.HasError) return CreateError<TResult, TError>(inner.Error);

            foreach (var result in YieldValues(either).Join(
                YieldValues(inner),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                comparer
            )) return CreateValue<TResult, TError>(result);
            return CreateError<TResult, TError>(errorSelector());
        }

        public static IEnumerable<T2> YieldErrors<T1, T2>(IEither<T1, T2> result) {
            if (result.HasError)
                yield return result.Error;
        }

        public static IEnumerable<T1> YieldValues<T1, T2>(IEither<T1, T2> result) {
            if (result.HasValue)
                yield return result.Value;
        }

        [Pure]
        public static IEither<TResult, TErrorResult> FullFlatMap<TFlatMap, TResult, TErrorResult>(
            IEither<T, TError> either,
            Func<T, IEither<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) {
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (either.HasError) return CreateError<TResult, TErrorResult>(errorSelector(either.Error));
            var mapSelector = flatMapSelector(either.Value);
            return mapSelector.HasValue
                ? CreateValue<TResult, TErrorResult>(resultSelector(either.Value, mapSelector.Value))
                : CreateError<TResult, TErrorResult>(mapSelector.Error);
        }

        [Pure]
        public static IEither<TResult, TError> Zip<TOther, TResult>(IEither<T, TError> either,
            IEither<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            return other.HasError
                ? CreateError<TResult, TError>(other.Error)
                : CreateValue<TResult, TError>(resultSelector(either.Value, other.Value));
        }

        public static Task<IEither<T, TError>> IsErrorWhenAsyncPredicate(Task<IEither<T, TError>> either,
            Func<T, Task<bool>> predicate, Func<Maybe<T>, TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TError>> IsErrorWhenAsync(Task<IEither<T, TError>> either,
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TError>> DoAsync(Task<IEither<T, TError>> taskResult, Action action) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TError>> DoWithErrorAsync(Task<IEither<T, TError>> taskResult,
            Action<TError> action) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TError>> DoWithAsync(Task<IEither<T, TError>> taskResult, Action<T> action) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TErrorResult>> FullMapAsync<TResult, TErrorResult, T, TError>(
            Task<IEither<T, TError>> taskResult, Func<T, TResult> selector, Func<TError, TErrorResult> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<TResult> MatchAsync<TResult>(Task<IEither<T, TError>> taskResult, Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task MatchAsync(Task<IEither<T, TError>> taskResult, Action<T> selector,
            Action<TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TError>> FlatMapAsync<TResult>(Task<IEither<T, TError>> either,
            Func<T, Func<T, Task<IEither<TResult, TError>>>> func) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TError>> FlatMapAsync<TResult, TSelector>(Task<IEither<T, TError>> either,
            Func<T, Func<T, Task<IEither<TSelector, TError>>>> func, Func<T, TSelector, TResult> resultSelector) {
            throw new NotImplementedException();
        }

        [Pure]
        internal static async Task<IEither<TResult, TError>> FlatMapAsync<TResult, TErrorResult>(
            Task<IEither<T, TError>> source,
            Func<T, Task<IEither<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TError>> FlatMapAsync<TResult, TFlatMap, TErrorResult>(
            Task<IEither<T, TError>> either, Func<T, Task<IEither<TFlatMap, TErrorResult>>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector, Func<TErrorResult, TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TError>> FlattenAsync<TResult, TErrorResult>(Task<IEither<T, TError>> either,
            Func<T, Task<IEither<TResult, TErrorResult>>> compose, Func<TErrorResult, TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TError>> FlattenAsync<TResult>(Task<IEither<T, TError>> either,
            Func<T, Task<IEither<TResult, TError>>> compose) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TErrorResult>> FullCastAsync<TResult, TErrorResult>(
            Task<IEither<T, TError>> either) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TResult>> FullCastAsync<TResult>(Task<IEither<T, TError>> either) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TResult>> CastErrorAsync<TResult>(Task<IEither<T, TError>> either) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TError>> SafeCastAsync<TResult>(Task<IEither<T, TError>> either,
            Func<T, TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TErrorResult>> FullFlatMapAsync<TFlatMap, TErrorResult, TResult>(
            Task<IEither<T, TError>> either, Func<T, Task<IEither<TFlatMap, TErrorResult>>> compose,
            Func<T, TFlatMap, TResult> resultSelector, Func<TError, TErrorResult> errorSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TErrorResult>> FullFlatMapAsync<TResult, TErrorResult>(
            Task<IEither<T, TError>> either, Func<T, Task<IEither<TResult, TErrorResult>>> compose,
            Func<TError, TErrorResult> resultSelector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TError>> JoinAsync<TResult, TInner, TKey>(Task<IEither<T, TError>> either,
            Task<IEither<TInner, TError>> innerEither, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector, Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer) {
            throw new NotImplementedException();
        }

        public static Task<IEither<TResult, TError>> JoinAsync<TResult, TInner, TKey>(Task<IEither<T, TError>> either,
            Task<IEither<TInner, TError>> innerEither, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector, Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) {
            throw new NotImplementedException();
        }

        public static AsyncResult<TResult, TError> ZipAsync<TResult, TOther>(Task<IEither<T, TError>> either,
            Task<IEither<TOther, TError>> otherEither, Func<T, TOther, TResult> resultSelector) {
            throw new NotImplementedException();
        }

        public static AsyncResult<TResult, TError> CastAsync<TResult>(Task<IEither<T, TError>> either) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TErrorResult>> MapErrorAsyncSelector< TErrorResult>(Task<IEither<T, TError>> either, Func<TError, Task<TErrorResult>> selector) {
            throw new NotImplementedException();
        }

        public static Task<IEither<T, TErrorResult>> MapErrorAsync<T, TErrorResult >(Task<IEither<T, TError>> either, Func<TError, TErrorResult> selector) {
            throw new NotImplementedException();
        }
    }
}