using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Addimagefilepathtousers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageFilePath",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
                columns: new[] { "ImageFilePath", "Passcode" },
                values: new object[] { "", "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0002",
                columns: new[] { "ImageFilePath", "Passcode" },
                values: new object[] { "", "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0003",
                columns: new[] { "ImageFilePath", "Passcode" },
                values: new object[] { "", "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0004",
                columns: new[] { "ImageFilePath", "Passcode" },
                values: new object[] { "", "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0005",
                columns: new[] { "ImageFilePath", "Passcode" },
                values: new object[] { "", "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: "U-0006",
                columns: new[] { "ImageFilePath", "Passcode" },
                values: new object[] { "", "zmrHo/UaWrsiGaayCzFZeL/K1Tk=;vCSanSzK/gvT2Dj70PdSbw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFilePath",
                table: "Users");

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
                column: "Passcode",
                value: "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==");

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
                column: "Passcode",
                value: "bvbGeuXHNenHZxdmJlfpFXe/ETs=;aPQbe8aFaDHNyL1AYNZZbw==");

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
    }
}
