using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    public partial class IncreasedCharacteristicValueLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CharacteristicsNames",
                type: "nvarchar(120)",
                maxLength: 120,
                nullable: false,
                comment: "characteristic name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "characteristic name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CharacteristicsNames",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "characteristic name",
                oldClrType: typeof(string),
                oldType: "nvarchar(120)",
                oldMaxLength: 120,
                oldComment: "characteristic name");
        }
    }
}
