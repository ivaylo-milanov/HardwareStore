using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class AddDateAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComputerParts_Products_ComputerId",
                table: "ComputerParts");

            migrationBuilder.DropForeignKey(
                name: "FK_ComputerParts_Products_PartId",
                table: "ComputerParts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Manufacturers_ManufacturerId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ManufacturerId",
                table: "Products",
                type: "int",
                nullable: true,
                comment: "product manufacturer id",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "product manufacturer id");

            migrationBuilder.AddColumn<DateTime>(
                name: "AddDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now,
                comment: "product add date");

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerParts_Products_ComputerId",
                table: "ComputerParts",
                column: "ComputerId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerParts_Products_PartId",
                table: "ComputerParts",
                column: "PartId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Manufacturers_ManufacturerId",
                table: "Products",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComputerParts_Products_ComputerId",
                table: "ComputerParts");

            migrationBuilder.DropForeignKey(
                name: "FK_ComputerParts_Products_PartId",
                table: "ComputerParts");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Manufacturers_ManufacturerId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AddDate",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ManufacturerId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "product manufacturer id",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "product manufacturer id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerParts_Products_ComputerId",
                table: "ComputerParts",
                column: "ComputerId",
                principalTable: "Products",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComputerParts_Products_PartId",
                table: "ComputerParts",
                column: "PartId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Manufacturers_ManufacturerId",
                table: "Products",
                column: "ManufacturerId",
                principalTable: "Manufacturers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
