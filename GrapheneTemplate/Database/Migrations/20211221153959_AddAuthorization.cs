using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrapheneTemplate.Database.Migrations
{
    public partial class AddAuthorization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author_id = table.Column<int>(type: "int", nullable: false),
                    action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    denied = table.Column<bool>(type: "bit", nullable: false),
                    entity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    author_id1 = table.Column<int>(type: "int", nullable: true),
                    uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_permissions", x => x.id);
                    table.ForeignKey(
                        name: "fk_permissions_authors_author_id1",
                        column: x => x.author_id1,
                        principalTable: "authors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_permissions_author_id1",
                table: "permissions",
                column: "author_id1");

            migrationBuilder.CreateIndex(
                name: "IX_permissions_uid",
                table: "permissions",
                column: "uid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "permissions");
        }
    }
}
