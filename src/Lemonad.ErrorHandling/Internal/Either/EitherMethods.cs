using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal.Either {
    /// <summary>
    ///     This class will be used to share the logic between <see cref="IAsyncResult{T,TError}" /> and
    ///     <see cref="IResult{T,TError}" />.
    ///     The goal is to make <see cref="IAsyncResult{T,TError}" /> methods execute <see cref="IResult{T,TError}" />  methods
    ///     so they
    ///     share as much logic as possible.
    /// </summary>
    internal static class EitherMethods {
        [Pure]
        internal static IEither<TResult, TError> Cast<T, TResult, TError>(IEither<T, TError> either) => either.HasError
            ? CreateError<TResult, TError>(either.Error)
            : CreateValue<TResult, TError>((TResult) (object) either.Value);

        internal static async Task<IEither<TResult, TError>> CastAsync<T, TResult, TError>(
            Task<IEither<T, TError>> either) =>
            Cast<T, TResult, TError>(await either.ConfigureAwait(false));

        [Pure]
        internal static IEither<T, TResult> CastError<T, TResult, TError>(IEither<T, TError> either) => either.HasValue
            ? CreateValue<T, TResult>(either.Value)
            : CreateError<T, TResult>((TResult) (object) either.Error);

        internal static async Task<IEither<T, TResult>> CastErrorAsync<T, TResult, TError>(
            Task<IEither<T, TError>> either) => CastError<T, TResult, TError>(await either.ConfigureAwait(false));

        private static IEither<T1, T2> CreateError<T1, T2>(in T2 error) =>
            new NonNullableEither<T1, T2>(default, error, true, false);

        private static IEither<T1, T2> CreateValue<T1, T2>(in T1 value) =>
            new NonNullableEither<T1, T2>(value, default, false, true);

        internal static IEither<T, TError> Do<T, TError>(IEither<T, TError> either, Action action) {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            action();
            return either;
        }

        internal static async Task<IEither<T, TError>> Do<T, TError>(Task<IEither<T, TError>> taskResult,
            Action action) =>
            Do(await taskResult.ConfigureAwait(false), action);

        public static async Task<IEither<T, TError>> DoAsync<T, TError>(
            Task<IEither<T, TError>> source,
            Func<Task> selector
        ) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            await selector();
            return await source;
        }

        internal static IEither<T, TError> DoWith<T, TError>(IEither<T, TError> either, Action<T> action) {
            if (either.HasError) return either;
            if (action != null)
                action.Invoke(either.Value);
            else
                throw new ArgumentNullException(nameof(action));

            return either;
        }

        public static async Task<IEither<T, TError>> DoWithAsync<T, TError>(
            Task<IEither<T, TError>> source,
            Func<T, Task> selector
        ) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);
            if (either.HasValue) await selector(either.Value);

            return either;
        }

        internal static async Task<IEither<T, TError>> DoWithAsync<T, TError>(Task<IEither<T, TError>> taskResult,
            Action<T> action) =>
            DoWith(await taskResult.ConfigureAwait(false), action);

        internal static IEither<T, TError> DoWithError<T, TError>(
            IEither<T, TError> either, Action<TError> action) {
            if (either.HasValue) return either;
            if (action != null)
                action.Invoke(either.Error);
            else
                throw new ArgumentNullException(nameof(action));

            return either;
        }

        internal static async Task<IEither<T, TError>> DoWithError<T, TError>(Task<IEither<T, TError>> taskResult,
            Action<TError> action) => DoWithError(await taskResult.ConfigureAwait(false), action);

        public static async Task<IEither<T, TError>> DoWithErrorAsync<T, TError>(
            Task<IEither<T, TError>> source,
            Func<TError, Task> selector
        ) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);
            if (either.HasError) await selector(either.Error);

            return either;
        }

        [Pure]
        internal static IEither<T, TError> Filter<T, TError>(IEither<T, TError> either, Func<T, bool> predicate,
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
        internal static async Task<IEither<T, TError>> FilterAsync<T, TError>(
            Task<IEither<T, TError>> source,
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) => Filter(await source.ConfigureAwait(false), predicate, errorSelector);

        [Pure]
        internal static async Task<IEither<T, TError>> FilterAsyncPredicate<T, TError>(Task<IEither<T, TError>> source,
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
        internal static IEither<TResult, TError> FlatMap<T, TResult, TError>(IEither<T, TError> either,
            Func<T, IEither<TResult, TError>> flatSelector) {
            if (flatSelector == null) throw new ArgumentNullException(nameof(flatSelector));

            return either.HasValue ? flatSelector(either.Value) : CreateError<TResult, TError>(either.Error);
        }

        [Pure]
        internal static IEither<TResult, TError> FlatMap<T, TSelector, TResult, TError>(IEither<T, TError> either,
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
        internal static IEither<TResult, TError> FlatMap<T, TResult, TError, TErrorResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            var okSelector = flatMapSelector(either.Value);
            return okSelector.HasValue
                ? CreateValue<TResult, TError>(okSelector.Value)
                : CreateError<TResult, TError>(errorSelector(okSelector.Error));
        }

        [Pure]
        internal static IEither<TResult, TError> FlatMap<T, TFlatMap, TResult, TError, TErrorResult>(
            IEither<T, TError> either,
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

        public static async Task<IEither<TResult, TError>> FlatMapAsync<T, TResult, TError>(
            Task<IEither<T, TError>> source,
            Func<T, Task<IEither<TResult, TError>>> compose) {
            if (compose == null) throw new ArgumentNullException(nameof(compose));
            var either = await source.ConfigureAwait(false);

            return either.HasValue
                ? await compose(either.Value).ConfigureAwait(false)
                : CreateError<TResult, TError>(either.Error);
        }

        [Pure]
        internal static async Task<IEither<TResult, TError>> FlatMapAsync<T, TResult, TError, TErrorResult>(
            Task<IEither<T, TError>> source,
            Func<T, Task<IEither<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) {
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            var either = await source.ConfigureAwait(false);
            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            var okSelector = await flatMapSelector(either.Value).ConfigureAwait(false);
            return okSelector.HasValue
                ? CreateValue<TResult, TError>(okSelector.Value)
                : CreateError<TResult, TError>(errorSelector(okSelector.Error));
        }

        internal static Task<IEither<TResult, TError>> FlatMapAsync<T, TSelector, TResult, TError>(
            Task<IEither<T, TError>> either,
            Func<T, Task<IEither<TSelector, TError>>> func, Func<T, TSelector, TResult> resultSelector) =>
            FlatMapAsync(either, x => MapAsync(func(x), y => resultSelector(x, y)));

        internal static async Task<IEither<TResult, TError>> FlatMapAsync<T, TFlatMap, TResult, TError, TErrorResult>(
            Task<IEither<T, TError>> source, Func<T, Task<IEither<TFlatMap, TErrorResult>>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector, Func<TErrorResult, TError> errorSelector) {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            var either = await source.ConfigureAwait(false);

            if (either.HasError)
                return CreateError<TResult, TError>(either.Error);

            var eitherSelector = await flatMapSelector(either.Value);

            return eitherSelector.HasValue
                ? CreateValue<TResult, TError>(resultSelector(either.Value, eitherSelector.Value))
                : CreateError<TResult, TError>(errorSelector(eitherSelector.Error));
        }

        [Pure]
        internal static IEither<T, TError> Flatten<T, TResult, TError, TErrorResult>(IEither<T, TError> either,
            Func<T, IEither<TResult, TErrorResult>> selector, Func<TErrorResult, TError> errorSelector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (either.HasError) return CreateError<T, TError>(either.Error);
            var okSelector = selector(either.Value);
            return okSelector.HasValue
                ? either
                : CreateError<T, TError>(errorSelector(okSelector.Error));
        }

        [Pure]
        internal static IEither<T, TError> Flatten<T, TResult, TError>(IEither<T, TError> either,
            Func<T, IEither<TResult, TError>> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (either.HasError) return CreateError<T, TError>(either.Error);
            var okSelector = selector(either.Value);
            return okSelector.HasValue
                ? either
                : CreateError<T, TError>(okSelector.Error);
        }

        internal static async Task<IEither<T, TError>> FlattenAsync<T, TResult, TError, TErrorResult>(
            Task<IEither<T, TError>> source,
            Func<T, Task<IEither<TResult, TErrorResult>>> selector, Func<TErrorResult, TError> errorSelector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);
            if (either.HasError) return CreateError<T, TError>(either.Error);
            var okSelector = await selector(either.Value).ConfigureAwait(false);
            return okSelector.HasValue
                ? either
                : CreateError<T, TError>(errorSelector(okSelector.Error));
        }

        internal static async Task<IEither<T, TError>> FlattenAsync<T, TResult, TError>(Task<IEither<T, TError>> source,
            Func<T, Task<IEither<TResult, TError>>> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);
            if (either.HasError) return CreateError<T, TError>(either.Error);
            var okSelector = await selector(either.Value).ConfigureAwait(false);
            return okSelector.HasValue
                ? either
                : CreateError<T, TError>(okSelector.Error);
        }

        [Pure]
        internal static IEither<TResult, TErrorResult> FullCast<T, TResult, TError, TErrorResult>(
            IEither<T, TError> either) =>
            either.HasValue
                ? CreateValue<TResult, TErrorResult>((TResult) (object) either.Value)
                : CreateError<TResult, TErrorResult>((TErrorResult) (object) either.Error);

        [Pure]
        internal static IEither<TResult, TResult> FullCast<T, TResult, TError>(IEither<T, TError> either) =>
            FullCast<T, TResult, TError, TResult>(either);

        internal static async Task<IEither<TResult, TErrorResult>> FullCastAsync<T, TResult, TError, TErrorResult>(
            Task<IEither<T, TError>> either) =>
            FullCast<T, TResult, TError, TErrorResult>(await either.ConfigureAwait(false));

        internal static async Task<IEither<TResult, TResult>> FullCastAsync<T, TResult, TError>(
            Task<IEither<T, TError>> either) => FullCast<T, TResult, TError>(await either.ConfigureAwait(false));

        internal static IEither<TResult, TErrorResult> FullFlatMap<T, TResult, TError, TErrorResult>(
            IEither<T, TError> either,
            Func<T, IEither<TResult, TErrorResult>> flatMapSelector, Func<TError, TErrorResult> errorSelector) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));

            return either.HasValue
                ? flatMapSelector(either.Value)
                : CreateError<TResult, TErrorResult>(errorSelector(either.Error));
        }

        [Pure]
        internal static IEither<TResult, TErrorResult> FullFlatMap<T, TFlatMap, TResult, TError, TErrorResult>(
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

        internal static async Task<IEither<TResult, TErrorResult>> FullFlatMapAsync<T, TFlatMap, TResult, TError,
            TErrorResult>(
            Task<IEither<T, TError>> source, Func<T, Task<IEither<TFlatMap, TErrorResult>>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector, Func<TError, TErrorResult> errorSelector) {
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            var either = await source.ConfigureAwait(false);
            if (either.HasError) return CreateError<TResult, TErrorResult>(errorSelector(either.Error));
            var mapSelector = await flatMapSelector(either.Value).ConfigureAwait(false);
            return mapSelector.HasValue
                ? CreateValue<TResult, TErrorResult>(resultSelector(either.Value, mapSelector.Value))
                : CreateError<TResult, TErrorResult>(mapSelector.Error);
        }

        internal static async Task<IEither<TResult, TErrorResult>> FullFlatMapAsync<T, TResult, TError, TErrorResult>(
            Task<IEither<T, TError>> source,
            Func<T, Task<IEither<TResult, TErrorResult>>> flatMapSelector, Func<TError, TErrorResult> errorSelector) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            var either = await source.ConfigureAwait(false);

            return either.HasValue
                ? await flatMapSelector(either.Value).ConfigureAwait(false)
                : CreateError<TResult, TErrorResult>(errorSelector(either.Error));
        }

        [Pure]
        internal static IEither<TResult, TErrorResult> FullMap<T, TResult, TError, TErrorResult>(
            IEither<T, TError> either,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return either.HasError
                ? CreateError<TResult, TErrorResult>(errorSelector(either.Error))
                : CreateValue<TResult, TErrorResult>(selector(either.Value));
        }

        public static async Task<IEither<TResult, TErrorResult>> FullMapAsync<TResult, TErrorResult, T,
            TError>(
            Task<IEither<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);

            return either.HasError
                ? CreateError<TResult, TErrorResult>(await errorSelector(either.Error).ConfigureAwait(false))
                : CreateValue<TResult, TErrorResult>(selector(either.Value));
        }

        public static async Task<IEither<TResult, TErrorResult>> FullMapAsync<TResult, TErrorResult, T,
            TError>(
            Task<IEither<T, TError>> source,
            Func<T, Task<TResult>> selector,
            Func<TError, Task<TErrorResult>> errorSelector
        ) {
            var fullMap = FullMap(await source.ConfigureAwait(false), selector, errorSelector);
            return fullMap.HasError
                ? CreateError<TResult, TErrorResult>(await fullMap.Error.ConfigureAwait(false))
                : CreateValue<TResult, TErrorResult>(await fullMap.Value.ConfigureAwait(false));
        }

        public static async Task<IEither<TResult, TErrorResult>> FullMapAsync<TResult, TErrorResult, T, TError>(
            Task<IEither<T, TError>> source,
            Func<T, Task<TResult>> selector,
            Func<TError, TErrorResult> errorSelector
        ) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var either = await source.ConfigureAwait(false);

            return either.HasError
                ? CreateError<TResult, TErrorResult>(errorSelector(either.Error))
                : CreateValue<TResult, TErrorResult>(await selector(either.Value).ConfigureAwait(false));
        }

        internal static async Task<IEither<TResult, TErrorResult>> FullMapAsync<T, TResult, TError, TErrorResult>(
            Task<IEither<T, TError>> source, Func<T, TResult> selector, Func<TError, TErrorResult> errorSelector) =>
            FullMap(await source.ConfigureAwait(false), selector, errorSelector);

        [Pure]
        internal static IEither<T, TError> IsErrorWhen<T, TError>(IEither<T, TError> either,
            Func<T, bool> predicate, Func<T, TError> errorSelector) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (either.HasError)
                return either;

            return predicate(either.Value)
                ? CreateError<T, TError>(errorSelector(either.Value))
                : CreateValue<T, TError>(either.Value);
        }

        internal static async Task<IEither<T, TError>> IsErrorWhenAsync<T, TError>(Task<IEither<T, TError>> either,
            Func<T, bool> predicate,
            Func<T, TError> errorSelector) => IsErrorWhen(await either.ConfigureAwait(false), predicate, errorSelector);

        internal static async Task<IEither<T, TError>> IsErrorWhenAsyncPredicate<T, TError>(
            Task<IEither<T, TError>> source,
            Func<T, Task<bool>> predicate, Func<T, TError> errorSelector) {
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            var either = await source.ConfigureAwait(false);
            if (either.HasError)
                return either;

            return await predicate(either.Value).ConfigureAwait(false)
                ? CreateError<T, TError>(errorSelector(either.Value))
                : CreateValue<T, TError>(either.Value);
        }

        internal static IEither<TResult, TError> Join<T, TInner, TKey, TResult, TError>(IEither<T, TError> either,
            IEither<TInner, TError> inner, Func<T, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector,
            Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) => Join(either, inner,
            outerKeySelector,
            innerKeySelector, resultSelector, errorSelector,
            EqualityComparer<TKey>.Default);

        internal static IEither<TResult, TError> Join<T, TInner, TKey, TResult, TError>(IEither<T, TError> either,
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

        internal static async Task<IEither<TResult, TError>> JoinAsync<T, TInner, TKey, TResult, TError>(
            Task<IEither<T, TError>> either,
            Task<IEither<TInner, TError>> innerEither, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector, Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer) => Join(await either.ConfigureAwait(false),
            await innerEither.ConfigureAwait(false), outerKeySelector, innerKeySelector, resultSelector, errorSelector,
            comparer);

        internal static async Task<IEither<TResult, TError>> JoinAsync<T, TInner, TKey, TResult, TError>(
            Task<IEither<T, TError>> either,
            Task<IEither<TInner, TError>> innerEither, Func<T, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector, Func<T, TInner, TResult> resultSelector, Func<TError> errorSelector) =>
            Join(await either.ConfigureAwait(false), await innerEither.ConfigureAwait(false), outerKeySelector,
                innerKeySelector, resultSelector, errorSelector);

        [Pure]
        internal static IEither<TResult, TError> Map<T, TResult, TError>(IEither<T, TError> either,
            Func<T, TResult> selector) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return either.HasValue
                ? CreateValue<TResult, TError>(selector(either.Value))
                : CreateError<TResult, TError>(either.Error);
        }

        internal static async Task<IEither<TResult, TError>> MapAsync<T, TResult, TError>(
            Task<IEither<T, TError>> source,
            Func<T, TResult> selector) => Map(await source.ConfigureAwait(false), selector);

        [Pure]
        internal static async Task<IEither<TResult, TError>> MapAsyncSelector<T, TResult, TError>(
            Task<IEither<T, TError>> source,
            Func<T, Task<TResult>> selector) {
            var either = await MapAsync(source, selector).ConfigureAwait(false);

            return either.HasError
                ? CreateError<TResult, TError>(either.Error)
                : CreateValue<TResult, TError>(await either.Value.ConfigureAwait(false));
        }

        [Pure]
        internal static IEither<T, TErrorResult> MapError<T, TError, TErrorResult>(
            IEither<T, TError> either,
            Func<TError, TErrorResult> selector
        ) {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            return either.HasError
                ? CreateError<T, TErrorResult>(selector(either.Error))
                : CreateValue<T, TErrorResult>(either.Value);
        }

        internal static async Task<IEither<T, TErrorResult>> MapErrorAsync<T, TError, TErrorResult>(
            Task<IEither<T, TError>> either,
            Func<TError, TErrorResult> selector) => MapError(await either.ConfigureAwait(false), selector);

        internal static async Task<IEither<T, TErrorResult>> MapErrorAsyncSelector<T, TError, TErrorResult>(
            Task<IEither<T, TError>> source,
            Func<TError, Task<TErrorResult>> selector
        ) {
            var either = await MapErrorAsync(source, selector).ConfigureAwait(false);

            return either.HasError
                ? CreateError<T, TErrorResult>(await either.Error.ConfigureAwait(false))
                : CreateValue<T, TErrorResult>(either.Value);
        }

        [Pure]
        internal static TResult Match<T, TError, TResult>(IEither<T, TError> either,
            Func<T, TResult> selector, Func<TError, TResult> errorSelector) {
            if (either.HasError)
                return errorSelector != null
                    ? errorSelector(either.Error)
                    : throw new ArgumentNullException(nameof(errorSelector));

            return selector != null
                ? selector(either.Value)
                : throw new ArgumentNullException(nameof(selector));
        }

        internal static void Match<T, TError>(IEither<T, TError> either, Action<T> action, Action<TError> errorAction) {
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

        internal static async Task<TResult> MatchAsync<T, TResult, TError>(Task<IEither<T, TError>> taskResult,
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) =>
            Match(await taskResult.ConfigureAwait(false), selector, errorSelector);

        internal static async Task MatchAsync<T, TError>(Task<IEither<T, TError>> taskResult, Action<T> selector,
            Action<TError> errorSelector) => Match(await taskResult.ConfigureAwait(false), selector, errorSelector);

        internal static IEither<T, IReadOnlyList<TError>> Multiple<T, TError>(IEither<T, TError> either,
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

        internal static async Task<IEither<T, IReadOnlyList<TError>>> MultipleAsync<T, TError>(
            Task<IEither<T, TError>> source,
            IEnumerable<Task<IEither<T, TError>>> validations) => Multiple(await source.ConfigureAwait(false),
            await Task.WhenAll(validations).ConfigureAwait(false));

        [Pure]
        internal static IEither<TResult, TError> SafeCast<T, TResult, TError>(IEither<T, TError> either,
            Func<T, TError> errorSelector) {
            if (errorSelector == null) throw new ArgumentNullException(nameof(errorSelector));
            if (either.HasError) return CreateError<TResult, TError>(either.Error);

            return either.Value is TResult result
                ? CreateValue<TResult, TError>(result)
                : CreateError<TResult, TError>(errorSelector(either.Value));
        }

        internal static async Task<IEither<TResult, TError>> SafeCastAsync<T, TResult, TError>(
            Task<IEither<T, TError>> either,
            Func<T, TError> errorSelector) =>
            SafeCast<T, TResult, TError>(await either.ConfigureAwait(false), errorSelector);

        internal static IEnumerable<T2> YieldErrors<T1, T2>(IEither<T1, T2> result) {
            if (result.HasError)
                yield return result.Error;
        }

        internal static IEnumerable<T1> YieldValues<T1, T2>(IEither<T1, T2> result) {
            if (result.HasValue)
                yield return result.Value;
        }

        [Pure]
        internal static IEither<TResult, TError> Zip<T, TOther, TResult, TError>(IEither<T, TError> either,
            IEither<TOther, TError> other,
            Func<T, TOther, TResult> resultSelector) {
            if (resultSelector == null) throw new ArgumentNullException(nameof(resultSelector));
            if (either.HasError) return CreateError<TResult, TError>(either.Error);
            return other.HasError
                ? CreateError<TResult, TError>(other.Error)
                : CreateValue<TResult, TError>(resultSelector(either.Value, other.Value));
        }

        internal static async Task<IEither<TResult, TError>> ZipAsync<T, TResult, TOther, TError>(
            Task<IEither<T, TError>> either,
            Task<IEither<TOther, TError>> otherEither, Func<T, TOther, TResult> resultSelector) => Zip(
            await either.ConfigureAwait(false), await otherEither.ConfigureAwait(false), resultSelector);
    }
}