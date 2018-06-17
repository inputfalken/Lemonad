﻿using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test.Either.Tests {
    public class LeftWhenTests {
        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_False_Predicate_And_Null_LeftSelector__Expects_No_ArgumentNullException_Thrown() {
            var exception = Record.Exception(() => {
                Func<string> leftSelector = null;
                var either = 20.ToEitherRight<string, int>();
                var result = either.IsLeftWhen(_ => false, leftSelector);
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
                var result = either.IsLeftWhen(predicate, () => "");
            });
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_Null_Predicate_And_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> leftSelector = null;
                var either = 20.ToEitherRight<string, int>();
                var result = either.IsLeftWhen(predicate, leftSelector);
            });
        }

        [Fact]
        public void
            Either_Int_Whose_Property_IsRight_Is_True_With_True_Predicate() {
            var either = 20.ToEitherRight<string, int>();
            var result = either.IsLeftWhen(x => true, () => "ERROR");

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
                var result = either.IsLeftWhen(_ => true, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_int___Whose_Property_IsRight_Is_False_With_True_Predicate() {
            var either = "ERROR".ToEitherLeft<string, int>().IsLeftWhen(x => true, () => "foo");

            Assert.False(either.IsRight, "Either should have left value.");
            Assert.True(either.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), either.Right);
            Assert.Equal("ERROR", either.Left);
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_FalsePredicate_And_Null_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                // Leftselector is mandatory if  it's a left either from the start.
                var result = either.IsLeftWhen(_ => false, leftSelector);
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
                var result = either.IsLeftWhen(predicate, () => "ERROR FROM LEFTSELECTOR");
                Assert.True(result.IsLeft, "Either should have left value.");
                Assert.False(result.IsRight, "Either should have left value.");
                Assert.Equal("ERROR", result.Left);
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
                var result = either.IsLeftWhen(predicate, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_Null_Predicate_And_Null_LeftSelector__Expects_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<int, bool> predicate = null;
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                var result = either.IsLeftWhen(predicate, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_False_With_True_Predicate_And_Null_LeftSelector__Expects_No_ArgumentNullException_Thrown() {
            Assert.Throws<ArgumentNullException>(() => {
                Func<string> leftSelector = null;
                var either = "ERROR".ToEitherLeft<string, int>();
                var result = either.IsLeftWhen(_ => true, leftSelector);
            });
        }

        [Fact]
        public void
            Either_String_Int__Whose_Property_IsRight_Is_True_With_False_Predicate() {
            var either = 20.ToEitherRight<string, int>();
            var result = either.IsLeftWhen(x => false, () => "ERROR");

            Assert.True(result.IsRight, "Either should have right value.");
            Assert.False(result.IsLeft, "Either should have right value.");
            Assert.Equal(20, result.Right);
            Assert.Equal(default(string), result.Left);
        }

        [Fact]
        public void
            Either_String_int_Whose_Property_IsRight_Is_False__Expected_To_Not_Be_Mapped_To_StringEmpty__With_False_Predicate() {
            var result = "foo".ToEitherLeft<string, int>().IsLeftWhen(i => false, () => string.Empty);

            Assert.False(result.IsRight, "Either should have left value.");
            Assert.True(result.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), result.Right);
            Assert.Equal("foo", result.Left);
        }

        [Fact]
        public void
            Either_String_int_Whose_Property_IsRight_Is_False__Expected_To_Not_Be_Mapped_To_StringEmpty__With_True_Predicate() {
            var result = "foo".ToEitherLeft<string, int>().IsLeftWhen(i => true, () => string.Empty);

            Assert.False(result.IsRight, "Either should have left value.");
            Assert.True(result.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), result.Right);
            Assert.Equal("foo", result.Left);
        }

        [Fact]
        public void
            Either_String_int_Whose_Property_IsRight_Is_False_With_False_Predicate() {
            var either = "ERROR".ToEitherLeft<string, int>();
            var result = either.IsLeftWhen(x => true, () => "Foo");

            Assert.False(result.IsRight, "Either should have left value.");
            Assert.True(result.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), result.Right);
            Assert.Equal("ERROR", result.Left);
        }

        [Fact]
        public void
            Either_String_int_Whose_Property_IsRight_Is_True__Expected_Not_To__Be_Mapped_To_String__With_False_Predicate() {
            var result = 2.ToEitherRight<string, int>().IsLeftWhen(i => false, () => "foo");

            Assert.True(result.IsRight, "Either should have right value.");
            Assert.False(result.IsLeft, "Either should not have left value.");
            Assert.Equal(2, result.Right);
            Assert.Equal(default(string), result.Left);
        }

        [Fact]
        public void
            Either_String_int_Whose_Property_IsRight_Is_True__Expected_To_Be_Mapped_To_String__With_True_Predicate() {
            var result = 2.ToEitherRight<string, int>().IsLeftWhen(i => true, () => "foo");

            Assert.False(result.IsRight, "Either should have left value.");
            Assert.True(result.IsLeft, "Either should have left value.");
            Assert.Equal(default(int), result.Right);
            Assert.Equal("foo", result.Left);
        }
    }
}