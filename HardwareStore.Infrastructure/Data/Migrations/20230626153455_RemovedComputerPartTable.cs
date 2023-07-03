using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class RemovedComputerPartTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerParts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerParts",
                columns: table => new
                {
                    PartId = table.Column<int>(type: "int", nullable: false, comment: "computer part id"),
                    ComputerId = table.Column<int>(type: "int", nullable: false, comment: "computer id")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerParts", x => new { x.PartId, x.ComputerId });
                    table.ForeignKey(
                        name: "FK_ComputerParts_Products_ComputerId",
                        column: x => x.ComputerId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComputerParts_Products_PartId",
                        column: x => x.PartId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "computer part table");

            migrationBuilder.CreateIndex(
                name: "IX_ComputerParts_ComputerId",
                table: "ComputerParts",
                column: "ComputerId");
        }
    }
}
