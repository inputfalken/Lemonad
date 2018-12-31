using System;
using System.Collections.Generic;
using Lemonad.ErrorHandling.Integration.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Sharpy;
using Sharpy.Builder;
using Sharpy.Core.Linq;
using Xunit;

// If this is not disabled, all the connection pools will be used up for the database.
[assembly: CollectionBehavior(MaxParallelThreads = 2)]

namespace Lemonad.ErrorHandling.Integration {
    public interface IDatabaseManager {
        void CreateAndSeedDatabase(int seed, int records);
        bool DatabaseExists();
        bool DeleteDatabase();
    }

    public class MovieDatabaseManager : IDatabaseManager {
        private readonly MovieContext _movieContext;

        public MovieDatabaseManager(MovieContext movieContext) => _movieContext = movieContext;

        public void CreateAndSeedDatabase(int seed, int records) {
            _movieContext.Database.Migrate();
            var movies = new List<Movie> {
                new Movie {
                    Id = Guid.NewGuid(),
                    Title = "Batman Begins"
                },
                new Movie {
                    Id = Guid.NewGuid(),
                    Title = "Fury"
                },
                new Movie {
                    Id = Guid.NewGuid(),
                    Title = "Pirates of the Caribbean: The Curse of the Black Pearl"
                }
            };
            var builder = new Builder(new Configurement(new Random(seed)));
            var entities = builder
                .Generator(x => new {
                    User = new User {Email = x.Mail(x.FirstName(), x.LastName()), Id = Guid.NewGuid()},
                    Builder = x
                })
                .SelectMany(x => movies, (user, movie) => new {
                    user.User,
                    Movie = movie,
                    Rating = new Rating {
                        Id = Guid.NewGuid(),
                        MovieId = movie.Id,
                        UserId = user.User.Id,
                        Score = user.Builder.Integer(1, 10)
                    }
                }).ToList(records);

            // Add generated data.
            foreach (var entity in entities) {
                _movieContext.Movies.Add(entity.Movie);
                _movieContext.Users.Add(entity.User);

                // So not every user rates every movie
                if (builder.Integer() % 3 == 0)
                    _movieContext.Ratings.Add(entity.Rating);
            }

            _movieContext.SaveChanges();
        }

        public bool DatabaseExists() {
            var dbCreator = _movieContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            return dbCreator?.Exists() ??
                   throw new ArgumentException($"Argument does not implement {nameof(RelationalDatabaseCreator)}");
        }

        public bool DeleteDatabase() {
            var dbCreator = _movieContext.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            return dbCreator?.EnsureDeleted() ??
                   throw new ArgumentException($"Argument does not implement {nameof(RelationalDatabaseCreator)}");
        }
    }
}