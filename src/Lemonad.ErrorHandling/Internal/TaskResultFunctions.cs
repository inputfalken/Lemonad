using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace Lemonad.ErrorHandling.Internal {
    // The methods needs to stay internal; so we do not expose extensions to Task<Result<T, TError>>.
    // Since the result will be ambigous method signatures when combined with extensions of Task<T>.
    // The API consumer will have to convert their task types into outcome.
    internal static class TaskResultFunctions {
        [Pure]
        internal static async Task<IEnumerable<T>> AsEnumerable<T, TError>(Task<Result<T, TError>> result) =>
            (await result.ConfigureAwait(false)).ToEnumerable();

        [Pure]
        internal static async Task<IEnumerable<TError>> AsErrorEnumerable<T, TError>(Task<Result<T, TError>> result) =>
            (await result.ConfigureAwait(false)).ToErrorEnumerable();

        [Pure]
        internal static async Task<Result<TResult, TError>> Cast<T, TResult, TError>(Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).Cast<TResult>();

        [Pure]
        internal static async Task<Result<T, TErrorResult>> CastError<T, TError, TErrorResult>
            (Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).CastError<TErrorResult>();

        internal static async Task<Result<T, TError>> Do<T, TError>(Task<Result<T, TError>> source, Action action) =>
            (await source.ConfigureAwait(false)).Do(action);

        internal static async Task<Result<T, TError>> DoWith<T, TError>(Task<Result<T, TError>> source,
            Action<T> action) =>
            (await source.ConfigureAwait(false)).DoWith(action);

        internal static async Task<Result<T, TError>> DoWithError<T, TError>(Task<Result<T, TError>> source,
            Action<TError> action) => (await source.ConfigureAwait(false)).DoWithError(action);

        [Pure]
        internal static async Task<Either<T, TError>> Either<T, TError>(Task<Result<T, TError>> result) =>
            (await result.ConfigureAwait(false)).Either;

        [Pure]
        internal static async Task<Result<T, TError>> Filter<T, TError>(
            Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Filter(predicate, errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> Filter<T, TError>(Task<Result<T, TError>> source,
            Func<T, Task<bool>> predicate,
            Func<Maybe<T>, TError> errorSelector) {
            var result = await source.ConfigureAwait(false);
            if (result.Either.HasError) return ResultExtensions.Error<T, TError>(result.Either.Error);
            if (await predicate(result.Either.Value)) return ResultExtensions.Value<T, TError>(result.Either.Value);

            return ResultExtensions.Error<T, TError>(errorSelector(result.Either.Value));
        }

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> flatSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> flatSelector) =>
            await FlatMap(await source.ConfigureAwait(false), flatSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TSelector, TResult, TError>(
            Task<Result<T, TError>> source,
            Func<T, Result<TSelector, TError>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatSelector, resultSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TSelector, TResult, TError>(
            Task<Result<T, TError>> source,
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) =>
            await FlatMap(await source.ConfigureAwait(false), flatSelector, resultSelector)
                .ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TResult, TError, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector, Func<TErrorResult, TError> errorSelector) =>
            await FlatMap(await source.ConfigureAwait(false), flatMapSelector, errorSelector)
                .ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).FlatMap(flatMapSelector, resultSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<T, TError, TFlatMap, TResult, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            await FlatMap(await source.ConfigureAwait(false), flatMapSelector, resultSelector, errorSelector)
                .ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<TResult, TError, T>(Result<T, TError> result,
            Func<T, Task<Result<TResult, TError>>> flatSelector) => result.Either.HasValue
            ? await flatSelector(result.Either.Value).ConfigureAwait(false)
            : ResultExtensions.Error<TResult, TError>(result.Either.Error);

        [Pure]
        internal static Task<Result<TResult, TError>> FlatMap<TSelector, TResult, TError, T>(Result<T, TError> result,
            Func<T, Task<Result<TSelector, TError>>> flatSelector,
            Func<T, TSelector, TResult> resultSelector) => FlatMap(result, x =>
            Map(flatSelector?.Invoke(x), y => resultSelector == null
                ? throw new ArgumentNullException(nameof(resultSelector))
                : resultSelector(x, y)));

        [Pure]
        internal static async Task<Result<TResult, TError>> FlatMap<TResult, TErrorResult, TError, T>(
            Result<T, TError> result, Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector,
            Func<TErrorResult, TError> errorSelector) {
            if (result.Either.HasError) return ResultExtensions.Error<TResult, TError>(result.Either.Error);
            if (flatMapSelector == null) throw new ArgumentNullException(nameof(flatMapSelector));
            var okSelector = await flatMapSelector(result.Either.Value);

            return okSelector.Either.HasValue
                ? ResultExtensions.Value<TResult, TError>(okSelector.Either.Value)
                : okSelector.MapError(errorSelector);
        }

        [Pure]
        internal static Task<Result<TResult, TError>> FlatMap<TFlatMap, TResult, TErrorResult, TError, T>(
            Result<T, TError> result, Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TErrorResult, TError> errorSelector) =>
            FlatMap(result, x => Map(flatMapSelector?.Invoke(x), y => {
                    if (resultSelector == null)
                        throw new ArgumentNullException(nameof(resultSelector));
                    return resultSelector(x, y);
                }),
                errorSelector);

        /// <summary>
        ///     Asynchronous version of Flatten.
        /// </summary>
        [Pure]
        internal static async Task<Result<T, TError>> Flatten<TResult, T, TError>(Result<T, TError> result,
            Func<T, Task<Result<TResult, TError>>> selector) {
            if (result.Either.HasError) return ResultExtensions.Error<T, TError>(result.Either.Error);
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            var okSelector = await selector(result.Either.Value).ConfigureAwait(false);
            return okSelector.Either.HasValue
                ? ResultExtensions.Value<T, TError>(result.Either.Value)
                : ResultExtensions.Error<T, TError>(okSelector.Either.Error);
        }

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(
            Task<Result<T, TError>> source, Func<T, Result<TResult, TErrorResult>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).Flatten(selector, errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TError, TResult>(Task<Result<T, TError>> source,
            Func<T, Result<TResult, TError>> selector) => (await source.ConfigureAwait(false)).Flatten(selector);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TResult, TError, TErrorResult>(
            Task<Result<T, TError>> source, Func<T, Task<Result<TResult, TErrorResult>>> selector,
            Func<TErrorResult, TError> errorSelector) =>
            await Flatten(await source.ConfigureAwait(false), selector, errorSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<T, TError>> Flatten<T, TError, TResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TError>>> selector) =>
            await Flatten(await source.ConfigureAwait(false), selector).ConfigureAwait(false);

        /// <summary>
        ///     Asynchronous version of Flatten.
        /// </summary>
        [Pure]
        internal static async Task<Result<T, TError>> Flatten<TResult, TErrorResult, T, TError>(
            Result<T, TError> result, Func<T, Task<Result<TResult, TErrorResult>>> selector,
            Func<TErrorResult, TError> errorSelector) {
            if (result.Either.HasValue) {
                if (selector == null) throw new ArgumentNullException(nameof(selector));
                var okSelector = await selector(result.Either.Value).ConfigureAwait(false);
                if (okSelector.Either.HasValue)
                    return ResultExtensions.Value<T, TError>(result.Either.Value);
                var tmpThis = result;
                return okSelector.FullMap(x => tmpThis.Either.Value, errorSelector);
            }

            return ResultExtensions.Error<T, TError>(result.Either.Error);
        }

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullCast<T, TResult, TError, TErrorResult>(
            Task<Result<T, TError>> source) =>
            (await source.ConfigureAwait(false)).FullCast<TResult, TErrorResult>();

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TResult, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) =>
            await FullFlatMap(await source.ConfigureAwait(false), flatMapSelector, errorSelector)
                .ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TFlatMap, TResult,
            TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) => await FullFlatMap(await source.ConfigureAwait(false),
            flatMapSelector, resultSelector, errorSelector).ConfigureAwait(false);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TResult, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, Result<TResult, TErrorResult>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) => (await source.ConfigureAwait(false))
            .FullFlatMap(flatMapSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<T, TError, TFlatMap, TResult,
            TErrorResult>(Task<Result<T, TError>> source,
            Func<T, Result<TFlatMap, TErrorResult>> flatMapSelector, Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            (await source.ConfigureAwait(false)).FullFlatMap(flatMapSelector, resultSelector, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullFlatMap<TResult, TErrorResult, T, TError>(
            Result<T, TError> result, Func<T, Task<Result<TResult, TErrorResult>>> flatMapSelector,
            Func<TError, TErrorResult> errorSelector) {
            if (result.Either.HasValue) return await flatMapSelector(result.Either.Value).ConfigureAwait(false);

            return errorSelector != null
                ? ResultExtensions.Error<TResult, TErrorResult>(errorSelector(result.Either.Error))
                : throw new ArgumentNullException(nameof(errorSelector));
        }

        [Pure]
        internal static Task<Result<TResult, TErrorResult>> FullFlatMap<TFlatMap, TResult, TErrorResult, T, TError>(
            Result<T, TError> result, Func<T, Task<Result<TFlatMap, TErrorResult>>> flatMapSelector,
            Func<T, TFlatMap, TResult> resultSelector,
            Func<TError, TErrorResult> errorSelector) =>
            FullFlatMap(result, x => Map(flatMapSelector?.Invoke(x), y => {
                    if (resultSelector == null)
                        throw new ArgumentNullException(nameof(resultSelector));
                    return resultSelector(x, y);
                }),
                errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TErrorResult>> FullMap<T, TResult, TError, TErrorResult>(
            Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TErrorResult> errorSelector
        ) => (await source.ConfigureAwait(false)).FullMap(selector, errorSelector);

        [Pure]
        internal static async Task<Result<T, TError>> IsErrorWhen<T, TError>(Task<Result<T, TError>> source,
            Func<T, Task<bool>> predicate,
            Func<Maybe<T>, TError> errorSelector) {
            var result = await source.ConfigureAwait(false);
            if (result.Either.HasError) return ResultExtensions.Error<T, TError>(result.Either.Error);
            return await predicate(result.Either.Value)
                ? ResultExtensions.Error<T, TError>(errorSelector(result.Either.Value))
                : ResultExtensions.Value<T, TError>(result.Either.Value);
        }

        [Pure]
        internal static async Task<Result<T, TError>> IsErrorWhen<T, TError>(Task<Result<T, TError>> source,
            Func<T, bool> predicate,
            Func<Maybe<T>, TError> errorSelector) =>
            (await source.ConfigureAwait(false)).IsErrorWhen(predicate, errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> Join<TOuter, TError, TInner, TKey, TResult>(
            Task<Result<TOuter, TError>> outer,
            Task<Result<TInner, TError>> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            Func<TError> errorSelector
        ) => (await outer.ConfigureAwait(false))
            .Join(
                await inner.ConfigureAwait(false),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector
            );

        [Pure]
        internal static async Task<Result<TResult, TError>> Join<TOuter, TError, TInner, TKey, TResult>(
            Task<Result<TOuter, TError>> outer,
            Task<Result<TInner, TError>> inner,
            Func<TOuter, TKey> outerKeySelector,
            Func<TInner, TKey> innerKeySelector,
            Func<TOuter, TInner, TResult> resultSelector,
            Func<TError> errorSelector,
            IEqualityComparer<TKey> comparer) => (await outer.ConfigureAwait(false))
            .Join(
                await inner.ConfigureAwait(false),
                outerKeySelector,
                innerKeySelector,
                resultSelector,
                errorSelector,
                comparer
            );

        [Pure]
        internal static async Task<Result<TResult, TError>> Map<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, TResult> selector) => (await source.ConfigureAwait(false)).Map(selector);

        [Pure]
        internal static async Task<Result<TResult, TError>> Map<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, Task<TResult>> selector) {
            var result = await source.ConfigureAwait(false);
            if (result.Either.HasError) return ResultExtensions.Error<TResult, TError>(result.Either.Error);
            if (selector != null) return ResultExtensions.Value<TResult, TError>(await selector(result.Either.Value));
            throw new ArgumentNullException(nameof(selector));
        }

        [Pure]
        internal static async Task<Result<T, TResult>> MapError<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<TError, Task<TResult>> selector) {
            var result = await source.ConfigureAwait(false);
            if (result.Either.HasValue) return ResultExtensions.Value<T, TResult>(result.Either.Value);

            if (selector != null) return ResultExtensions.Error<T, TResult>(await selector(result.Either.Error));
            throw new ArgumentNullException(nameof(selector));
        }

        [Pure]
        internal static async Task<Result<T, TErrorResult>> MapError<T, TError, TErrorResult>(
            Task<Result<T, TError>> source, Func<TError, TErrorResult> selector) =>
            (await source.ConfigureAwait(false)).MapError(selector);

        [Pure]
        internal static async Task<TResult> Match<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<T, TResult> selector,
            Func<TError, TResult> errorSelector) =>
            (await source.ConfigureAwait(false)).Match(selector, errorSelector);

        internal static async Task Match<T, TError>(Task<Result<T, TError>> source,
            Action<T> action,
            Action<TError> errorAction) =>
            (await source.ConfigureAwait(false)).Match(action, errorAction);

        [Pure]
        internal static async Task<Result<T, IReadOnlyList<TError>>> Multiple<T, TError>(
            Task<Result<T, TError>> source, params Func<Result<T, TError>, Result<T, TError>>[] validations
        ) => (await source.ConfigureAwait(false)).Multiple(validations);

        [Pure]
        internal static async Task<Result<TResult, TError>> SafeCast<T, TResult, TError>(Task<Result<T, TError>> source,
            Func<TError> errorSelector)
            => (await source.ConfigureAwait(false)).SafeCast<TResult>(errorSelector);

        [Pure]
        internal static async Task<Result<TResult, TError>> Zip<T, TError, TOther, TResult>(
            Task<Result<T, TError>> source,
            Task<Result<TOther, TError>> other,
            Func<T, TOther, TResult> resultSelector) => await (await source.ConfigureAwait(false))
            .Zip(await other.ConfigureAwait(false), resultSelector).ToAsyncResult().TaskResult.ConfigureAwait(false);
    }
}