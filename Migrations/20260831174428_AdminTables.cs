using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Gamana_Muttopalvelu_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AdminTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer_reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Author = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_reviews", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "detailed_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detailed_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "form_service_options",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_service_options", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "key_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key_services", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pricing_packages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RatePerHour = table.Column<decimal>(type: "numeric", nullable: false),
                    IsPopular = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_packages", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "process_steps",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StepNumber = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process_steps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customer_review_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReviewId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
                    DateDisplay = table.Column<string>(type: "text", nullable: false),
                    ServiceUsed = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customer_review_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_customer_review_translations_customer_reviews_ReviewId",
                        column: x => x.ReviewId,
                        principalTable: "customer_reviews",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detailed_service_highlights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetailedServiceId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detailed_service_highlights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detailed_service_highlights_detailed_services_DetailedServi~",
                        column: x => x.DetailedServiceId,
                        principalTable: "detailed_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detailed_service_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DetailedServiceId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Subtitle = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detailed_service_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detailed_service_translations_detailed_services_DetailedSer~",
                        column: x => x.DetailedServiceId,
                        principalTable: "detailed_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "form_service_option_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OptionId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Label = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form_service_option_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_form_service_option_translations_form_service_options_Optio~",
                        column: x => x.OptionId,
                        principalTable: "form_service_options",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "key_service_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KeyServiceId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_key_service_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_key_service_translations_key_services_KeyServiceId",
                        column: x => x.KeyServiceId,
                        principalTable: "key_services",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pricing_package_features",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PricingPackageId = table.Column<int>(type: "integer", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_package_features", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pricing_package_features_pricing_packages_PricingPackageId",
                        column: x => x.PricingPackageId,
                        principalTable: "pricing_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pricing_package_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PricingPackageId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    PriceDisplay = table.Column<string>(type: "text", nullable: false),
                    Unit = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_package_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pricing_package_translations_pricing_packages_PricingPackag~",
                        column: x => x.PricingPackageId,
                        principalTable: "pricing_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "process_step_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProcessStepId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_process_step_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_process_step_translations_process_steps_ProcessStepId",
                        column: x => x.ProcessStepId,
                        principalTable: "process_steps",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detailed_service_highlight_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HighlightId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detailed_service_highlight_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_detailed_service_highlight_translations_detailed_service_hi~",
                        column: x => x.HighlightId,
                        principalTable: "detailed_service_highlights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "pricing_package_feature_translations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FeatureId = table.Column<int>(type: "integer", nullable: false),
                    LanguageCode = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pricing_package_feature_translations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_pricing_package_feature_translations_pricing_package_featur~",
                        column: x => x.FeatureId,
                        principalTable: "pricing_package_features",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_customer_review_translations_ReviewId_LanguageCode",
                table: "customer_review_translations",
                columns: new[] { "ReviewId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detailed_service_highlight_translations_HighlightId_Languag~",
                table: "detailed_service_highlight_translations",
                columns: new[] { "HighlightId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_detailed_service_highlights_DetailedServiceId",
                table: "detailed_service_highlights",
                column: "DetailedServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_detailed_service_translations_DetailedServiceId_LanguageCode",
                table: "detailed_service_translations",
                columns: new[] { "DetailedServiceId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_form_service_option_translations_OptionId_LanguageCode",
                table: "form_service_option_translations",
                columns: new[] { "OptionId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_key_service_translations_KeyServiceId_LanguageCode",
                table: "key_service_translations",
                columns: new[] { "KeyServiceId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pricing_package_feature_translations_FeatureId_LanguageCode",
                table: "pricing_package_feature_translations",
                columns: new[] { "FeatureId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pricing_package_features_PricingPackageId",
                table: "pricing_package_features",
                column: "PricingPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_pricing_package_translations_PricingPackageId_LanguageCode",
                table: "pricing_package_translations",
                columns: new[] { "PricingPackageId", "LanguageCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_process_step_translations_ProcessStepId_LanguageCode",
                table: "process_step_translations",
                columns: new[] { "ProcessStepId", "LanguageCode" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_review_translations");

            migrationBuilder.DropTable(
                name: "detailed_service_highlight_translations");

            migrationBuilder.DropTable(
                name: "detailed_service_translations");

            migrationBuilder.DropTable(
                name: "form_service_option_translations");

            migrationBuilder.DropTable(
                name: "key_service_translations");

            migrationBuilder.DropTable(
                name: "pricing_package_feature_translations");

            migrationBuilder.DropTable(
                name: "pricing_package_translations");

            migrationBuilder.DropTable(
                name: "process_step_translations");

            migrationBuilder.DropTable(
                name: "customer_reviews");

            migrationBuilder.DropTable(
                name: "detailed_service_highlights");

            migrationBuilder.DropTable(
                name: "form_service_options");

            migrationBuilder.DropTable(
                name: "key_services");

            migrationBuilder.DropTable(
                name: "pricing_package_features");

            migrationBuilder.DropTable(
                name: "process_steps");

            migrationBuilder.DropTable(
                name: "detailed_services");

            migrationBuilder.DropTable(
                name: "pricing_packages");
        }
    }
}
