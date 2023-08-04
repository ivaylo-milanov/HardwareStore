using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class DecreasedCharacteristicNameLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CharacteristicsNames",
                type: "nvarchar(60)",
                maxLength: 60,
                nullable: false,
                comment: "characteristic name",
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120,
                oldComment: "characteristic name");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Characteristics",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                comment: "characteristic value",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "characteristic value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CharacteristicsNames",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                comment: "characteristic name",
                oldClrType: typeof(string),
                oldType: "nvarchar(60)",
                oldMaxLength: 60,
                oldComment: "characteristic name");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Characteristics",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "characteristic value",
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120,
                oldComment: "characteristic value");
        }
    }
}
