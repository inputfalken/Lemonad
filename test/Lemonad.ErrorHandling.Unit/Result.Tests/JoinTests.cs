using System;
using System.Collections.Generic;
using Assertion;
using Lemonad.ErrorHandling.Extensions;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.Result.Tests {
    public class JoinTests {
        [Fact]
        public void With_Comparer_Passing_Null_Inner_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            inner = null;
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinInnerParameter,
                () => outer.Join(
                    inner,
                    x => x, x => x,
                    (x, y) => $"{x} {y}",
                    () => string.Empty,
                    EqualityComparer<int>.Default
                )
            );
        }

        [Fact]
        public void With_Comparer_Passing_Null_OuterKeySelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinOuterKeyParameter,
                () => outer.Join(
                    inner,
                    null,
                    x => x,
                    (x, y) => $"{x} {y}",
                    () => string.Empty,
                    EqualityComparer<int>.Default
                )
            );
        }

        [Fact]
        public void With_Comparer_Passing_Null_InnerKeySelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinInnerKeyParameter,
                () => outer.Join(
                    inner,
                    x => x,
                    null,
                    (x, y) => $"{x} {y}",
                    () => string.Empty,
                    EqualityComparer<int>.Default
                )
            );
        }

        [Fact]
        public void With_Comparer_Passing_Null_ResultSelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ResultSelector,
                () => outer.Join<int, int, string>(
                    inner,
                    x => x,
                    x => x,
                    null,
                    () => string.Empty,
                    EqualityComparer<int>.Default
                )
            );
        }

        [Fact]
        public void With_Comparer_Passing_Null_ErrorSelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => outer.Join(
                    inner,
                    x => x,
                    x => x,
                    (i, i1) => "",
                    null,
                    EqualityComparer<int>.Default
                )
            );
        }

        [Fact]
        public void With_Comparer_Passing_Comparer_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinCompareParameter,
                () => outer.Join(
                    inner,
                    x => x,
                    x => x,
                    (i, i1) => "",
                    () => "foo",
                    null
                )
            );
        }

        [Fact]
        public void
            With_Comparer_Result_With_No_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer.Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => string.Empty,
                EqualityComparer<int>.Default
            ).AssertError("ERROR 1");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void
            With_Comparer_Result_With_No_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Join(inner, x => {
                        outerSelectorInvoked = true;
                        return x.Id;
                    }, x => {
                        innerSelectorInvoked = true;
                        return x.Id;
                    }, (x, y) => {
                        resultSelectorInvoked = true;
                        return $"{x.Text} {y.Text}";
                    }, () => string.Empty,
                    EqualityComparer<int>.Default
                )
                .AssertError("ERROR 1");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void
            With_Comparer_Result_With_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer
                .Join(inner, x => {
                        outerSelectorInvoked = true;
                        return x.Id;
                    }, x => {
                        innerSelectorInvoked = true;
                        return x.Id;
                    }, (x, y) => {
                        resultSelectorInvoked = true;
                        return $"{x.Text} {y.Text}";
                    }, () => string.Empty,
                    EqualityComparer<int>.Default
                )
                .AssertError("ERROR 2");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void
            With_Comparer_Result_With_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Join(inner, x => {
                        outerSelectorInvoked = true;
                        return x.Id;
                    }, x => {
                        innerSelectorInvoked = true;
                        return x.Id;
                    }, (x, y) => {
                        resultSelectorInvoked = true;
                        return $"{x.Text} {y.Text}";
                    }, () => "",
                    EqualityComparer<int>.Default
                )
                .AssertValue("Hello world");

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void
            With_Comparer_Result_With_Value_Joins_Result_With_Value_Using_No_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 2, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer.Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => "No key match",
                EqualityComparer<int>.Default
            ).AssertError("No key match");

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Passing_Null_Inner_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            inner = null;
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinInnerParameter,
                () => outer.Join(
                    inner,
                    x => x, x => x,
                    (x, y) => $"{x} {y}",
                    () => string.Empty
                )
            );
        }

        [Fact]
        public void Passing_Null_OuterKeySelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinOuterKeyParameter,
                () => outer.Join(
                    inner,
                    null,
                    x => x,
                    (x, y) => $"{x} {y}",
                    () => string.Empty
                )
            );
        }

        [Fact]
        public void Passing_Null_InnerKeySelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.JoinInnerKeyParameter,
                () => outer.Join(
                    inner,
                    x => x,
                    null,
                    (x, y) => $"{x} {y}",
                    () => string.Empty
                )
            );
        }

        [Fact]
        public void Passing_Null_ResultSelector_Throws() {
            var outer = 1.ToResult(x => false, x => "ERROR 1");
            var inner = 1.ToResult(x => false, x => "ERROR 2");
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ResultSelector,
                () => outer.Join<int, int, string>(
                    inner,
                    x => x,
                    x => x,
                    null,
                    () => string.Empty
                )
            );
        }

        [Fact]
        public void Passing_Null_ErrorSelector_Throws() {
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1").Map(x => x.Id);
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2").Map(x => x.Id);
            Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => outer.Join(
                    inner,
                    x => x,
                    x => x,
                    (i, i1) => "",
                    null
                )
            );
        }

        [Fact]
        public void Result_With_No_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer.Join(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => string.Empty).AssertError("ERROR 1");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_No_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => false, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => string.Empty)
                .AssertError("ERROR 1");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Joins_Result_With_No_Value_Using_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => false, x => "ERROR 2");
            outer
                .Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => string.Empty)
                .AssertError("ERROR 2");

            Assert.False(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.False(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Joins_Result_With_Value_Using_Matching_Key__Expects_Result_With_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 1, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer
                .Join(inner, x => {
                    outerSelectorInvoked = true;
                    return x.Id;
                }, x => {
                    innerSelectorInvoked = true;
                    return x.Id;
                }, (x, y) => {
                    resultSelectorInvoked = true;
                    return $"{x.Text} {y.Text}";
                }, () => "")
                .AssertValue("Hello world");

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.True(resultSelectorInvoked, "resultSelectorInvoked");
        }

        [Fact]
        public void Result_With_Value_Joins_Result_With_Value_Using_No_Matching_Key__Expects_Result_With_No_Value() {
            var outerSelectorInvoked = false;
            var innerSelectorInvoked = false;
            var resultSelectorInvoked = false;
            var outer = new {Id = 2, Text = "Hello"}.ToResult(x => true, x => "ERROR 1");
            var inner = new {Id = 1, Text = "world"}.ToResult(x => true, x => "ERROR 2");
            outer.Join(inner, x => {
                outerSelectorInvoked = true;
                return x.Id;
            }, x => {
                innerSelectorInvoked = true;
                return x.Id;
            }, (x, y) => {
                resultSelectorInvoked = true;
                return $"{x.Text} {y.Text}";
            }, () => "No key match").AssertError("No key match");

            Assert.True(outerSelectorInvoked, "outerSelectorInvoked");
            Assert.True(innerSelectorInvoked, "innerSelectorInvoked");
            Assert.False(resultSelectorInvoked, "resultSelectorInvoked");
        }
    }
}