using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Sampleuserswithhashedpasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0001",
                column: "Passcode",
                value: "245tbgt");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                column: "Passcode",
                value: "245tbgt");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                column: "Passcode",
                value: "245tbgt");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                column: "Passcode",
                value: "245tbgt");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                column: "Passcode",
                value: "245tbgt");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                column: "Passcode",
                value: "245tbgt");
        }
    }
}
