using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class AddedOnDeleteRestrictionsProductOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Orders_OrderId",
                table: "ProductsOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Products_ProductId",
                table: "ProductsOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Orders_OrderId",
                table: "ProductsOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Products_ProductId",
                table: "ProductsOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Orders_OrderId",
                table: "ProductsOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductsOrders_Products_ProductId",
                table: "ProductsOrders");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Orders_OrderId",
                table: "ProductsOrders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsOrders_Products_ProductId",
                table: "ProductsOrders",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
