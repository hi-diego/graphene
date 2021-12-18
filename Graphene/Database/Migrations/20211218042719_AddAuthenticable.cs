using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graphene.Database.Migrations
{
    public partial class AddAuthenticable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "password",
                table: "authors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "email",
                table: "authors");

            migrationBuilder.DropColumn(
                name: "password",
                table: "authors");
        }
    }
}
