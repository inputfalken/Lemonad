using System;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace Lemonad.ErrorHandling {
    // TODO Convert to maybes. we dont have context about why the parsing failed.
    public static class TryParse {
        [Pure]
        private static string FormatMessage(string input) => $"Could not parse string '{input}'.";

        public static Either<string, TEnum> Enum<TEnum>(string input) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, out var value)
                ? (Either<string, TEnum>) value
                : FormatMessage(input);

        public static Either<string, TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                ? (Either<string, TEnum>) value
                : FormatMessage(input);

        public static Either<string, decimal>
            Decimal(string input, NumberStyles style, IFormatProvider provider) =>
            decimal.TryParse(input, style, provider, out var number)
                ? (Either<string, decimal>) number
                : FormatMessage(input);

        public static Either<string, decimal> Decimal(string input) =>
            decimal.TryParse(input, out var number)
                ? (Either<string, decimal>) number
                : FormatMessage(input);

        public static Either<string, int> Int(string input, NumberStyles style, IFormatProvider provider) =>
            int.TryParse(input, style, provider, out var number)
                ? (Either<string, int>) number
                : FormatMessage(input);

        public static Either<string, long> Long(string input, NumberStyles style, IFormatProvider provider) =>
            long.TryParse(input, style, provider, out var number)
                ? (Either<string, long>) number
                : FormatMessage(input);

        public static Either<string, long> Long(string input) =>
            long.TryParse(input, out var number)
                ? (Either<string, long>) number
                : FormatMessage(input);

        public static Either<string, int> Int(string input) => int.TryParse(input, out var number)
            ? (Either<string, int>) number
            : FormatMessage(input);

        public static Either<string, double> Double(string input, NumberStyles style, IFormatProvider provider) =>
            double.TryParse(input, style, provider, out var number)
                ? (Either<string, double>) number
                : FormatMessage(input);

        public static Either<string, double> Double(string input) => double.TryParse(input, out var number)
            ? (Either<string, double>) number
            : FormatMessage(input);

        public static Either<string, DateTime> DateTime(string input, DateTimeStyles style, IFormatProvider provider) =>
            System.DateTime.TryParse(input, provider, style, out var date)
                ? (Either<string, DateTime>) date
                : FormatMessage(input);

        public static Either<string, DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, format, provider, style, out var date)
                ? (Either<string, DateTime>) date
                : FormatMessage(input);

        public static Either<string, DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                ? (Either<string, DateTime>) date
                : FormatMessage(input);

        public static Either<string, DateTime> DateTime(string input) => System.DateTime.TryParse(input, out var date)
            ? (Either<string, DateTime>) date
            : FormatMessage(input);

        public static Either<string, bool> Bool(string input) => bool.TryParse(input, out var boolean)
            ? (Either<string, bool>) boolean
            : FormatMessage(input);
    }
}