using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Created_Forklift_Class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Forklifts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LastPalletId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Forklifts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Forklifts_Pallets_LastPalletId",
                        column: x => x.LastPalletId,
                        principalTable: "Pallets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Forklifts_Users_LastUserId",
                        column: x => x.LastUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Forklifts",
                columns: new[] { "Id", "LastPalletId", "LastUserId" },
                values: new object[,]
                {
                    { "F-0007", "P-0003", "U-0002" },
                    { "F-0012", "P-0003", "U-0001" },
                    { "F-0016", "P-0005", "U-0003" },
                    { "F-0205", "P-0001", "U-0003" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Forklifts_LastPalletId",
                table: "Forklifts",
                column: "LastPalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Forklifts_LastUserId",
                table: "Forklifts",
                column: "LastUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Forklifts");
        }
    }
}
