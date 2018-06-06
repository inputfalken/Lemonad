using System;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public static class Maybe {
        public static void Match<TSource>(this Maybe<TSource> source, Action<TSource> someAction,
            Action noneAction) {
            if (source.HasValue) {
                if (someAction == null)
                    throw new ArgumentNullException(nameof(someAction));
                someAction(source.Value);
            }
            else {
                if (noneAction == null)
                    throw new ArgumentNullException(nameof(noneAction));
                noneAction();
            }
        }

        [Pure]
        public static Maybe<TSource> None<TSource>() => Maybe<TSource>.Identity;

        [Pure]
        public static Maybe<TSource> None<TSource>(this TSource item) => new Maybe<TSource>(item, false);

        [Pure]
        public static Maybe<TSource> NoneWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).SomeWhen(x => !predicate(x))
                : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TSource> NoneWhen<TSource>(this Maybe<TSource> source,
            Func<TSource, bool> predicate) => source.HasValue
            ? predicate != null
                ? predicate(source.Value) == false
                    ? source
                    : Maybe<TSource>.Identity
                : throw new ArgumentNullException(nameof(predicate))
            : Maybe<TSource>.Identity;

        [Pure]
        public static Maybe<TSource> Some<TSource>(this TSource item) => new Maybe<TSource>(item, true);

        [Pure]
        public static Maybe<TSource> NoneWhenNull<TSource>(this TSource item) =>
            item.NoneWhen(EquailtyFunctions.IsNull);

        [Pure]
        public static Maybe<TSource> NoneWhenNull<TSource>(this Maybe<TSource> item) =>
            item.NoneWhen<TSource>(EquailtyFunctions.IsNull);

        [Pure]
        public static Maybe<TSource> SomeWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).SomeWhen(predicate)
                : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TSource> ConvertToMaybe<TSource>(this TSource? item) where TSource : struct =>
            item.HasValue ? Some(item.Value) : None<TSource>();

        [Pure]
        public static Maybe<TSource> SomeWhen<TSource>(this Maybe<TSource> source,
            Func<TSource, bool> predicate) => source.HasValue
            ? predicate != null
                ? predicate(source.Value)
                    ? Some(source.Value)
                    : Maybe<TSource>.Identity
                : throw new ArgumentNullException()
            : Maybe<TSource>.Identity;

        [Pure]
        public static Maybe<TResult>
            Map<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selector) =>
            source.HasValue
                ? selector != null
                    ? Some(selector(source.Value))
                    : throw new ArgumentNullException(nameof(selector))
                : Maybe<TResult>.Identity;

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, Maybe<TResult>> selector) => source.HasValue
            ? selector?.Invoke(source.Value) ?? throw new ArgumentNullException(nameof(selector))
            : Maybe<TResult>.Identity;

        [Pure]
        public static TResult Match<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> someSelector,
            Func<TResult> noneSelector) => someSelector == null
            ? throw new ArgumentNullException(nameof(someSelector))
            : (noneSelector == null
                ? throw new ArgumentNullException(nameof(noneSelector))
                : (source.HasValue ? someSelector(source.Value) : noneSelector()));

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, TResult?> selector) where TResult : struct =>
            source.HasValue ? selector(source.Value).ConvertToMaybe() : None<TResult>();

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            TResult? nullable) where TResult : struct => source.FlatMap(_ => nullable);

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(this Maybe<TSource> source,
            TSelector? nullable, Func<TSource, TSelector, TResult> resultSelector) where TSelector : struct =>
            source.FlatMap(_ => nullable, resultSelector);

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            Maybe<TResult> maybe) => source.FlatMap(_ => maybe);

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(this Maybe<TSource> source,
            Maybe<TSelector> maybe, Func<TSource, TSelector, TResult> resultSelector) =>
            source.FlatMap(_ => maybe, resultSelector);

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) {
            if (source.HasValue)
                return selector != null
                    ? source.FlatMap(x => selector(x).Map(y => resultSelector != null
                        ? resultSelector(x, y)
                        : throw new ArgumentNullException(nameof(resultSelector))))
                    : throw new ArgumentNullException(nameof(selector));

            return Maybe<TResult>.Identity;
        }

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(
            this Maybe<TSource> source,
            Func<TSource, TSelector?> selector,
            Func<TSource, TSelector, TResult> resultSelector) where TSelector : struct => source.FlatMap(
            src => selector(src).ConvertToMaybe().Map(elem => resultSelector(src, elem)));
    }
}