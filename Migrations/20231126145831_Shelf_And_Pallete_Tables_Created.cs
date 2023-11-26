using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Shelf_And_Pallete_Tables_Created : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pallets",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pallets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shelves",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PalletId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shelves", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shelves_Pallets_PalletId",
                        column: x => x.PalletId,
                        principalTable: "Pallets",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Pallets",
                columns: new[] { "Id", "Location", "State" },
                values: new object[,]
                {
                    { "P-0001", "Warehouse A", 2 },
                    { "P-0002", "Warehouse A", 2 },
                    { "P-0003", "Warehouse A", 0 },
                    { "P-0004", "Warehouse A", 1 },
                    { "P-0005", "Warehouse A", 3 }
                });

            migrationBuilder.InsertData(
                table: "Shelves",
                columns: new[] { "Id", "Location", "PalletId" },
                values: new object[,]
                {
                    { "S-0003", "Warehouse A", null },
                    { "S-0004", "Warehouse A", null },
                    { "S-0005", "Warehouse A", null },
                    { "S-0001", "Warehouse A", "P-0001" },
                    { "S-0002", "Warehouse A", "P-0002" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_PalletId",
                table: "Shelves",
                column: "PalletId",
                unique: true,
                filter: "[PalletId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shelves");

            migrationBuilder.DropTable(
                name: "Pallets");
        }
    }
}
