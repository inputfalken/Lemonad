using System;

namespace Lemonad.ErrorHandling {
    public static class Maybe {
        public static Maybe<TSource> Some<TSource>(this TSource item) => new Maybe<TSource>(item, true);

        public static Maybe<TSource> None<TSource>() => new Maybe<TSource>(default(TSource), false);
        public static Maybe<TSource> None<TSource>(this TSource item) => new Maybe<TSource>(item, false);

        public static Maybe<TSource> SomeWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            Some(item).Filter(predicate);

        public static Maybe<TSource> NoneWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            SomeWhen(item, x => !predicate(x));

        public static Maybe<TSource> ConvertToMaybe<TSource>(this TSource? item) where TSource : struct =>
            item.HasValue ? Some(item.Value) : None<TSource>();

        public static Maybe<TSource> Filter<TSource>(this Maybe<TSource> source,
            Func<TSource, bool> predicate) => source.HasValue
            ? predicate(source.Value)
                ? Some(source.Value)
                : None<TSource>()
            : None<TSource>();

        public static Maybe<TResult>
            Map<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> selector) =>
            source.HasValue ? Some(selector(source.Value)) : None<TResult>();

        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, Maybe<TResult>> selector) {
            if (!source.HasValue) return None<TResult>();
            var result = selector(source.Value);
            return result.HasValue ? Some(result.Value) : None<TResult>();
        }

        public static TResult Match<TSource, TResult>(this Maybe<TSource> source, Func<TSource, TResult> someSelector,
            Func<TResult> noneSelector) => source.HasValue ? someSelector(source.Value) : noneSelector();
        
        public static void Match<TSource >(this Maybe<TSource> source, Action<TSource> someSelector,
            Action noneSelector) {
            if (source.HasValue) someSelector(source.Value);
            else noneSelector();
        }

        public static Maybe<TResult> FlatMap<TSource, TResult>(this Maybe<TSource> source,
            Func<TSource, TResult?> selector) where TResult : struct =>
            source.HasValue ? selector(source.Value).ConvertToMaybe() : None<TResult>();

        public static Maybe<TResultSelector> FlatMap<TSource, TSelector, TResultSelector>(
            this Maybe<TSource> source,
            Func<TSource, Maybe<TSelector>> collectionSelector,
            Func<TSource, TSelector, TResultSelector> resultSelector) => source.FlatMap(src =>
            collectionSelector(src).Map(elem => resultSelector(src, elem)));

        public static Maybe<TResultSelector> FlatMap<TSource, TSelector, TResultSelector>(
            this Maybe<TSource> source,
            Func<TSource, TSelector?> collectionSelector,
            Func<TSource, TSelector, TResultSelector> resultSelector) where TSelector : struct => source.FlatMap(
            src => collectionSelector(src).ConvertToMaybe().Map(elem => resultSelector(src, elem)));
    }
}