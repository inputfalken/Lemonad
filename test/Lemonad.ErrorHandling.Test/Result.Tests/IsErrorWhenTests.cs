using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Result.Tests {
    public class errorWhenTests {
        [Fact]
        public void
            Result_Int_Whose_Property_HasValue_Is_True_With_False_Predicate_And_Null_errorSelector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<string> errorSelector = null;
                var result = 20.ToResult<int, string>().IsErrorWhen(_ => false, errorSelector);
                Assert.True(result.HasValue, "Result should have value");
                Assert.Equal(20, result.Value);
                Assert.Equal(default(string), result.Error);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_Int_Whose_Property_HasValue_Is_True_With_Null_Predicate__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                var result = 20.ToResult<int, string>().IsErrorWhen(predicate, () => "");
            });
        }

        [Fact]
        public void
            Result_Int_Whose_Property_HasValue_Is_True_With_Null_Predicate_And_errorSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> errorSelector = null;
                var result = 20.ToResult<int, string>().IsErrorWhen(predicate, errorSelector);
            });
        }

        [Fact]
        public void
            Result_Int_Whose_Property_HasValue_Is_True_With_True_Predicate() {
            var result = 20.ToResult<int, string>().IsErrorWhen(x => true, () => "ERROR");

            Assert.False(result.HasValue, "Result should have error value.");
            Assert.True(result.HasError, "Result should have error value.");
            Assert.Equal("ERROR", result.Error);
            Assert.Equal(default(int), result.Value);
        }

        [Fact]
        public void
            Result_Int_Whose_Property_HasValue_Is_True_With_True_Predicate_And_Null_errorSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> errorSelector = null;
                var result = 20.ToResult<int, string>().IsErrorWhen(_ => true, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_int___Whose_Property_HasValue_Is_False_With_True_Predicate() {
            var result = "ERROR".ToResultError<int, string>().IsErrorWhen(x => true, () => "foo");

            Assert.False(result.HasValue, "Result should have error value.");
            Assert.True(result.HasError, "Result should have error value.");
            Assert.Equal(default(int), result.Value);
            Assert.Equal("ERROR", result.Error);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_FalsePredicate_And_Null_errorSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> errorSelector = null;
                var result = "ERROR".ToResultError<int, string>().IsErrorWhen(_ => false, errorSelector);
                // errorselector is mandatory if  it's a error result from the start.
                Assert.True(result.HasValue, "Result should have value");
                Assert.Equal(20, result.Value);
                Assert.Equal(default(string), result.Error);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_Predicate__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<int, bool> predicate = null;
                var result = "ERROR".ToResultError<int, string>().IsErrorWhen(predicate, () => "ERROR FROM errorSELECTOR");

                // Predicate is not mandatory if  it's a error result from the start.
                Assert.True(result.HasError, "Result should have error value.");
                Assert.False(result.HasValue, "Result should have error value.");
                Assert.Equal("ERROR", result.Error);
                Assert.Equal(default(int), result.Value);
            });
            Assert.Null(exception);
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_Predicate_And_errorSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> errorSelector = null;
                var result = "ERROR".ToResultError<int, string>().IsErrorWhen(predicate, errorSelector);
                // errorselector is mandatory if  it's a error result from the start.
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_Null_Predicate_And_Null_errorSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> errorSelector = null;
                var result = "ERROR".ToResultError<int, string>().IsErrorWhen(predicate, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_False_With_True_Predicate_And_Null_errorSelector__Expects_No_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> errorSelector = null;
                var result = "ERROR".ToResultError<int, string>().IsErrorWhen(_ => true, errorSelector);
            });
        }

        [Fact]
        public void
            Result_String_Int__Whose_Property_HasValue_Is_True_With_False_Predicate() {
            var result = 20.ToResult<int, string>().IsErrorWhen(x => false, () => "ERROR");

            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should have value.");
            Assert.Equal(20, result.Value);
            Assert.Equal(default(string), result.Error);
        }

        [Fact]
        public void
            Result_String_int_Whose_Property_HasValue_Is_False__Expected_To_Not_Be_Mapped_To_StringEmpty__With_False_Predicate() {
            var result = "foo".ToResultError<int, string>().IsErrorWhen(i => false, () => string.Empty);

            Assert.False(result.HasValue, "Result should have error value.");
            Assert.True(result.HasError, "Result should have error value.");
            Assert.Equal(default(int), result.Value);
            Assert.Equal("foo", result.Error);
        }

        [Fact]
        public void
            Result_String_int_Whose_Property_HasValue_Is_False__Expected_To_Not_Be_Mapped_To_StringEmpty__With_True_Predicate() {
            var result = "foo".ToResultError<int, string>().IsErrorWhen(i => true, () => string.Empty);

            Assert.False(result.HasValue, "Result should have error value.");
            Assert.True(result.HasError, "Result should have error value.");
            Assert.Equal(default(int), result.Value);
            Assert.Equal("foo", result.Error);
        }

        [Fact]
        public void
            Result_String_int_Whose_Property_HasValue_Is_False_With_False_Predicate() {
            var result = "ERROR".ToResultError<int, string>().IsErrorWhen(x => true, () => "Foo");

            Assert.False(result.HasValue, "Result should have error value.");
            Assert.True(result.HasError, "Result should have error value.");
            Assert.Equal(default(int), result.Value);
            Assert.Equal("ERROR", result.Error);
        }

        [Fact]
        public void
            Result_String_int_Whose_Property_HasValue_Is_True__Expected_Not_To__Be_Mapped_To_String__With_False_Predicate() {
            var result = 2.ToResult<int, string>().IsErrorWhen(i => false, () => "foo");

            Assert.True(result.HasValue, "Result should have value.");
            Assert.False(result.HasError, "Result should not have error value.");
            Assert.Equal(2, result.Value);
            Assert.Equal(default(string), result.Error);
        }

        [Fact]
        public void
            Result_String_int_Whose_Property_HasValue_Is_True__Expected_To_Be_Mapped_To_String__With_True_Predicate() {
            var result = 2.ToResult<int, string>().IsErrorWhen(i => true, () => "foo");

            Assert.False(result.HasValue, "Result should have error value.");
            Assert.True(result.HasError, "Result should have error value.");
            Assert.Equal(default(int), result.Value);
            Assert.Equal("foo", result.Error);
        }
    }
}
