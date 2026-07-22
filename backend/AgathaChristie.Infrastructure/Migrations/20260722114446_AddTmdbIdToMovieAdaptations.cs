using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgathaChristie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTmdbIdToMovieAdaptations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TmdbId",
                table: "MovieAdaptations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TmdbId",
                table: "MovieAdaptations");
        }
    }
}
