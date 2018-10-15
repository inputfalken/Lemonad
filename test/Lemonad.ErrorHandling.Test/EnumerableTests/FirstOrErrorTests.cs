using System.Linq;
using Lemonad.ErrorHandling.EnumerableExtensions;
using Xunit;

namespace Lemonad.ErrorHandling.Test.EnumerableTests {
    public class FirstOrErrorTests {
        [Fact]
        public void Predicate_Falsy__Expects_Error() {
            var result = Enumerable.Range(10, 2).FirstOrError(i => i == 2, () => "The integer 2 does not exist.");
            
            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal("The integer 2 does not exist.", result.Either.Error);
            Assert.Equal(default, result.Either.Value);
        } 
        
    }
}