using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgathaChristie.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SplitUserBookOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsOwned",
                table: "UserBooks",
                newName: "IsOwnedEnglish");

            migrationBuilder.AddColumn<bool>(
                name: "IsOwnedSwedish",
                table: "UserBooks",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOwnedSwedish",
                table: "UserBooks");

            migrationBuilder.RenameColumn(
                name: "IsOwnedEnglish",
                table: "UserBooks",
                newName: "IsOwned");
        }
    }
}
