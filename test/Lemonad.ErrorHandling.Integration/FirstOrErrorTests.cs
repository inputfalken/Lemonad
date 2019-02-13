using System;
using System.Linq;
using Assertion;
using Lemonad.ErrorHandling.Extensions.Result.Queryable;
using Lemonad.ErrorHandling.Integration.EntityFramework;
using Xunit;

namespace Lemonad.ErrorHandling.Integration {
    public class FirstOrErrorTests {
        static FirstOrErrorTests() {
            MovieContext = new MovieContext();
            new MovieDatabaseManager(MovieContext).BuildIntegrationTestDatabase(
                () => MovieContext.Users.Count() == 6667
            );
        }

        private static MovieContext MovieContext { get; }

        [Fact]
        public void Behaves_Like_FirstOrDefault_On_Empty_IQueryable_Without_Predicate() {
            var expected = MovieContext.Users
                .Where(x => x.Email == string.Empty)
                .FirstOrDefault();
            MovieContext.Users
                .Where(x => x.Email == string.Empty)
                .FirstOrError(() => "Could not find any user.")
                .AssertError("Could not find any user.");

            Assert.Null(expected);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_False_Predicate() {
            const string title = "test";
            var expected = MovieContext.Movies.FirstOrDefault(x => x.Title == title);
            MovieContext.Movies
                .FirstOrError(
                    x => x.Title == title,
                    () => $"Could not find a movie with the name '{title}'."
                )
                .AssertError($"Could not find a movie with the name '{title}'.");

            Assert.Null(expected);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_False_Predicate_Using_Value_Type() {
            const int score = 20;
            MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrError(
                    x => x == score,
                    () => $"Could not find a score that's equal to '{score}'."
                )
                .AssertError($"Could not find a score that's equal to '{score}'.");
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_True_Predicate() {
            const string title = "Fury";
            var expected = MovieContext.Movies.FirstOrDefault(x => x.Title == title);
            MovieContext.Movies.FirstOrError(
                x => x.Title == title,
                () => $"Could not find a movie with the name '{title}'."
            ).AssertValue(expected);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_With_True_Predicate_Using_Value_Type() {
            const int score = 5;
            MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrError(
                    x => x == score,
                    () => $"Could not find a score that's equal to '{score}'."
                )
                .AssertValue(score);
        }

        [Fact]
        public void Behaves_Like_FirstOrDefault_Without_Predicate() {
            var expected = MovieContext.Users.FirstOrDefault();
            MovieContext.Users
                .FirstOrError(() => "Could not find any user.")
                .AssertValue(expected);
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
            MovieContext.Ratings
                .Select(x => x.Score)
                .FirstOrError(
                    x => x == score,
                    () => $"Could not find a score that's equal to '{score}'."
                )
                .AssertError($"Could not find a score that's equal to '{score}'.");

            Assert.Equal(0, firstOrDefault);
        }

        [Fact]
        public void Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => MovieContext.Users.FirstOrError<User, string>(null)
            );

        [Fact]
        public void Passing_Null_Predicate_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.PredicateName,
                () => MovieContext.Users.FirstOrError(null, () => "")
            );

        [Fact]
        public void Passing_Null_Source_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IQueryable<string>) null).FirstOrError(() => "")
            );

        [Fact]
        public void Predicate_Overload_Passing_Null_ErrorSelector_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ErrorSelectorName,
                () => MovieContext.Users.FirstOrError<User, string>(u => true, null)
            );

        [Fact]
        public void Predicate_Overload_Passing_Null_Source_Throws()
            => Assert.Throws<ArgumentNullException>(
                AssertionUtilities.ExtensionParameterName,
                () => ((IQueryable<string>) null).FirstOrError(x => true, () => "")
            );
    }
}