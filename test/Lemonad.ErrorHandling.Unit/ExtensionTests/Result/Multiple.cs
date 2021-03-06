using System;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result;
using Xunit;

namespace Lemonad.ErrorHandling.Unit.ExtensionTests.Result {
    public class Multiple {
        [Fact]
        public void First_And_Second_False()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => false, d => "Should happen!"),
                    x => x.Filter(y => false, d => "Should happen2!")
                ).AssertError(new[] {"Should happen!", "Should happen2!"});

        [Fact]
        public void First_And_Second_True()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!")
                ).AssertValue(5);

        [Fact]
        public void First_And_Second_True_Additional_False()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => false, d => "Should happen1!"),
                    x => x.Filter(y => false, d => "Should happen2!"),
                    x => x.Filter(y => false, d => "Should happen3!")
                ).AssertError(new[] {"Should happen1!", "Should happen2!", "Should happen3!"});

        [Fact]
        public void First_And_Second_True_Additional_True()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!")
                ).AssertValue(5);

        [Fact]
        public void First_And_Second_True_Mixed_True_And_False()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => false, d => "Should happen1!"),
                    x => x.Filter(y => true, d => "Should happen2!"),
                    x => x.Filter(y => false, d => "Should happen3!")
                ).AssertError(new[] {"Should happen1!", "Should happen3!"});

        [Fact]
        public void First_False()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => false, d => "Should happen!"),
                    x => x.Filter(y => true, d => "Should never happen!")
                ).AssertError(new[] {"Should happen!"});

        [Fact]
        public void First_True()
            => AssertionUtilities
                .Division(10, 2)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => false, d => "Should happen!")
                ).AssertError(new[] {"Should happen!"});

        [Fact]
        public void Passing_Null_Additional_Throws()
            => Assert.Throws<ArgumentNullException>(
                "additional",
                () => AssertionUtilities.Division(10, 2).Multiple(
                    x => x,
                    x => x,
                    null
                )
            );

        [Fact]
        public void Passing_Null_First_Throws()
            => Assert.Throws<ArgumentNullException>(
                "first",
                () => AssertionUtilities.Division(10, 2).Multiple(
                    null,
                    x => x
                )
            );

        [Fact]
        public void Passing_Null_Second_Throws()
            => Assert.Throws<ArgumentNullException>(
                "second",
                () => AssertionUtilities.Division(10, 2).Multiple(
                    x => x,
                    null
                ));

        [Fact]
        public void Passing_Null_Source_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IResult<string, int>) null).Multiple(
                    x => x,
                    x => x
                )
            );

        [Fact]
        public void With_Error_First_And_Second_False()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => false, d => "Should happen!"),
                    x => x.Filter(y => false, d => "Should happen2!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});

        [Fact]
        public void With_Error_First_And_Second_True()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});

        [Fact]
        public void With_Error_First_And_Second_True_Additional_False()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => false, d => "Should happen1!"),
                    x => x.Filter(y => false, d => "Should happen2!"),
                    x => x.Filter(y => false, d => "Should happen3!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});

        [Fact]
        public void With_Error_First_And_Second_True_Additional_True()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});

        [Fact]
        public void With_Error_First_And_Second_True_Mixed_True_And_False()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => false, d => "Should happen1!"),
                    x => x.Filter(y => true, d => "Should happen2!"),
                    x => x.Filter(y => false, d => "Should happen3!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});

        [Fact]
        public void With_Error_First_False()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => false, d => "Should happen!"),
                    x => x.Filter(y => true, d => "Should never happen!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});

        [Fact]
        public void With_Error_First_True()
            => AssertionUtilities
                .Division(10, 0)
                .Multiple(
                    x => x.Filter(y => true, d => "Should never happen!"),
                    x => x.Filter(y => false, d => "Should happen!")
                ).AssertError(new[] {"Can not divide '10' with '0'."});
    }
}