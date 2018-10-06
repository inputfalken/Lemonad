using System;
using System.Globalization;

namespace Lemonad.ErrorHandling.Parsers {
    public static class ResultParsers {
        public static IResult<bool, string> Bool(string input) => bool.TryParse(input, out var boolean)
            ? Result.Value<bool, string>(boolean)
            : Result.Error<bool, string>(FormatStringParserMessage<bool>(input));

        public static IResult<DateTime, string> DateTime(string input, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParse(input, provider, style, out var date)
                ? Result.Value<DateTime, string>(date)
                : Result.Error<DateTime, string>(FormatStringParserMessage<DateTime>(input));

        public static IResult<DateTime, string> DateTime(string input) =>
            System.DateTime.TryParse(input, out var date)
                ? Result.Value<DateTime, string>(date)
                : Result.Error<DateTime, string>(FormatStringParserMessage<DateTime>(input));

        public static IResult<DateTime, string> DateTimeExact(string input, string format, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, format, provider, style, out var date)
                ? Result.Value<DateTime, string>(date)
                : Result.Error<DateTime, string>(FormatStringParserMessage<DateTime>(input));

        public static IResult<DateTime, string> DateTimeExact(string input, string[] formats, DateTimeStyles style,
            IFormatProvider provider) =>
            System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                ? Result.Value<DateTime, string>(date)
                : Result.Error<DateTime, string>(FormatStringParserMessage<DateTime>(input));

        public static IResult<decimal, string>
            Decimal(string input, NumberStyles style, IFormatProvider provider) =>
            decimal.TryParse(input, style, provider, out var number)
                ? Result.Value<decimal, string>(number)
                : Result.Error<decimal, string>(FormatStringParserMessage<decimal>(input));

        public static IResult<decimal, string> Decimal(string input) =>
            decimal.TryParse(input, out var number)
                ? Result.Value<decimal, string>(number)
                : Result.Error<decimal, string>(FormatStringParserMessage<decimal>(input));

        public static IResult<double, string> Double(string input, NumberStyles style, IFormatProvider provider) =>
            double.TryParse(input, style, provider, out var number)
                ? Result.Value<double, string>(number)
                : Result.Error<double, string>(FormatStringParserMessage<double>(input));

        public static IResult<double, string> Double(string input) => double.TryParse(input, out var number)
            ? Result.Value<double, string>(number)
            : Result.Error<double, string>(FormatStringParserMessage<double>(input));

        public static IResult<TEnum, string> Enum<TEnum>(string input) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, out var value)
                ? Result.Value<TEnum, string>(value)
                : Result.Error<TEnum, string>(FormatStringParserMessage<TEnum>(input));

        public static IResult<TEnum, string> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
            System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                ? Result.Value<TEnum, string>(value)
                : Result.Error<TEnum, string>(FormatStringParserMessage<TEnum>(input));

        private static string FormatStringParserMessage<T>(string input) where T : struct =>
            $"Could not parse type {typeof(string)}(\"{input}\") into {typeof(T)}.";

        public static IResult<int, string> Int(string input, NumberStyles style, IFormatProvider provider) =>
            int.TryParse(input, style, provider, out var number)
                ? Result.Value<int, string>(number)
                : Result.Error<int, string>(FormatStringParserMessage<int>(input));

        public static IResult<int, string> Int(string input) => int.TryParse(input, out var number)
            ? Result.Value<int, string>(number)
            : Result.Error<int, string>(FormatStringParserMessage<int>(input));

        public static IResult<long, string> Long(string input, NumberStyles style, IFormatProvider provider) =>
            long.TryParse(input, style, provider, out var number)
                ? Result.Value<long, string>(number)
                : Result.Error<long, string>(FormatStringParserMessage<long>(input));

        public static IResult<long, string> Long(string input) =>
            long.TryParse(input, out var number)
                ? Result.Value<long, string>(number)
                : Result.Error<long, string>(FormatStringParserMessage<long>(input));
    }
}