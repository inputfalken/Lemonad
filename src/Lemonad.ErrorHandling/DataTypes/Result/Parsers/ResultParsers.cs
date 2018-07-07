using System;
using System.Globalization;

namespace Lemonad.ErrorHandling.DataTypes.Result.Parsers {
    public static class ResultParsers {
        private static string FormatStringParserMessage<T>(string input) where T : struct =>
            $"Could not parse type {typeof(string)}(\"{input}\") into {typeof(T)}.";

        public static Result<TEnum, string> Enum<TEnum>(string input) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, out var value)
                ? (Result<TEnum, string>) value
                : FormatStringParserMessage<TEnum>(input);

        public static Result<TEnum, string> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                ? (Result<TEnum, string>) value
                : FormatStringParserMessage<TEnum>(input);

        public static Result<decimal, string>
            Decimal(string input, NumberStyles style, IFormatProvider provider) =>
            decimal.TryParse(input, style, provider, out var number)
                ? (Result<decimal, string>) number
                : FormatStringParserMessage<decimal>(input);

        public static Result<decimal, string> Decimal(string input) =>
            decimal.TryParse(input, out var number)
                ? (Result<decimal, string>) number
                : FormatStringParserMessage<decimal>(input);

        public static Result<int, string> Int(string input, NumberStyles style, IFormatProvider provider) =>
            int.TryParse(input, style, provider, out var number)
                ? (Result<int, string>) number
                : FormatStringParserMessage<int>(input);

        public static Result<long, string> Long(string input, NumberStyles style, IFormatProvider provider) =>
            long.TryParse(input, style, provider, out var number)
                ? (Result<long, string>) number
                : FormatStringParserMessage<long>(input);

        public static Result<long, string> Long(string input) =>
            long.TryParse(input, out var number)
                ? (Result<long, string>) number
                : FormatStringParserMessage<long>(input);

        public static Result<int, string> Int(string input) => int.TryParse(input, out var number)
            ? (Result<int, string>) number
            : FormatStringParserMessage<int>(input);

        public static Result<double, string> Double(string input, NumberStyles style, IFormatProvider provider) =>
            double.TryParse(input, style, provider, out var number)
                ? (Result<double, string>) number
                : FormatStringParserMessage<double>(input);

        public static Result<double, string> Double(string input) => double.TryParse(input, out var number)
            ? (Result<double, string>) number
            : FormatStringParserMessage<double>(input);

        public static Result<DateTime, string> DateTime(string input, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParse(input, provider, style, out var date)
                ? (Result<DateTime, string>) date
                : FormatStringParserMessage<DateTime>(input);

        public static Result<DateTime, string> DateTimeExact(string input, string format, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, format, provider, style, out var date)
                ? (Result<DateTime, string>) date
                : FormatStringParserMessage<DateTime>(input);

        public static Result<DateTime, string> DateTimeExact(string input, string[] formats, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                ? (Result<DateTime, string>) date
                : FormatStringParserMessage<DateTime>(input);

        public static Result<DateTime, string> DateTime(string input) =>
            System.DateTime.TryParse(input, out var date)
                ? (Result<DateTime, string>) date
                : FormatStringParserMessage<DateTime>(input);

        public static Result<bool, string> Bool(string input) => bool.TryParse(input, out var boolean)
            ? (Result<bool, string>) boolean
            : FormatStringParserMessage<bool>(input);
    }
}