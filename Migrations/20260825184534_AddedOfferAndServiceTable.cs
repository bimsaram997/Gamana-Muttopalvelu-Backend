using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gamana_Muttopalvelu_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddedOfferAndServiceTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OfferId",
                table: "Addresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    DesiredMovingDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdditionalInfo = table.Column<string>(type: "text", nullable: true),
                    PrivacyAgreed = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RequestedServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OfferId = table.Column<Guid>(type: "uuid", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false),
                    ServiceName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestedServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestedServices_Offers_OfferId",
                        column: x => x.OfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_OfferId",
                table: "Addresses",
                column: "OfferId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_UserId",
                table: "Offers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestedServices_OfferId",
                table: "RequestedServices",
                column: "OfferId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Offers_OfferId",
                table: "Addresses",
                column: "OfferId",
                principalTable: "Offers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Offers_OfferId",
                table: "Addresses");

            migrationBuilder.DropTable(
                name: "RequestedServices");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_OfferId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "OfferId",
                table: "Addresses");
        }
    }
}
