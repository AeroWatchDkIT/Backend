using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectPlacementscolumntousersv2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 16, 48, 8, 159, DateTimeKind.Utc).AddTicks(945));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 16, 48, 8, 159, DateTimeKind.Utc).AddTicks(950));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0001",
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 999, "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                column: "Passcode",
                value: "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 3, "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                column: "Passcode",
                value: "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                column: "Passcode",
                value: "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                column: "Passcode",
                value: "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 16, 46, 34, 785, DateTimeKind.Utc).AddTicks(2977));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 16, 46, 34, 785, DateTimeKind.Utc).AddTicks(2984));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0001",
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 0, "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                column: "Passcode",
                value: "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 0, "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                column: "Passcode",
                value: "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                column: "Passcode",
                value: "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                column: "Passcode",
                value: "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==");
        }
    }
}
