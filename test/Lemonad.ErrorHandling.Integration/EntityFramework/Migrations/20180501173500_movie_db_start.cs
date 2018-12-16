using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Lemonad.ErrorHandling.Integration.EntityFramework.Migrations {
    public partial class movie_db_start : Migration {
        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                "Ratings");

            migrationBuilder.DropTable(
                "Movies");

            migrationBuilder.DropTable(
                "Users");
        }

        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.CreateTable(
                "Movies",
                table => new {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Movies", x => x.Id); });

            migrationBuilder.CreateTable(
                "Users",
                table => new {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });

            migrationBuilder.CreateTable(
                "Ratings",
                table => new {
                    Id = table.Column<Guid>(nullable: false),
                    MovieId = table.Column<Guid>(nullable: false),
                    Score = table.Column<int>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.ForeignKey(
                        "FK_Ratings_Movies_MovieId",
                        x => x.MovieId,
                        "Movies",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        "FK_Ratings_Users_UserId",
                        x => x.UserId,
                        "Users",
                        "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                "IX_Ratings_MovieId",
                "Ratings",
                "MovieId");

            migrationBuilder.CreateIndex(
                "IX_Ratings_UserId",
                "Ratings",
                "UserId");
        }
    }
}