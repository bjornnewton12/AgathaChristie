using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgathaChristie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMovieAdaptations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MovieAdaptations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    PosterImageUrl = table.Column<string>(type: "text", nullable: true),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieAdaptations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovieAdaptations_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MovieAdaptations_BookId",
                table: "MovieAdaptations",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovieAdaptations");
        }
    }
}
