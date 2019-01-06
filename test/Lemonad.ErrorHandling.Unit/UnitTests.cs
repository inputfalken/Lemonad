using Xunit;

namespace Lemonad.ErrorHandling.Unit {
    public class UnitTests {
        [Fact]
        public void GetHashCode__Returns_Zero() {
            Assert.Equal(0, ErrorHandling.Internal.Unit.Default.GetHashCode());
        }

        [Fact]
        public void ToString_Return_Parantheses() {
            Assert.Equal("()", ErrorHandling.Internal.Unit.Default.ToString());
        }

        [Fact]
        public void Unit_CompareTo_Unit__Expects_Zero() {
            Assert.Equal(0,
                ErrorHandling.Internal.Unit.Default.CompareTo(ErrorHandling.Internal.Unit
                    .Default));
        }

        [Fact]
        public void Unit_Equals_Object_String__Expects_False() {
            Assert.False(ErrorHandling.Internal.Unit.Default.Equals("foobar"));
        }

        [Fact]
        public void Unit_Equals_Object_Unit__Expects_True() {
            Assert.True(
                ErrorHandling.Internal.Unit.Default.Equals((object) ErrorHandling.Internal.Unit
                    .Default));
        }

        [Fact]
        public void Unit_Equals_Unit__Expects_True() {
            Assert.True(
                ErrorHandling.Internal.Unit.Default.Equals(ErrorHandling.Internal.Unit.Default));
        }
    }
}