using System.Globalization;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Maybe {
    public class DateTime {
        [Fact]
        public void With_Invalid_String()
            => MaybeParsers
                .DateTime("foobar")
                .AssertNone();

        [Fact]
        public void With_Invalid_String_With_DateTimeStyle()
            => MaybeParsers
                .DateTime("foobar", DateTimeStyles.None, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Null_String()
            => MaybeParsers
                .DateTime(null)
                .AssertNone();

        [Fact]
        public void With_Null_String_With_DateTimeStyle()
            => MaybeParsers
                .DateTime(null, DateTimeStyles.None, CultureInfo.InvariantCulture)
                .AssertNone();

        [Fact]
        public void With_Valid_String()
            => MaybeParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture))
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Valid_String_With_DateTimeStyle()
            => MaybeParsers
                .DateTime(System.DateTime.Today.ToString(CultureInfo.InvariantCulture), DateTimeStyles.None,
                    CultureInfo.InvariantCulture)
                .AssertValue(System.DateTime.Today);
    }

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
            => MaybeParsers
                .DateTimeExact(
                    "foobar",
                    Formats,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertNone();

        [Fact]
        public void With_Invalid_String_Using_Single_Format()
            => MaybeParsers
                .DateTimeExact(
                    "foobar",
                    Format,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertNone();

        [Fact]
        public void With_Null_String_Using_Multiple_Formats()
            => MaybeParsers
                .DateTimeExact(
                    null,
                    Formats,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertNone();

        [Fact]
        public void With_Null_String_Using_Single_Format()
            => MaybeParsers
                .DateTimeExact(
                    null,
                    Format,
                    DateTimeStyles.None,
                    CultureInfo.InvariantCulture
                )
                .AssertNone();

        [Fact]
        public void With_Valid_String_Using_Multiple_Formats()
            => MaybeParsers
                .DateTimeExact(
                    System.DateTime.Today.ToString(CultureInfo.InvariantCulture),
                    Formats,
                    DateTimeStyles.None,
                    CultureInfo.CurrentCulture
                )
                .AssertValue(System.DateTime.Today);

        [Fact]
        public void With_Valid_String_Using_Single_Format()
            => MaybeParsers
                .DateTimeExact(
                    System.DateTime.Today.ToString(Format),
                    Format,
                    DateTimeStyles.None,
                    CultureInfo.CurrentCulture
                )
                .AssertValue(System.DateTime.Today);
    }
}