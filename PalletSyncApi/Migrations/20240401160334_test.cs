using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 4, 1, 16, 3, 33, 987, DateTimeKind.Utc).AddTicks(3391));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 4, 1, 16, 3, 33, 987, DateTimeKind.Utc).AddTicks(3396));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0001",
                column: "Passcode",
                value: "DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                column: "Passcode",
                value: "DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                column: "Passcode",
                value: "DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                column: "Passcode",
                value: "DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                column: "Passcode",
                value: "DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                column: "Passcode",
                value: "DWTvsEKSdCWzEECgamQHh/Wu17k=;CDNDs2aWCX3QOUFSM480iQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 17, 8, 49, 677, DateTimeKind.Utc).AddTicks(2704));

            migrationBuilder.UpdateData(
                table: "PalletTrackingLog",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateTime",
                value: new DateTime(2024, 3, 31, 17, 8, 49, 677, DateTimeKind.Utc).AddTicks(2709));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0001",
                column: "Passcode",
                value: "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                column: "Passcode",
                value: "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                column: "Passcode",
                value: "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                column: "Passcode",
                value: "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                column: "Passcode",
                value: "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                column: "Passcode",
                value: "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==");
        }
    }
}
