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
                    using (var e = source.GetEnumerator())
                        return e.MoveNext() ? e.Current : Maybe<TSource>.None;
            }
        }

        public static Maybe<TSource> FirstMaybe<TSource>(this IEnumerable<TSource> source,
            Func<TSource, bool> predicate) {
            foreach (var element in source)
                if (predicate(element))
                    return element;

            return Maybe<TSource>.None;
        }

        public static IEnumerable<TSource> GetMaybeValues<TSource>(this IEnumerable<Maybe<TSource>> source) =>
            source.SelectMany(x => x.Enumerable);

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

        public static class Parse {
            [Pure]
            private static string FormatMessage(string input) => $"Could not parse string '{input}'.";

            public static Maybe<TEnum> Enum<TEnum>(string input) where TEnum : struct =>
                System.Enum.TryParse<TEnum>(input, out var value)
                    ? value.Some()
                    : Maybe.None<TEnum>();

            public static Maybe<TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
                System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                    ? value.Some()
                    : Maybe.None<TEnum>();

            public static Maybe<decimal> Decimal(string input, NumberStyles style, IFormatProvider provider) =>
                decimal.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : Maybe.None<decimal>();

            public static Maybe<decimal> Decimal(string input) =>
                decimal.TryParse(input, out var number)
                    ? number.Some()
                    : Maybe.None<decimal>();

            public static Maybe<int> Int(string input, NumberStyles style, IFormatProvider provider) =>
                int.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : Maybe.None<int>();

            public static Maybe<long> Long(string input, NumberStyles style, IFormatProvider provider) =>
                long.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : Maybe.None<long>();

            public static Maybe<long> Long(string input) =>
                long.TryParse(input, out var number)
                    ? number.Some()
                    : Maybe.None<long>();

            public static Maybe<int> Int(string input) => int.TryParse(input, out var number)
                ? number.Some()
                : Maybe.None<int>();

            public static Maybe<double> Double(string input, NumberStyles style, IFormatProvider provider) =>
                double.TryParse(input, style, provider, out var number)
                    ? number.Some()
                    : Maybe.None<double>();

            public static Maybe<double> Double(string input) => double.TryParse(input, out var number)
                ? number.Some()
                : Maybe.None<double>();

            public static Maybe<DateTime> DateTime(string input, DateTimeStyles style, IFormatProvider provider) =>
                System.DateTime.TryParse(input, provider, style, out var date)
                    ? date.Some()
                    : Maybe.None<DateTime>();

            public static Maybe<DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParseExact(input, format, provider, style, out var date)
                    ? date.Some()
                    : Maybe.None<DateTime>();

            public static Maybe<DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                    ? date.Some()
                    : Maybe.None<DateTime>();

            public static Maybe<DateTime> DateTime(string input) => System.DateTime.TryParse(input, out var date)
                ? date.Some()
                : Maybe.None<DateTime>();

            public static Maybe<bool> Bool(string input) => bool.TryParse(input, out var boolean)
                ? boolean.Some()
                : Maybe.None<bool>();
        }
    }
}