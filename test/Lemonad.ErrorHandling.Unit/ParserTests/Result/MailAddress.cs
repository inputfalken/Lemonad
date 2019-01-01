using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ParserTests.Result {
    public class MailAddress {
            [Fact]
            public void Double_White_Space() {
            ResultParsers.MailAddress("  ")
                .AssertError("Failed parsing input '  '. Mail with white spaces are not allowed.");
        }

        [Fact]
        public void Empty_String() {
            ResultParsers.MailAddress(string.Empty)
                .AssertError("Failed parsing input ''. Mail with empty string is not allowed.");
        }

        [Fact]
        public void Mail_Prefixed_With_Spaces() {
            ResultParsers.MailAddress("  foo@bar.com").Map(x => x.Address).AssertValue("foo@bar.com");
        }

        [Fact]
        public void Mail_Suffixed_And_Prefixed_With_Spaces() {
            ResultParsers.MailAddress("  foo@bar.com   ").Map(x => x.Address).AssertValue("foo@bar.com");
        }

        [Fact]
        public void Mail_Suffixed_With_Spaces() {
            ResultParsers.MailAddress("foo@bar.com   ").Map(x => x.Address).AssertValue("foo@bar.com");
        }

        [Fact]
        public void Mail_With_More_Than_One_At_Symbol_Before_Domain() {
            ResultParsers
                .MailAddress("foo@bar@.com")
                .AssertErrorContains("Failed parsing input 'foo@bar@.com'. Exception:");
        }

        [Fact]
        public void Mail_Without_At_Symbol() {
            ResultParsers
                .MailAddress("foobar.com")
                .AssertError("Failed parsing input 'foobar.com'. Mail with out '@' sign is not allowed.");
        }

        [Fact]
        public void Null_String() {
            ResultParsers.MailAddress(null)
                .AssertError("Failed parsing input ''. Mail with null string is not allowed.");
        }

        [Fact]
        public void Single_White_Space() {
            ResultParsers.MailAddress(" ")
                .AssertError("Failed parsing input ' '. Mail with white space is not allowed.");
        }

        [Fact]
        public void Tripple_White_Space() {
            ResultParsers.MailAddress("   ")
                .AssertError("Failed parsing input '  ...'. Mail with white spaces are not allowed.");
        }

        [Fact]
        public void Upper_Cased_Mail() {
            ResultParsers.MailAddress("FOO@BAR.COM").Map(x => x.Address).AssertValue("foo@bar.com");
        }
    }
}