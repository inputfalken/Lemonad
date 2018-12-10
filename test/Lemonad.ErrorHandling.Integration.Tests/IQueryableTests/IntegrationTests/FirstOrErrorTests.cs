using System.Linq;
using DatabaseManager;
using EntityFramework;
using Lemonad.ErrorHandling.EnumerableFunctions;
using Xunit;

namespace IntegrationTests {
    public class FirstOrErrorTests {
        private static MovieContext MovieContext { get; }

        static FirstOrErrorTests() {
            MovieContext = new MovieContext();
            new MovieDatabaseManager(MovieContext).BuildIntegrationTestDatabase(
                skipCreationWhenDatabaseExistsAnd: () => MovieContext.Users.Count() == 6667
            );
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_Without_Predicate() {
            var expected = MovieContext.Users.FirstOrDefault();
            var result = MovieContext.Users.FirstOrError(() => "Could not find any user.");

            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(expected, result.Either.Value);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_On_Empty_IQueryable_Without_Predicate() {
            var expected = MovieContext.Users
                .Where(x => x.Email == string.Empty)
                .FirstOrDefault();
            var result = MovieContext.Users
                .Where(x => x.Email == string.Empty)
                .FirstOrError(() => "Could not find any user.");

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal("Could not find any user.", result.Either.Error);
            Assert.Null(expected);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_True_Predicate() {
            const string title = "Fury";
            var expected = MovieContext.Movies.FirstOrDefault(x => x.Title == title);
            var result = MovieContext.Movies.FirstOrError(
                x => x.Title == title,
                () => $"Could not find a movie with the name '{title}'."
            );

            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(expected, result.Either.Value);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_False_Predicate() {
            const string title = "test";
            var expected = MovieContext.Movies.FirstOrDefault(x => x.Title == title);
            var result = MovieContext.Movies.FirstOrError(
                x => x.Title == title,
                () => $"Could not find a movie with the name '{title}'."
            );

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Null(expected);
            Assert.Equal($"Could not find a movie with the name '{title}'.", result.Either.Error);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_False_Predicate_Using_Value_Type() {
            const int score = 20;
            var result = MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrError(
                    x => x == score,
                    () => $"Could not find a score that's equal to '{score}'."
                );

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal($"Could not find a score that's equal to '{score}'.", result.Either.Error);
        }

        [Fact]
        public void Look_For_ValueType_Equal_To_Default_Value() {
            const int score = 0;
            var firstOrDefault = MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrDefault(i => i == 0);

            // There's no scores equal to 0 available in the database.
            // Using FirstOrDefault would give a 0 even tho it does not exist.
            // Meanwhile this implementation understands that an error should be created instead of giving a value with 0.
            var result = MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrError(
                    x => x == score,
                    () => $"Could not find a score that's equal to '{score}'."
                );

            Assert.False(result.Either.HasValue);
            Assert.True(result.Either.HasError);
            Assert.Equal($"Could not find a score that's equal to '{score}'.", result.Either.Error);
            Assert.Equal(0, firstOrDefault);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_True_Predicate_Using_Value_Type() {
            const int score = 5;
            var result = MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrError(
                    x => x == score,
                    () => $"Could not find a score that's equal to '{score}'."
                );

            Assert.True(result.Either.HasValue);
            Assert.False(result.Either.HasError);
            Assert.Equal(score, result.Either.Value);
        }
    }
}