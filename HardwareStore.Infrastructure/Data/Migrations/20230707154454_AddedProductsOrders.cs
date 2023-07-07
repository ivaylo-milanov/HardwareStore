using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class AddedProductsOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "ProductsOrders",
            columns: table => new
            {
                OrderId = table.Column<int>(nullable: false),
                ProductId = table.Column<int>(nullable: false),
                Quantity = table.Column<int>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProductsOrders", x => new { x.OrderId, x.ProductId });
                table.ForeignKey("FK_ProductsOrders_Orders_OrderId", x => x.OrderId, "Orders", "Id", onDelete: ReferentialAction.Cascade);
                table.ForeignKey("FK_ProductsOrders_Products_ProductId", x => x.ProductId, "Products", "Id", onDelete: ReferentialAction.Cascade);
            });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
            name: "ProductsOrders");
        }
    }
}
