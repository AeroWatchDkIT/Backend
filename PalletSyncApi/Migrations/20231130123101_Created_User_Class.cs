using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PalletSyncApi.Migrations
{
    /// <inheritdoc />
    public partial class Created_User_Class : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Passcode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ForkliftCertified = table.Column<bool>(type: "bit", nullable: false),
                    IncorrectPalletPlacements = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "FirstName", "ForkliftCertified", "IncorrectPalletPlacements", "LastName", "Passcode", "UserType" },
                values: new object[,]
                {
                    { "U-0001", "Kacper", true, 0, "Wroblewski", "245tbgt", 1 },
                    { "U-0002", "Nikita", true, 13, "Fedans", "245tbgt", 0 },
                    { "U-0003", "Teodor", true, 3, "Donchev", "245tbgt", 0 },
                    { "U-0004", "Vincent", false, 0, "Arellano", "245tbgt", 0 },
                    { "U-0005", "Kyle", false, 0, "McQuillan", "245tbgt", 0 },
                    { "U-0006", "Siya", false, 0, "Salekar", "245tbgt", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
