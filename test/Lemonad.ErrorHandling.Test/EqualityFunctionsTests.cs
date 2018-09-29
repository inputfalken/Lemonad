using System;
using Xunit;

namespace Lemonad.ErrorHandling.Test {
    public class EqualityFunctionsTests {
        [Fact]
        public void Null_String_Expected_To_Be_True() {
            string foo = null;
            Assert.True(foo.IsNull());
        }

        [Fact]
        public void Empty_String_Expected_To_Be_False() {
            var foo = string.Empty;
            Assert.False(foo.IsNull());
        }

        [Fact]
        public void Default_Structs_Expected_To_Be_False() {
            var date = default(DateTime);
            const int integer32 = default;
            const long integer64 = default;
            Assert.False(date.IsNull());
            Assert.False(integer32.IsNull());
            Assert.False(integer64.IsNull());
        }

        [Fact]
        public void Default_Nullable_Structs_Expected_To_Be_True() {
            DateTime? date = default;
            int? integer32 = default;
            long? integer64 = default;
            Assert.True(date.IsNull());
            Assert.True(integer32.IsNull());
            Assert.True(integer64.IsNull());
        }
        
        [Fact]
        public void Nullable_Structs_With_Value_Expected_To_Be_False() {
            DateTime? date = DateTime.Now;
            int? integer32 = 2;
            long? integer64 = long.MaxValue;
            Assert.False(date.IsNull());
            Assert.False(integer32.IsNull());
            Assert.False(integer64.IsNull());
        }
    }
}