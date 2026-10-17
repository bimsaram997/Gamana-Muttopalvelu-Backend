using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamana_Muttopalvelu_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AdminNotesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "Bookings");
        }
    }
}
