using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Created_Pallet_Tracking_Log : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PalletTrackingLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PalletId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PalletState = table.Column<int>(type: "int", nullable: false),
                    PalletLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForkliftId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PalletTrackingLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PalletTrackingLog_Forklifts_ForkliftId",
                        column: x => x.ForkliftId,
                        principalTable: "Forklifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PalletTrackingLog_Pallets_PalletId",
                        column: x => x.PalletId,
                        principalTable: "Pallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PalletTrackingLog_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PalletTrackingLog",
                columns: new[] { "Id", "Action", "DateTime", "ForkliftId", "PalletId", "PalletLocation", "PalletState", "UserId" },
                values: new object[,]
                {
                    { 1, "Forklift F-0012 placed pallet P-0001 on shelf S-0001 in Warehouse A by user U-0001", new DateTime(2024, 1, 21, 16, 3, 42, 638, DateTimeKind.Utc).AddTicks(8464), "F-0012", "P-0001", "Warehouse A", 2, "U-0001" },
                    { 2, "Forklift F-0007 placed pallet P-0002 on the floor in Warehouse B by user U-0002", new DateTime(2024, 1, 21, 16, 3, 42, 638, DateTimeKind.Utc).AddTicks(8467), "F-0007", "P-0002", "Warehouse B", 0, "U-0002" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PalletTrackingLog_ForkliftId",
                table: "PalletTrackingLog",
                column: "ForkliftId");

            migrationBuilder.CreateIndex(
                name: "IX_PalletTrackingLog_PalletId",
                table: "PalletTrackingLog",
                column: "PalletId");

            migrationBuilder.CreateIndex(
                name: "IX_PalletTrackingLog_UserId",
                table: "PalletTrackingLog",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PalletTrackingLog");
        }
    }
}
