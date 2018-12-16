using System;
using Xunit;

namespace Lemonad.ErrorHandling.Unit {
    public class CompositionTests {
        [Fact]
        public void One_In_parameter_One_Out_Parameter() {
            Func<string, string> func = x => x + x;
            var composition = func.Compose(s => s.Length);
            Assert.Equal(4, composition("fo"));
        }

        [Fact]
        public void Zero_In_parameter_One_Out_Parameter() {
            Func<string> func = () => "fo";
            var composition = func.Compose(s => s.Length);
            Assert.Equal(2, composition());
        }
    }
}