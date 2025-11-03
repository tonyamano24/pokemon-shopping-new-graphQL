using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace itsc_dotnet_practice.Migrations
{
    /// <inheritdoc />
    public partial class PokemonSeeder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ImageUrl", "Name", "Price", "Stock", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "Clothing", new DateTime(2025, 7, 16, 7, 17, 34, 917, DateTimeKind.Utc).AddTicks(9181), "Beautiful handmade Karen shirt with traditional patterns.", "https://example.com/images/karen-shirt.jpg", "Handwoven Karen Shirt", 49.99m, 25, new DateTime(2025, 7, 16, 7, 17, 34, 917, DateTimeKind.Utc).AddTicks(9181) },
                    { 2, "Accessories", new DateTime(2025, 7, 16, 7, 17, 34, 917, DateTimeKind.Utc).AddTicks(9185), "Natural cotton bag with traditional Karen embroidery.", "https://example.com/images/cotton-bag.jpg", "Embroidered Cotton Bag", 19.99m, 40, new DateTime(2025, 7, 16, 7, 17, 34, 917, DateTimeKind.Utc).AddTicks(9185) },
                    { 3, "Accessories", new DateTime(2025, 7, 16, 7, 17, 34, 917, DateTimeKind.Utc).AddTicks(9188), "Soft scarf with intricate hand embroidery, perfect for any season.", "https://example.com/images/scarf.jpg", "Handmade Scarf", 29.99m, 30, new DateTime(2025, 7, 16, 7, 17, 34, 917, DateTimeKind.Utc).AddTicks(9189) }
                });
        }
    }
}
