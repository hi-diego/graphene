using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrapheneTemplate.Database.Migrations
{
    public partial class AddProductPreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_preferences",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    unit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    min = table.Column<double>(type: "double", nullable: false),
                    max = table.Column<double>(type: "double", nullable: false),
                    default_comment = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_by_id = table.Column<int>(type: "int", nullable: true),
                    buyer_id = table.Column<int>(type: "int", nullable: true),
                    seller_id = table.Column<int>(type: "int", nullable: true),
                    uid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_preferences", x => x.id);
                    table.ForeignKey(
                        name: "fk_product_preferences_companies_buyer_id",
                        column: x => x.buyer_id,
                        principalTable: "companies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_product_preferences_companies_seller_id",
                        column: x => x.seller_id,
                        principalTable: "companies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_product_preferences_user_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_product_preferences_buyer_id",
                table: "product_preferences",
                column: "buyer_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_preferences_created_by_id",
                table: "product_preferences",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_preferences_seller_id",
                table: "product_preferences",
                column: "seller_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_preferences");
        }
    }
}
