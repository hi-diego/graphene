using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrapheneTemplate.Database.Migrations
{
    public partial class AddProductPreferenceProductId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_preferences_companies_seller_id",
                table: "product_preferences");

            migrationBuilder.RenameColumn(
                name: "seller_id",
                table: "product_preferences",
                newName: "product_id");

            migrationBuilder.RenameIndex(
                name: "IX_product_preferences_seller_id",
                table: "product_preferences",
                newName: "IX_product_preferences_product_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "uid",
                table: "product_preferences",
                type: "char(36)",
                nullable: false,
                defaultValueSql: "(uuid())",
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_product_preferences_id",
                table: "product_preferences",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_preferences_uid",
                table: "product_preferences",
                column: "uid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_product_preferences_products_product_id",
                table: "product_preferences",
                column: "product_id",
                principalTable: "products",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_product_preferences_products_product_id",
                table: "product_preferences");

            migrationBuilder.DropIndex(
                name: "IX_product_preferences_id",
                table: "product_preferences");

            migrationBuilder.DropIndex(
                name: "IX_product_preferences_uid",
                table: "product_preferences");

            migrationBuilder.RenameColumn(
                name: "product_id",
                table: "product_preferences",
                newName: "seller_id");

            migrationBuilder.RenameIndex(
                name: "IX_product_preferences_product_id",
                table: "product_preferences",
                newName: "IX_product_preferences_seller_id");

            migrationBuilder.AlterColumn<Guid>(
                name: "uid",
                table: "product_preferences",
                type: "char(36)",
                nullable: false,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldDefaultValueSql: "(uuid())")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "fk_product_preferences_companies_seller_id",
                table: "product_preferences",
                column: "seller_id",
                principalTable: "companies",
                principalColumn: "id");
        }
    }
}
