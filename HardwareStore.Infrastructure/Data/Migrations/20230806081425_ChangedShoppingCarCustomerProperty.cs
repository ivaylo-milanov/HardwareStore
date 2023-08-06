using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class ChangedShoppingCarCustomerProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_AspNetUsers_UserId",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_UserId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ShoppingCartItems");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "ShoppingCartItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                comment: "shopping cart item customer id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems",
                columns: new[] { "ProductId", "CustomerId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_CustomerId",
                table: "ShoppingCartItems",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_AspNetUsers_CustomerId",
                table: "ShoppingCartItems",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_AspNetUsers_CustomerId",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItems_CustomerId",
                table: "ShoppingCartItems");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "ShoppingCartItems");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ShoppingCartItems",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                comment: "shopping cart item user id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems",
                columns: new[] { "ProductId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_UserId",
                table: "ShoppingCartItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_AspNetUsers_UserId",
                table: "ShoppingCartItems",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
