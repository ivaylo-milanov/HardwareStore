using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddProductAssemblyComponents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAssemblyComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "assembly component row id")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssemblyProductId = table.Column<int>(type: "int", nullable: false, comment: "assembled product id (parent)"),
                    ComponentProductId = table.Column<int>(type: "int", nullable: false, comment: "component product id"),
                    Role = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false, comment: "component slot role label e.g. CPU, GPU"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "units of this component in the assembly"),
                    SortOrder = table.Column<int>(type: "int", nullable: false, comment: "display order within the assembly")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAssemblyComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductAssemblyComponents_Products_AssemblyProductId",
                        column: x => x.AssemblyProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductAssemblyComponents_Products_ComponentProductId",
                        column: x => x.ComponentProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "product assembly bill of materials row");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssemblyComponents_AssemblyProductId",
                table: "ProductAssemblyComponents",
                column: "AssemblyProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAssemblyComponents_ComponentProductId",
                table: "ProductAssemblyComponents",
                column: "ComponentProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAssemblyComponents");
        }
    }
}
