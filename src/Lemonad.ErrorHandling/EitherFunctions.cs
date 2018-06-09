using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;

namespace Lemonad.ErrorHandling {
    public static class Either {
        public static IEnumerable<TLeft> EitherLefts<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> enumerable) => enumerable.SelectMany(x => x.LeftEnumerable);

        public static IEnumerable<TRight> EitherRights<TLeft, TRight>(
            this IEnumerable<Either<TLeft, TRight>> enumerable) => enumerable.SelectMany(x => x.RightEnumerable);

        [Pure]
        public static Either<TLeft, TRight> Right<TLeft, TRight>(TRight right) =>
            new Either<TLeft, TRight>(default(TLeft), right, false, true);

        [Pure]
        public static Either<TLeft, TRight> Left<TLeft, TRight>(TLeft left) =>
            new Either<TLeft, TRight>(left, default(TRight), true, false);

        [Pure]
        public static Either<TLeft, TRight>
            ToEither<TLeft, TRight>(this TRight? source, Func<TLeft> leftSelector) where TRight : struct =>
            source.HasValue
                ? Right<TLeft, TRight>(source.Value)
                : (leftSelector != null
                    ? Left<TLeft, TRight>(leftSelector())
                    : throw new ArgumentNullException(nameof(leftSelector)));

        [Pure]
        public static Either<TLeft, TRight> ToEitherRight<TLeft, TRight>(this TRight right) =>
            Right<TLeft, TRight>(right);

        [Pure]
        public static Either<TLeft, TRight> ToEitherLeft<TLeft, TRight>(this TLeft left) => Left<TLeft, TRight>(left);

        [Pure]
        public static Maybe<TRight> ConvertToMaybe<TLeft, TRight>(this Either<TLeft, TRight> source) =>
            source.IsRight ? source.Right.Some() : Maybe<TRight>.None;

        public static class Parse {
            private static string FormatStringParserMessage<TEnum>(string input) where TEnum : struct =>
                $"Could not parse type {typeof(string)}(\"{input}\") into {typeof(TEnum)}.";

            public static Either<string, TEnum> Enum<TEnum>(string input) where TEnum : struct =>
                System.Enum.TryParse<TEnum>(input, out var value)
                    ? (Either<string, TEnum>) value
                    : FormatStringParserMessage<TEnum>(input);

            public static Either<string, TEnum> Enum<TEnum>(string input, bool ignoreCase) where TEnum : struct =>
                System.Enum.TryParse<TEnum>(input, ignoreCase, out var value)
                    ? (Either<string, TEnum>) value
                    : FormatStringParserMessage<TEnum>(input);

            public static Either<string, decimal>
                Decimal(string input, NumberStyles style, IFormatProvider provider) =>
                decimal.TryParse(input, style, provider, out var number)
                    ? (Either<string, decimal>) number
                    : FormatStringParserMessage<decimal>(input);

            public static Either<string, decimal> Decimal(string input) =>
                decimal.TryParse(input, out var number)
                    ? (Either<string, decimal>) number
                    : FormatStringParserMessage<decimal>(input);

            public static Either<string, int> Int(string input, NumberStyles style, IFormatProvider provider) =>
                int.TryParse(input, style, provider, out var number)
                    ? (Either<string, int>) number
                    : FormatStringParserMessage<int>(input);

            public static Either<string, long> Long(string input, NumberStyles style, IFormatProvider provider) =>
                long.TryParse(input, style, provider, out var number)
                    ? (Either<string, long>) number
                    : FormatStringParserMessage<long>(input);

            public static Either<string, long> Long(string input) =>
                long.TryParse(input, out var number)
                    ? (Either<string, long>) number
                    : FormatStringParserMessage<long>(input);

            public static Either<string, int> Int(string input) => int.TryParse(input, out var number)
                ? (Either<string, int>) number
                : FormatStringParserMessage<int>(input);

            public static Either<string, double> Double(string input, NumberStyles style, IFormatProvider provider) =>
                double.TryParse(input, style, provider, out var number)
                    ? (Either<string, double>) number
                    : FormatStringParserMessage<double>(input);

            public static Either<string, double> Double(string input) => double.TryParse(input, out var number)
                ? (Either<string, double>) number
                : FormatStringParserMessage<double>(input);

            public static Either<string, DateTime> DateTime(string input, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParse(input, provider, style, out var date)
                    ? (Either<string, DateTime>) date
                    : FormatStringParserMessage<DateTime>(input);

            public static Either<string, DateTime> DateTimeExact(string input, string format, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParseExact(input, format, provider, style, out var date)
                    ? (Either<string, DateTime>) date
                    : FormatStringParserMessage<DateTime>(input);

            public static Either<string, DateTime> DateTimeExact(string input, string[] formats, DateTimeStyles style,
                IFormatProvider provider) =>
                System.DateTime.TryParseExact(input, formats, provider, style, out var date)
                    ? (Either<string, DateTime>) date
                    : FormatStringParserMessage<DateTime>(input);

            public static Either<string, DateTime> DateTime(string input) =>
                System.DateTime.TryParse(input, out var date)
                    ? (Either<string, DateTime>) date
                    : FormatStringParserMessage<DateTime>(input);

            public static Either<string, bool> Bool(string input) => bool.TryParse(input, out var boolean)
                ? (Either<string, bool>) boolean
                : FormatStringParserMessage<bool>(input);
        }
    }
}