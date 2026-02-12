using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SecondProj.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    cateName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Category__3213E83F57319421", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Register",
                columns: table => new
                {
                    userId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Register__CB9A1CFF17B3376E", x => x.userId);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    price = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    image = table.Column<string>(type: "character varying(225)", maxLength: 225, nullable: true),
                    categoryId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Product__3213E83F95D0B8D4", x => x.id);
                    table.ForeignKey(
                        name: "FK__Product__categor__286302EC",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    productId = table.Column<int>(type: "integer", nullable: false),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    tprice = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__3213E83F6A0EA0B0", x => x.id);
                    table.ForeignKey(
                        name: "FK__Cart__productId__2B3F6F97",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Cart__userId__2C3393D0",
                        column: x => x.userId,
                        principalTable: "Register",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userId = table.Column<int>(type: "integer", nullable: false),
                    productId = table.Column<int>(type: "integer", nullable: false),
                    shippingAddress = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    paymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tprice = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Order__3213E83F3A2778F6", x => x.id);
                    table.ForeignKey(
                        name: "FK__Order__productId__300424B4",
                        column: x => x.productId,
                        principalTable: "Product",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__Order__userId__2F10007B",
                        column: x => x.userId,
                        principalTable: "Register",
                        principalColumn: "userId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_productId",
                table: "Cart",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_userId",
                table: "Cart",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_productId",
                table: "Order",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_userId",
                table: "Order",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_categoryId",
                table: "Product",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "UQ__Register__AB6E616411E03C6E",
                table: "Register",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Register");

            migrationBuilder.DropTable(
                name: "Category");
        }
    }
}
