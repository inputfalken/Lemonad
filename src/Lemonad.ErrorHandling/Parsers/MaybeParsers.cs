using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Lemonad.ErrorHandling.Parsers {
    public static class MaybeParsers {
        public static IMaybe<bool> Bool(string input) => bool.TryParse(input, out var boolean)
            ? boolean.ToMaybe()
            : Maybe.ToMaybeNone<bool>();

        public static IMaybe<DateTime> DateTime(string input, DateTimeStyles style, IFormatProvider provider) =>
            System.DateTime.TryParse(input, provider, style, out var date)
                ? date.ToMaybe()
                : Maybe.ToMaybeNone<DateTime>();

        public static IMaybe<DateTime> DateTime(string input) => System.DateTime.TryParse(input, out var date)
            ? date.ToMaybe()
            : Maybe.ToMaybeNone<DateTime>();

        public static IMaybe<DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, format, provider, style, out var date)
                ? date.ToMaybe()
                : Maybe.ToMaybeNone<DateTime>();

        public static IMaybe<DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                ? date.ToMaybe()
                : Maybe.ToMaybeNone<DateTime>();

        public static IMaybe<decimal> Decimal(string input, NumberStyles style, IFormatProvider provider) =>
            decimal.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : Maybe.ToMaybeNone<decimal>();

        public static IMaybe<decimal> Decimal(string input) =>
            decimal.TryParse(input, out var number)
                ? number.ToMaybe()
                : Maybe.ToMaybeNone<decimal>();

        public static IMaybe<double> Double(string input, NumberStyles style, IFormatProvider provider) =>
            double.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : Maybe.ToMaybeNone<double>();

        public static IMaybe<double> Double(string input) => double.TryParse(input, out var number)
            ? number.ToMaybe()
            : Maybe.ToMaybeNone<double>();

        public static IMaybe<TEnum> Enum<TEnum>(string input) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, out var value)
                ? value.ToMaybe()
                : Maybe.ToMaybeNone<TEnum>();

        public static IMaybe<TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                ? value.ToMaybe()
                : Maybe.ToMaybeNone<TEnum>();

        [Pure]
        private static string FormatMessage(string input) => $"Could not parse string '{input}'.";

        public static IMaybe<int> Int(string input, NumberStyles style, IFormatProvider provider) =>
            int.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : Maybe.ToMaybeNone<int>();

        public static IMaybe<int> Int(string input) => int.TryParse(input, out var number)
            ? number.ToMaybe()
            : Maybe.ToMaybeNone<int>();

        public static IMaybe<long> Long(string input, NumberStyles style, IFormatProvider provider) =>
            long.TryParse(input, style, provider, out var number)
                ? number.ToMaybe()
                : Maybe.ToMaybeNone<long>();

        public static IMaybe<long> Long(string input) =>
            long.TryParse(input, out var number)
                ? number.ToMaybe()
                : Maybe.ToMaybeNone<long>();
    }
}