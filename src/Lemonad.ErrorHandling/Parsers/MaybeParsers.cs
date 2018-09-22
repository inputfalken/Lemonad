using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Lemonad.ErrorHandling.Parsers {
    public static class MaybeParsers {
        public static Maybe<bool> Bool(string input) => bool.TryParse(input, out var boolean)
            ? boolean.ToMaybe()
            : MaybeExtensions.ToMaybeNone<bool>();

        public static Maybe<DateTime> DateTime(string input, DateTimeStyles style, IFormatProvider provider) =>
            System.DateTime.TryParse(input, provider, style, out var date)
                ? date.ToMaybe()
                : MaybeExtensions.ToMaybeNone<DateTime>();

        public static Maybe<DateTime> DateTime(string input) => System.DateTime.TryParse(input, out var date)
            ? date.ToMaybe()
            : MaybeExtensions.ToMaybeNone<DateTime>();

        public static Maybe<DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, format, provider, style, out var date)
                ? date.ToMaybe()
                : MaybeExtensions.ToMaybeNone<DateTime>();

        public static Maybe<DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                ? date.ToMaybe()
                : MaybeExtensions.ToMaybeNone<DateTime>();

        public static Maybe<decimal> Decimal(string input, NumberStyles style, IFormatProvider provider) =>
            decimal.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : MaybeExtensions.ToMaybeNone<decimal>();

        public static Maybe<decimal> Decimal(string input) =>
            decimal.TryParse(input, out var number)
                ? number.ToMaybe()
                : MaybeExtensions.ToMaybeNone<decimal>();

        public static Maybe<double> Double(string input, NumberStyles style, IFormatProvider provider) =>
            double.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : MaybeExtensions.ToMaybeNone<double>();

        public static Maybe<double> Double(string input) => double.TryParse(input, out var number)
            ? number.ToMaybe()
            : MaybeExtensions.ToMaybeNone<double>();

        public static Maybe<TEnum> Enum<TEnum>(string input) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, out var value)
                ? value.ToMaybe()
                : MaybeExtensions.ToMaybeNone<TEnum>();

        public static Maybe<TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                ? value.ToMaybe()
                : MaybeExtensions.ToMaybeNone<TEnum>();

        [Pure]
        private static string FormatMessage(string input) => $"Could not parse string '{input}'.";

        public static Maybe<int> Int(string input, NumberStyles style, IFormatProvider provider) =>
            int.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : MaybeExtensions.ToMaybeNone<int>();

        public static Maybe<int> Int(string input) => int.TryParse(input, out var number)
            ? number.ToMaybe()
            : MaybeExtensions.ToMaybeNone<int>();

        public static Maybe<long> Long(string input, NumberStyles style, IFormatProvider provider) =>
            long.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : MaybeExtensions.ToMaybeNone<long>();

        public static Maybe<long> Long(string input) =>
            long.TryParse(input, out var number)
                ? number.ToMaybe()
                : MaybeExtensions.ToMaybeNone<long>();
    }
}