using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class RemoveCharacteristicsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "CharacteristicsNames");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacteristicsNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "characteristic name id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false, comment: "characteristic name")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacteristicsNames", x => x.Id);
                },
                comment: "characteristic name table");

            migrationBuilder.CreateTable(
                name: "Characteristics",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "characteristic product id"),
                    CharacteristicNameId = table.Column<int>(type: "int", nullable: false, comment: "characteristic name id"),
                    Value = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false, comment: "characteristic value")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => new { x.ProductId, x.CharacteristicNameId });
                    table.ForeignKey(
                        name: "FK_Characteristics_CharacteristicsNames_CharacteristicNameId",
                        column: x => x.CharacteristicNameId,
                        principalTable: "CharacteristicsNames",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Characteristics_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                },
                comment: "characteristic table");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_CharacteristicNameId",
                table: "Characteristics",
                column: "CharacteristicNameId");
        }
    }
}
