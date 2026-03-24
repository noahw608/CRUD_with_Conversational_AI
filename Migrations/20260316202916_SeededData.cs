using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ConversationalAI.Migrations
{
    /// <inheritdoc />
    public partial class SeededData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CartId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Percentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sales_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "Id", "CreatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 14, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 3, 15, 14, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[,]
                {
                    { 1, "Gadgets, devices, and accessories", "Electronics" },
                    { 2, "Apparel for all ages and styles", "Clothing" },
                    { 3, "Everything for your living space", "Home & Garden" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Price", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, 1, "Over-ear noise cancelling headphones", 89.99m, 40, "Wireless Headphones" },
                    { 2, 1, "Tenkeyless with brown switches", 119.99m, 25, "Mechanical Keyboard" },
                    { 3, 1, "7-in-1 multiport adapter", 34.99m, 60, "USB-C Hub" },
                    { 4, 1, "HD webcam with built-in mic", 59.99m, 35, "Webcam 1080p" },
                    { 5, 2, "100% organic cotton t-shirt", 19.99m, 100, "Classic White Tee" },
                    { 6, 2, "Stretch chino trousers", 44.99m, 50, "Slim Chinos" },
                    { 7, 2, "Lightweight packable puffer", 89.99m, 30, "Puffer Jacket" },
                    { 8, 2, "Moisture-wicking ankle socks", 12.99m, 120, "Running Socks 3-Pack" },
                    { 9, 3, "Minimalist pot with drainage hole", 14.99m, 75, "Ceramic Plant Pot" },
                    { 10, 3, "Extra-large with juice groove", 24.99m, 45, "Bamboo Cutting Board" },
                    { 11, 3, "Adjustable arm with USB charging", 39.99m, 55, "LED Desk Lamp" },
                    { 12, 3, "Set of 3 soy wax candles", 29.99m, 80, "Scented Candle Set" }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "CategoryId", "EndDate", "Percentage", "ProductId", "StartDate" },
                values: new object[] { 2, 2, new DateTime(2026, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), 15m, null, new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "Id", "CartId", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 5, 2 },
                    { 3, 2, 2, 1 },
                    { 4, 2, 6, 1 },
                    { 5, 2, 12, 3 }
                });

            migrationBuilder.InsertData(
                table: "Sales",
                columns: new[] { "Id", "CategoryId", "EndDate", "Percentage", "ProductId", "StartDate" },
                values: new object[] { 1, null, new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), 20m, 1, new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CategoryId",
                table: "Sales",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_ProductId",
                table: "Sales",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
