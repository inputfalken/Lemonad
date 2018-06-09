using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class LeftWhenTests {
        [Fact]
        public void
            Either_int_int___Int_Whose_Property_IsRight_Is_False_With_False_Predicate() {
            var either = "ERROR".ToEitherLeft<string, int>();
            var result = either.LeftWhen(x => false, () => 20);

            Assert.False(result.IsRight, "Either should have left value.");
            Assert.True(result.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), result.Right);
            Assert.Equal(20, result.Left);
        }

        [Fact]
        public void
            Either_Int_int___Whose_Property_IsRight_Is_False_With_True_Predicate() {
            var either = "ERROR".ToEitherLeft<string, int>().LeftWhen(x => true, () => 20);

            Assert.False(either.IsRight, "Either should have left value.");
            Assert.True(either.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal(20, either.Left);
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_False_Predicate_And_Null_LeftSelector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<string> leftSelector = null;
                var either = 20.ToEitherRight<string, int>();
                var result = either.LeftWhen(_ => false, leftSelector);
                Assert.True(result.IsRight, "Either should have right value");
                Assert.Equal(20, result.Right);
                Assert.Equal(default(string), result.Left);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_Null_Predicate__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                var either = 20.ToEitherRight<string, int>();
                var result = either.LeftWhen(predicate, () => "");
            });
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_Null_Predicate_And_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> leftSelector = null;
                var either = 20.ToEitherRight<string, int>();
                var result = either.LeftWhen(predicate, leftSelector);
            });
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_True_Predicate() {
            var either = 20.ToEitherRight<string, int>();
            var result = either.LeftWhen(x => true, () => "ERROR");

            Assert.False(result.IsRight, "Either should have left value.");
            Assert.True(result.IsLeft, "Either should have left value.");
            Assert.Equal("ERROR", result.Left);
            Assert.Equal(default(int), result.Right);
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_True_Predicate_And_Null_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> leftSelector = null;
                var either = 20.ToEitherRight<string, int>();
                var result = either.LeftWhen(_ => true, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_FalsePredicate_And_Null_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                // Leftselector is mandatory if  it's a left either from the start.
                var result = either.LeftWhen(_ => false, leftSelector);
                Assert.True(result.IsRight, "Either should have right value");
                Assert.Equal(20, result.Right);
                Assert.Equal(default(string), result.Left);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_Predicate__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<int, bool> predicate = null;
                var either = "ERROR".ToEitherLeft<string, int>();

                // Predicate is not mandatory if  it's a left either from the start.
                var result = either.LeftWhen(predicate, () => "ERROR FROM LEFTSELECTOR");
                Assert.True(result.IsLeft, "Either should have left value.");
                Assert.False(result.IsRight, "Either should have left value.");
                Assert.Equal("ERROR FROM LEFTSELECTOR", result.Left);
                Assert.Equal(default(int), result.Right);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_Predicate_And_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                // Leftselector is mandatory if  it's a left either from the start.
                var result = either.LeftWhen(predicate, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_Predicate_And_Null_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                var result = either.LeftWhen(predicate, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_True_Predicate_And_Null_LeftSelector__Expects_No_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                var result = either.LeftWhen(_ => true, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True_With_False_Predicate() {
            var either = 20.ToEitherRight<string, int>();
            var result = either.LeftWhen(x => false, () => "ERROR");

            Assert.True(result.IsRight, "Either should have right value.");
            Assert.False(result.IsLeft, "Either should have right value.");
            Assert.Equal(20, result.Right);
            Assert.Equal(default(string), result.Left);
        }
    }
}