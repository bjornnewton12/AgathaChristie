using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgathaChristie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTVAdaptations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TVAdaptations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TmdbShowId = table.Column<int>(type: "integer", nullable: false),
                    SeriesName = table.Column<string>(type: "text", nullable: false),
                    EpisodeTitle = table.Column<string>(type: "text", nullable: true),
                    SeasonNumber = table.Column<int>(type: "integer", nullable: true),
                    EpisodeNumber = table.Column<int>(type: "integer", nullable: true),
                    ReleaseYear = table.Column<int>(type: "integer", nullable: false),
                    PosterImageUrl = table.Column<string>(type: "text", nullable: true),
                    BookId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TVAdaptations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TVAdaptations_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TVAdaptations_BookId",
                table: "TVAdaptations",
                column: "BookId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TVAdaptations");
        }
    }
}
