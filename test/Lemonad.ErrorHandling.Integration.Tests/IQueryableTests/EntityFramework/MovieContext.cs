using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EntityFramework {
    public class MovieContext : DbContext {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=MovieDb;Trusted_Connection=True;")
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Movie>().HasMany(typeof(Rating));
            modelBuilder.Entity<User>().HasMany(typeof(Rating));

            modelBuilder.Entity<Movie>().HasKey(x => x.Id);
            modelBuilder.Entity<Rating>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasKey(x => x.Id);
        }
    }

    public class Movie {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public override string ToString() => Title;
    }

    public class Rating {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public int Score { get; set; }
        public override string ToString() => Score.ToString();
    }

    public class User {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public override string ToString() => Email;
    }
}