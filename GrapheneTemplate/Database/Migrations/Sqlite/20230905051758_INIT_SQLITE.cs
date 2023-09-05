using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrapheneTemplate.Database.Migrations.Sqlite
{
    public partial class INIT_SQLITE : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "spaces",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    capacity = table.Column<int>(type: "INTEGER", nullable: false),
                    configuration = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    uuid = table.Column<Guid>(type: "BLOB", nullable: false, defaultValueSql: "randomblob(16)"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modified_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_spaces", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    email = table.Column<string>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    uuid = table.Column<Guid>(type: "BLOB", nullable: false, defaultValueSql: "randomblob(16)"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modified_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "bills",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    space_id = table.Column<int>(type: "INTEGER", nullable: false),
                    type = table.Column<int>(type: "INTEGER", nullable: false),
                    total = table.Column<double>(type: "REAL", nullable: false),
                    uuid = table.Column<Guid>(type: "BLOB", nullable: false, defaultValueSql: "randomblob(16)"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modified_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bills", x => x.id);
                    table.ForeignKey(
                        name: "fk_bills_space_space_id",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bills_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    subcategory = table.Column<string>(type: "TEXT", nullable: true),
                    category = table.Column<string>(type: "TEXT", nullable: true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    price = table.Column<double>(type: "REAL", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    packaging = table.Column<string>(type: "TEXT", nullable: false),
                    available = table.Column<bool>(type: "INTEGER", nullable: false),
                    created_by_id = table.Column<int>(type: "INTEGER", nullable: true),
                    uuid = table.Column<Guid>(type: "BLOB", nullable: false, defaultValueSql: "randomblob(16)"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modified_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_user_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    role = table.Column<int>(type: "INTEGER", nullable: false),
                    space_id = table.Column<int>(type: "INTEGER", nullable: true),
                    user_id = table.Column<int>(type: "INTEGER", nullable: true),
                    uuid = table.Column<Guid>(type: "BLOB", nullable: false, defaultValueSql: "randomblob(16)"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modified_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_staff", x => x.id);
                    table.ForeignKey(
                        name: "fk_staff_spaces_space_id",
                        column: x => x.space_id,
                        principalTable: "spaces",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_staff_user_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    quantity = table.Column<double>(type: "REAL", nullable: false),
                    comments = table.Column<string>(type: "TEXT", nullable: false),
                    status = table.Column<int>(type: "INTEGER", nullable: false),
                    created_by_id = table.Column<int>(type: "INTEGER", nullable: true),
                    product_id = table.Column<int>(type: "INTEGER", nullable: true),
                    bill_id = table.Column<int>(type: "INTEGER", nullable: true),
                    uuid = table.Column<Guid>(type: "BLOB", nullable: false, defaultValueSql: "randomblob(16)"),
                    created_at = table.Column<DateTime>(type: "TEXT", nullable: false),
                    modified_at = table.Column<DateTime>(type: "TEXT", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_bills_bill_id",
                        column: x => x.bill_id,
                        principalTable: "bills",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_orders_product_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_orders_user_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_bills_id",
                table: "bills",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bills_space_id",
                table: "bills",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "IX_bills_user_id",
                table: "bills",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_bills_uuid",
                table: "bills",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_bill_id",
                table: "orders",
                column: "bill_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_created_by_id",
                table: "orders",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_id",
                table: "orders",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_orders_product_id",
                table: "orders",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_uuid",
                table: "orders",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_created_by_id",
                table: "products",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_id",
                table: "products",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_uuid",
                table: "products",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_spaces_id",
                table: "spaces",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_spaces_uuid",
                table: "spaces",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_id",
                table: "staff",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_staff_space_id",
                table: "staff",
                column: "space_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_user_id",
                table: "staff",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_staff_uuid",
                table: "staff",
                column: "uuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_id",
                table: "users",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_uuid",
                table: "users",
                column: "uuid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "bills");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "spaces");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
