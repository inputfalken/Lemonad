using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class DateTimeExact {
        private static readonly string[] Formats = {
            "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
            "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
            "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
            "M/d/yyyy h:mm", "M/d/yyyy h:mm",
            "MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm"
        };

        private const string Format = "G";

        [Fact]
        public void With_Invalid_String_Using_Multiple_Formats()
            => ResultParsers
                .DateTimeExact(
                    "foobar",
                    Formats,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Invalid_String_Using_Single_Format()
            => ResultParsers
                .DateTimeExact(
                    "foobar",
                    Format,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        "foobar"
                    )
                );

        [Fact]
        public void With_Null_String_Using_Multiple_Formats()
            => ResultParsers
                .DateTimeExact(
                    null,
                    Formats,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );

        [Fact]
        public void With_Null_String_Using_Single_Format()
            => ResultParsers
                .DateTimeExact(
                    null,
                    Format,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertError(
                    AssertionUtilities.FormatStringParserMessage<System.DateTime>(
                        null
                    )
                );

        [Fact]
        public void With_Valid_String_Using_Multiple_Formats()
            => ResultParsers
                .DateTimeExact(
                    System.DateTime.Today.ToString(CultureInfo.InvariantCulture),
                    Formats,
                    DateTimeStyles.None,
                    CultureInfo.CurrentCulture
                )
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Valid_String_Using_Single_Format()
            => ResultParsers
                .DateTimeExact(
                    System.DateTime.Today.ToString(Format),
                    Format,
                    DateTimeStyles.None,
                    CultureInfo.CurrentCulture
                )
                .AssertValue(System.DateTime.Today);
    }
}