using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public static class Maybe {
        [Pure]
        public static Maybe<TSource> Some<TSource>(this TSource item) => new Maybe<TSource>(item, true);

        [Pure]
        public static Maybe<TSource> None<TSource>() => Maybe<TSource>.None;

        [Pure]
        public static Maybe<TSource> None<TSource>(this TSource item) => new Maybe<TSource>(item, false);

        public static Maybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source) {
            switch (source) {
                case IList<TSource> list when list.Count > 0:
                    return list[0];
                case IReadOnlyList<TSource> readOnlyList when readOnlyList.Count > 0:
                    return readOnlyList[0];
                default:
                    using (var e = source.GetEnumerator()) {
                        return e.MoveNext() ? e.Current : Maybe<TSource>.None;
                    }
            }
        }

        public static Maybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate) {
            foreach (var element in source)
                if (predicate(element))
                    return element;

            return Maybe<TSource>.None;
        }

        public static IEnumerable<TSource> MaybeSome<TSource>(this IEnumerable<Maybe<TSource>> source) =>
            source.SelectMany(x => x.Enumerable);

        public static IEnumerable<TResult> MaybeMatch<TSource, TResult>(this IEnumerable<Maybe<TSource>> source,
            Func<TSource, TResult> someSelector, Func<TResult> noneSelector) =>
            source.Select(x => x.Match(someSelector, noneSelector));

        public static IEnumerable<TResult> MaybeNone<TSource, TResult>(this IEnumerable<Maybe<TSource>> source,
            Func<TResult> selector) => source.Where(x => x.HasValue == false).Select(_ => selector());

        [Pure]
        public static Maybe<TSource> None<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).IsSomeWhen(x => !predicate(x))
                : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TSource> Some<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).IsSomeWhen(predicate)
                : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TSource> ConvertToMaybe<TSource>(this TSource? item) where TSource : struct =>
            item.HasValue ? Some(item.Value) : None<TSource>();

        [Pure]
        public static Either<TLeft, TRight>
            ToEither<TLeft, TRight>(this Maybe<TRight> source, Func<TLeft> leftSelector) =>
            source.HasValue
                ? Either.Right<TLeft, TRight>(source.Value)
                : (leftSelector != null
                    ? Either.Left<TLeft, TRight>(leftSelector())
                    : throw new ArgumentNullException(nameof(leftSelector)));
    }
}