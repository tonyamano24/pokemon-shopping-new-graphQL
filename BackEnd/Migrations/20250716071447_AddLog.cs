using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace itsc_dotnet_practice.Migrations
{
    /// <inheritdoc />
    public partial class AddLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 7, 14, 46, 560, DateTimeKind.Utc).AddTicks(3413), new DateTime(2025, 7, 16, 7, 14, 46, 560, DateTimeKind.Utc).AddTicks(3414) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 7, 14, 46, 560, DateTimeKind.Utc).AddTicks(3418), new DateTime(2025, 7, 16, 7, 14, 46, 560, DateTimeKind.Utc).AddTicks(3419) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 16, 7, 14, 46, 560, DateTimeKind.Utc).AddTicks(3422), new DateTime(2025, 7, 16, 7, 14, 46, 560, DateTimeKind.Utc).AddTicks(3422) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 14, 8, 5, 37, 820, DateTimeKind.Utc).AddTicks(7411), new DateTime(2025, 7, 14, 8, 5, 37, 820, DateTimeKind.Utc).AddTicks(7412) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 14, 8, 5, 37, 820, DateTimeKind.Utc).AddTicks(7414), new DateTime(2025, 7, 14, 8, 5, 37, 820, DateTimeKind.Utc).AddTicks(7415) });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 7, 14, 8, 5, 37, 820, DateTimeKind.Utc).AddTicks(7417), new DateTime(2025, 7, 14, 8, 5, 37, 820, DateTimeKind.Utc).AddTicks(7417) });
        }
    }
}
