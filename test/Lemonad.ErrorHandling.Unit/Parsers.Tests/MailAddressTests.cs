using System;
using Assertion;
using Lemonad.ErrorHandling.Parsers;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Parsers.Tests {
    public class MailAddressTests {
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
            ResultParsers.MailAddress("foo@bar@.com").AssertError(@"Failed parsing input 'foo@bar@.com'. Exception:
System.FormatException: An invalid character was found in the mail header: '.'.
   at System.Net.Mail.DotAtomReader.ReadReverse(String data, Int32 index)
   at System.Net.Mail.MailAddressParser.ParseDomain(String data, Int32& index)
   at System.Net.Mail.MailAddressParser.ParseAddress(String data, Boolean expectMultipleAddresses, Int32& index)
   at System.Net.Mail.MailAddressParser.ParseAddress(String data)
   at System.Net.Mail.MailAddress..ctor(String address, String displayName, Encoding displayNameEncoding)
   at System.Net.Mail.MailAddress..ctor(String address)
   at Lemonad.ErrorHandling.Parsers.ResultParsers.<>c.<MailAddress>b__17_12(String x) in C:\Users\Robert\source\repos\Lemonad\src\Lemonad.ErrorHandling\Parsers\ResultParsers.cs:line 137");
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