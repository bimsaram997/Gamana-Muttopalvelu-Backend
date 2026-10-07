using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamana_Muttopalvelu_Backend.Migrations
{
    /// <inheritdoc />
    public partial class NewRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SelectedPackageId",
                table: "Bookings",
                column: "SelectedPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_pricing_packages_SelectedPackageId",
                table: "Bookings",
                column: "SelectedPackageId",
                principalTable: "pricing_packages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_pricing_packages_SelectedPackageId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_SelectedPackageId",
                table: "Bookings");
        }
    }
}
