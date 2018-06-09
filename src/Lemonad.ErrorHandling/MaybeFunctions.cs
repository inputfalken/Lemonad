using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
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
        public static Maybe<TSource> NoneWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).SomeWhen(x => !predicate(x))
                : throw new ArgumentNullException(nameof(predicate));

        [Pure]
        public static Maybe<TSource> NoneWhenNull<TSource>(this TSource item) =>
            item.NoneWhen(EquailtyFunctions.IsNull);

        [Pure]
        public static Maybe<TSource> SomeWhen<TSource>(this TSource item, Func<TSource, bool> predicate) =>
            predicate != null
                ? Some(item).SomeWhen(predicate)
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

        public static class Parse {
            [Pure]
            private static string FormatMessage(string input) => $"Could not parse string '{input}'.";

            public static Maybe<TEnum> Enum<TEnum>(string input) where TEnum : struct =>
                System.Enum.TryParse<TEnum>(input, out var value)
                    ? value.Some()
                    : None<TEnum>();

            public static Maybe<TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
                System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                    ? value.Some()
                    : None<TEnum>();

            public static Maybe<decimal> Decimal(string input, NumberStyles style, IFormatProvider provider) =>
                decimal.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : None<decimal>();

            public static Maybe<decimal> Decimal(string input) =>
                decimal.TryParse(input, out var number)
                    ? number.Some()
                    : None<decimal>();

            public static Maybe<int> Int(string input, NumberStyles style, IFormatProvider provider) =>
                int.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : None<int>();

            public static Maybe<long> Long(string input, NumberStyles style, IFormatProvider provider) =>
                long.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : None<long>();

            public static Maybe<long> Long(string input) =>
                long.TryParse(input, out var number)
                    ? number.Some()
                    : None<long>();

            public static Maybe<int> Int(string input) => int.TryParse(input, out var number)
                ? number.Some()
                : None<int>();

            public static Maybe<double> Double(string input, NumberStyles style, IFormatProvider provider) =>
                double.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : None<double>();

            public static Maybe<double> Double(string input) => double.TryParse(input, out var number)
                ? number.Some()
                : None<double>();

            public static Maybe<DateTime> DateTime(string input, DateTimeStyles style, IFormatProvider provider) =>
                System.DateTime.TryParse(input, provider, style, out var date)
                    ? date.Some()
                    : None<DateTime>();

            public static Maybe<DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParseExact(input, format, provider, style, out var date)
                    ? date.Some()
                    : None<DateTime>();

            public static Maybe<DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                    ? date.Some()
                    : None<DateTime>();

            public static Maybe<DateTime> DateTime(string input) => System.DateTime.TryParse(input, out var date)
                ? date.Some()
                : None<DateTime>();

            public static Maybe<bool> Bool(string input) => bool.TryParse(input, out var boolean)
                ? boolean.Some()
                : None<bool>();
        }
    }
}