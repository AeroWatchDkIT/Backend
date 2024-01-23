using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Add_DbSet_For_PalletTrackingLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 1, 23, 14, 43, 11, 982, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 1, 23, 14, 43, 11, 982, DateTimeKind.Utc).AddTicks(7131));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 1, 21, 16, 3, 42, 638, DateTimeKind.Utc).AddTicks(8464));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 1, 21, 16, 3, 42, 638, DateTimeKind.Utc).AddTicks(8467));
        }
    }
}
