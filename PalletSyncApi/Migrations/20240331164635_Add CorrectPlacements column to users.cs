using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrectPlacementscolumntousers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectPalletPlacements",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 0, "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==" });

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
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 0, "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 0, "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                columns: new[] { "CorrectPalletPlacements", "Passcode" },
                values: new object[] { 0, "FTL3ccjs/Pf9PZn4F2YbHlWohQs=;61l/vVP+ZMzjiRd6075LqA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorrectPalletPlacements",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 16, 38, 13, 838, DateTimeKind.Utc).AddTicks(5236));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 16, 38, 13, 838, DateTimeKind.Utc).AddTicks(5239));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0001",
                column: "Passcode",
                value: "Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                column: "Passcode",
                value: "Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                column: "Passcode",
                value: "Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                column: "Passcode",
                value: "Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                column: "Passcode",
                value: "Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                column: "Passcode",
                value: "Y0zSxFzAN8TnzhIFaKOpm+O2Zn4=;fvmCitU3kakymjgXVKxeiQ==");
        }
    }
}
