using System;
using System.Linq;
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
            var result = MovieContext.Movies
                .Where(x => x.Id == Guid.Empty)
                .SingleOrError();

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(default(Movie), result.Either.Value);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Null(expected);
        }

        [Fact]
        public void Empty_Collection_Behaves_Like_SingleOrDefault_With_Predicate() {
            var expected = MovieContext.Movies
                .SingleOrDefault(x => x.Id == Guid.Empty);
            var result = MovieContext.Movies
                .SingleOrError(x => x.Id == Guid.Empty);

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(default(Movie), result.Either.Value);
            Assert.Equal(SingleOrErrorCase.NoElement, result.Either.Error);
            Assert.Null(expected);
        }

        [Fact]
        public void Multiple_Elements_Behaves_Like_SingleOrDefault_No_Predicate() {
            Assert.Throws<InvalidOperationException>(() => MovieContext.Movies.SingleOrDefault());
            var result = MovieContext.Movies.SingleOrError();

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(default(Movie), result.Either.Value);
            Assert.Equal(SingleOrErrorCase.ManyElements, result.Either.Error);
        }

        [Fact]
        public void Multiple_Elements_Behaves_Like_SingleOrDefault_With_Predicate() {
            Assert.Throws<InvalidOperationException>(() => MovieContext.Ratings.SingleOrDefault(y => y.Score == 2));
            var result = MovieContext.Ratings.SingleOrError(y => y.Score == 2);

            Assert.True(result.Either.HasError);
            Assert.False(result.Either.HasValue);
            Assert.Equal(default(Rating), result.Either.Value);
            Assert.Equal(SingleOrErrorCase.ManyElements, result.Either.Error);
        }

        [Fact]
        public void Single_Element_Behaves_Like_SingleOrDefault_With_Predicate() {
            var result = MovieContext.Users
                .SingleOrError(x => x.Email == "athanasios-radu@hotmail.com");
            Assert.False(result.Either.HasError);
            Assert.True(result.Either.HasValue);
            Assert.Equal(result.Either.Value, result.Either.Value);
            Assert.Equal(default(SingleOrErrorCase), result.Either.Error);
        }

        [Fact]
        public void Single_Element_Using_Take_Behaves_Like_SingleOrDefault_No_Predicate() {
            var result = MovieContext.Movies
                .Take(1)
                .SingleOrError();

            Assert.False(result.Either.HasError);
            Assert.True(result.Either.HasValue);
            Assert.Equal(result.Either.Value, result.Either.Value);
            Assert.Equal(default(SingleOrErrorCase), result.Either.Error);
        }

        [Fact]
        public void Single_Element_Using_Where_Behaves_Like_SingleOrDefault_No_Predicate() {
            var result = MovieContext.Users
                .Where(x => x.Email == "athanasios-radu@hotmail.com")
                .SingleOrError();
            Assert.False(result.Either.HasError);
            Assert.True(result.Either.HasValue);
            Assert.Equal(result.Either.Value, result.Either.Value);
            Assert.Equal(default(SingleOrErrorCase), result.Either.Error);
        }
    }
}