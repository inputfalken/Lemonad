using System;
using System.Globalization;

namespace Lemonad.ErrorHandling.Parsers {
    public static class MaybeParsers {
        public static IMaybe<bool> Bool(string input) => bool.TryParse(input, out var boolean)
            ? Maybe.Value(boolean)
            : Maybe.None<bool>();

        public static IMaybe<DateTime> DateTime(string input, DateTimeStyles style, IFormatProvider provider) =>
            System.DateTime.TryParse(input, provider, style, out var date)
                ? Maybe.Value(date)
                : Maybe.None<DateTime>();

        public static IMaybe<DateTime> DateTime(string input) => System.DateTime.TryParse(input, out var date)
            ? Maybe.Value(date)
            : Maybe.None<DateTime>();

        public static IMaybe<DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, format, provider, style, out var date)
                ? Maybe.Value(date)
                : Maybe.None<DateTime>();

        public static IMaybe<DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                ? Maybe.Value(date)
                : Maybe.None<DateTime>();

        public static IMaybe<decimal> Decimal(string input, NumberStyles style, IFormatProvider provider) =>
            decimal.TryParse(input, style, provider, out var number)
                ? Maybe.Value(number)
                : Maybe.None<decimal>();

        public static IMaybe<decimal> Decimal(string input) =>
            decimal.TryParse(input, out var number)
                ? Maybe.Value(number)
                : Maybe.None<decimal>();

        public static IMaybe<double> Double(string input, NumberStyles style, IFormatProvider provider) =>
            double.TryParse(input, style, provider, out var number)
                ? Maybe.Value(number)
                : Maybe.None<double>();

        public static IMaybe<double> Double(string input) => double.TryParse(input, out var number)
            ? Maybe.Value(number)
            : Maybe.None<double>();

        public static IMaybe<TEnum> Enum<TEnum>(string input) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, out var value)
                ? Maybe.Value(value)
                : Maybe.None<TEnum>();

        public static IMaybe<TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                ? Maybe.Value(value)
                : Maybe.None<TEnum>();

        public static IMaybe<Guid> Guid(string input) => System.Guid.TryParse(input, out var guid)
            ? Maybe.Value(guid)
            : Maybe.None<Guid>();

        public static IMaybe<Guid> GuidExact(string input, GuidFormat format) =>
            System.Guid.TryParseExact(input, char.ToString((char) format), out var guid)
                ? Maybe.Value(guid)
                : Maybe.None<Guid>();

        public static IMaybe<int> Int(string input, NumberStyles style, IFormatProvider provider) =>
            int.TryParse(input, style, provider, out var number)
                ? Maybe.Value(number)
                : Maybe.None<int>();

        public static IMaybe<int> Int(string input) => int.TryParse(input, out var number)
            ? Maybe.Value(number)
            : Maybe.None<int>();

        public static IMaybe<long> Long(string input, NumberStyles style, IFormatProvider provider) =>
            long.TryParse(input, style, provider, out var number)
                ? Maybe.Value(number)
                : Maybe.None<long>();

        public static IMaybe<long> Long(string input) =>
            long.TryParse(input, out var number)
                ? Maybe.Value(number)
                : Maybe.None<long>();
    }
}