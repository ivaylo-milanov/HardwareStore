using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class AddedCharacteristicNameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductsAttributes");

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.CreateTable(
                name: "CharacteristicsNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "characteristic name id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "characteristic name")
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
                    Id = table.Column<int>(type: "int", nullable: false, comment: "characteristic id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "characteristic product id"),
                    CharacteristicNameId = table.Column<int>(type: "int", nullable: false, comment: "characteristic name id"),
                    Value = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "characteristic value")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characteristics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characteristics_CharacteristicsNames_CharacteristicNameId",
                        column: x => x.CharacteristicNameId,
                        principalTable: "CharacteristicsNames",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Characteristics_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "characteristic table");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_CharacteristicNameId",
                table: "Characteristics",
                column: "CharacteristicNameId");

            migrationBuilder.CreateIndex(
                name: "IX_Characteristics_ProductId",
                table: "Characteristics",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Characteristics");

            migrationBuilder.DropTable(
                name: "CharacteristicsNames");

            migrationBuilder.CreateTable(
                name: "ProductsAttributes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "product attribute id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false, comment: "product attribute product id"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "product attribute key"),
                    Value = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "product attribute value")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductsAttributes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductsAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "product attribute table");

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Processor" },
                    { 2, "Mother Board" },
                    { 3, "Power Supply" },
                    { 4, "Case" },
                    { 5, "Hard Disk" },
                    { 6, "SSD" },
                    { 7, "RAM" },
                    { 8, "Processor Cooler" },
                    { 9, "Video Card" },
                    { 10, "Computer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductsAttributes_ProductId",
                table: "ProductsAttributes",
                column: "ProductId");
        }
    }
}
