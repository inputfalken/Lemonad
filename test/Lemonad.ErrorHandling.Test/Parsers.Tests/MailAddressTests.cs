using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Parsers.Tests {
    public class MailAddressTests {
        [Fact]
        public void Double_White_Space() {
            var mailAddress = ResultParsers.MailAddress("  ").Either;
            Assert.True(mailAddress.HasError);
            Assert.False(mailAddress.HasValue);
            Assert.Equal("Failed parsing input '  '. Mail with white spaces are not allowed.", mailAddress.Error);
        }

        [Fact]
        public void Empty_String() {
            var mailAddress = ResultParsers.MailAddress(string.Empty).Either;
            Assert.True(mailAddress.HasError);
            Assert.False(mailAddress.HasValue);
            Assert.Equal("Failed parsing input ''. Mail with empty string is not allowed.", mailAddress.Error);
        }

        [Fact]
        public void Mail_Prefixed_With_Spaces() {
            var mailAddress = ResultParsers.MailAddress("  foo@bar.com").Either;
            Assert.False(mailAddress.HasError);
            Assert.True(mailAddress.HasValue);
            Assert.Equal("foo@bar.com", mailAddress.Value.Address);
        }

        [Fact]
        public void Mail_Suffixed_And_Prefixed_With_Spaces() {
            var mailAddress = ResultParsers.MailAddress("  foo@bar.com   ").Either;
            Assert.False(mailAddress.HasError);
            Assert.True(mailAddress.HasValue);
            Assert.Equal("foo@bar.com", mailAddress.Value.Address);
        }

        [Fact]
        public void Mail_Suffixed_With_Spaces() {
            var mailAddress = ResultParsers.MailAddress("foo@bar.com   ").Either;
            Assert.False(mailAddress.HasError);
            Assert.True(mailAddress.HasValue);
            Assert.Equal("foo@bar.com", mailAddress.Value.Address);
        }

        [Fact]
        public void Mail_With_More_Than_One_At_Symbol() {
            var mailAddress = ResultParsers.MailAddress("foo@bar@.com").Either;
            Assert.False(mailAddress.HasError);
            Assert.True(mailAddress.HasValue);
            Assert.Equal("foo@bar@.com", mailAddress.Value.Address);
        }

        [Fact]
        public void Mail_Without_At_Symbol() {
            var mailAddress = ResultParsers.MailAddress("foobar.com").Either;
            Assert.True(mailAddress.HasError);
            Assert.False(mailAddress.HasValue);
            Assert.Equal("Failed parsing input 'foobar.com'. Mail with out '@' sign is not allowed.",
                mailAddress.Error);
        }

        [Fact]
        public void Null_String() {
            var mailAddress = ResultParsers.MailAddress(null).Either;
            Assert.True(mailAddress.HasError);
            Assert.False(mailAddress.HasValue);
            Assert.Equal("Failed parsing input ''. Mail with null string is not allowed.", mailAddress.Error);
        }

        [Fact]
        public void Single_White_Space() {
            var mailAddress = ResultParsers.MailAddress(" ").Either;
            Assert.True(mailAddress.HasError);
            Assert.False(mailAddress.HasValue);
            Assert.Equal("Failed parsing input ' '. Mail with white space is not allowed.", mailAddress.Error);
        }

        [Fact]
        public void Tripple_White_Space() {
            var mailAddress = ResultParsers.MailAddress("   ").Either;
            Assert.True(mailAddress.HasError);
            Assert.False(mailAddress.HasValue);
            Assert.Equal("Failed parsing input '  ...'. Mail with white spaces are not allowed.", mailAddress.Error);
        }

        [Fact]
        public void Upper_Cased_Mail() {
            var mailAddress = ResultParsers.MailAddress("FOO@BAR.COM").Either;
            Assert.False(mailAddress.HasError);
            Assert.True(mailAddress.HasValue);
            Assert.Equal("foo@bar.com", mailAddress.Value.Address);
        }
    }
}
