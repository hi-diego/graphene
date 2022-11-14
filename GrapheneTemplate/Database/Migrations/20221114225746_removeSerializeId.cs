using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrapheneTemplate.Database.Migrations
{
    public partial class removeSerializeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serialize_id",
                table: "blogs");

            migrationBuilder.DropColumn(
                name: "serialize_id",
                table: "authors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "serialize_id",
                table: "blogs",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "serialize_id",
                table: "authors",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
