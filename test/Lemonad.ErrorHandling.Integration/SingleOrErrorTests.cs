using System;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result.Enumerable;
using Lemonad.ErrorHandling.Integration.EntityFramework;
using Xunit;

namespace Lemonad.ErrorHandling.Integration {
    public class SingleOrErrorTests {
        static SingleOrErrorTests() {
            MovieContext = new MovieContext();
            new MovieDatabaseManager(MovieContext).BuildIntegrationTestDatabase(
                () => MovieContext.Users.Count() == 6667
            );
        }

        private static MovieContext MovieContext { get; }

        [Fact]
        public void Empty_Collection_Behaves_Like_SingleOrDefault_No_Predicate() {
            var expected = MovieContext.Movies
                .SingleOrDefault(x => x.Id == Guid.Empty);
            MovieContext.Movies
                .Where(x => x.Id == Guid.Empty)
                .SingleOrError()
                .AssertError(SingleOrErrorCase.NoElement);

            Assert.Null(expected);
        }

        [Fact]
        public void Empty_Collection_Behaves_Like_SingleOrDefault_With_Predicate() {
            var expected = MovieContext.Movies
                .SingleOrDefault(x => x.Id == Guid.Empty);
            MovieContext.Movies
                .SingleOrError(x => x.Id == Guid.Empty)
                .AssertError(SingleOrErrorCase.NoElement);

            Assert.Null(expected);
        }

        [Fact]
        public void Multiple_Elements_Behaves_Like_SingleOrDefault_No_Predicate() {
            Assert.Throws<InvalidOperationException>(() => MovieContext.Movies.SingleOrDefault());
            MovieContext.Movies.SingleOrError().AssertError(SingleOrErrorCase.ManyElements);
        }

        [Fact]
        public void Multiple_Elements_Behaves_Like_SingleOrDefault_With_Predicate() {
            Assert.Throws<InvalidOperationException>(() => MovieContext.Ratings.SingleOrDefault(y => y.Score == 2));
            MovieContext.Ratings
                .SingleOrError(y => y.Score == 2)
                .AssertError(SingleOrErrorCase.ManyElements);
        }

        [Fact]
        public void Single_Element_Behaves_Like_SingleOrDefault_With_Predicate() {
            MovieContext.Users
                .SingleOrError(x => x.Email == "athanasios-radu@hotmail.com")
                .AssertValue(MovieContext.Users.SingleOrDefault(x => x.Email == "athanasios-radu@hotmail.com"));
        }

        [Fact]
        public void Single_Element_Using_Take_Behaves_Like_SingleOrDefault_No_Predicate() {
            MovieContext.Movies
                .Take(1)
                .SingleOrError()
                .AssertValue(MovieContext.Movies.Take(1).SingleOrDefault());
        }

        [Fact]
        public void Single_Element_Using_Where_Behaves_Like_SingleOrDefault_No_Predicate() {
            MovieContext.Users
                .Where(x => x.Email == "athanasios-radu@hotmail.com")
                .SingleOrError()
                .AssertValue(MovieContext.Users.SingleOrDefault(x => x.Email == "athanasios-radu@hotmail.com"));
        }
    }
}