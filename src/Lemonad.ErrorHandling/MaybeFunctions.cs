using System;
using System.Diagnostics.Contracts;

namespace Lemonad.ErrorHandling {
    public static class Maybe {
        [Pure]
        internal static Maybe<TSource> None<TSource>() => Maybe<TSource>.Identity;

        [Pure]
        public static Maybe<TSource> NoneWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).SomeWhen(x => !predicate(x))
                : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TSource> Some<TSource>(this TSource item) => item == null
            ? new Maybe<TSource>(default(TSource), false)
            : new Maybe<TSource>(item, true);

        [Pure]
        public static Maybe<TSource> SomeWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null ? Some(item).SomeWhen(predicate) : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<string> NoneWhenStringIsNullOrEmpty(this string item) =>
            Some(item).SomeWhen(x => !string.IsNullOrEmpty(x));

        [Pure]
        public static Maybe<string> NoneWhenStringIsNullOrEmpty(this Maybe<string> item) =>
            item.SomeWhen(x => !string.IsNullOrEmpty(x));

        [Pure]
        public static Maybe<TSource> ConvertToMaybe<TSource>(this TSource? item) where TSource : struct =>
            item.HasValue ? Some(item.Value) : None<TSource>();

        [Pure]
        public static Maybe<TSource> SomeWhen<TSource>(this Maybe<TSource> source,
            Func<TSource, bool> predicate) => predicate != null
            ? source.HasValue
                ? predicate(source.Value)
                    ? Some(source.Value)
                    : None<TSource>()
                : None<TSource>()
            : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TResult>
            Map<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selector) =>
            source.HasValue ? Some(selector(source.Value)) : None<TResult>();

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, Maybe<TResult>> selector) {
            if (!source.HasValue) return None<TResult>();
            var result = selector(source.Value);
            return result.HasValue ? Some(result.Value) : None<TResult>();
        }

        [Pure]
        public static TResult Match<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> someSelector,
            Func<TResult> noneSelector) => source.HasValue ? someSelector(source.Value) : noneSelector();

        public static void Match<TSource>(this Maybe<TSource> source, Action<TSource> someSelector,
            Action noneSelector) {
            if (source.HasValue) someSelector(source.Value);
            else noneSelector();
        }

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
            Maybe<TResult> nullable) => source.FlatMap(_ => nullable);

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(this Maybe<TSource> source,
            Maybe<TSelector> nullable, Func<TSource, TSelector, TResult> resultSelector) =>
            source.FlatMap(_ => nullable, resultSelector);

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TSelector>> collectionSelector,
            Func<TSource, TSelector, TResult> resultSelector) => source.FlatMap(src =>
            collectionSelector(src).Map(elem => resultSelector(src, elem)));

        [Pure]
        public static Maybe<TResult> FlatMap<TSource, TSelector, TResult>(
            this Maybe<TSource> source,
            Func<TSource, TSelector?> collectionSelector,
            Func<TSource, TSelector, TResult> resultSelector) where TSelector : struct => source.FlatMap(
            src => collectionSelector(src).ConvertToMaybe().Map(elem => resultSelector(src, elem)));
    }
}