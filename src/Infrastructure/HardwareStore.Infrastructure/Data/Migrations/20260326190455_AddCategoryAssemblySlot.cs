using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryAssemblySlot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssemblySlot",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "assembly BOM slot filter for products in this category; None if not used for standard slots");

            migrationBuilder.Sql(
                """
                UPDATE [Categories] SET [AssemblySlot] = 1 WHERE [Name] = N'Processors';
                UPDATE [Categories] SET [AssemblySlot] = 2 WHERE [Name] = N'Graphics';
                UPDATE [Categories] SET [AssemblySlot] = 3 WHERE [Name] = N'Memory';
                UPDATE [Categories] SET [AssemblySlot] = 4 WHERE [Name] = N'Power supplies';
                UPDATE [Categories] SET [AssemblySlot] = 5 WHERE [Name] = N'Motherboard';
                UPDATE [Categories] SET [AssemblySlot] = 6 WHERE [Name] = N'Cases';
                UPDATE [Categories] SET [AssemblySlot] = 7 WHERE [Name] = N'Internal drives';
                UPDATE [Categories] SET [AssemblySlot] = 9 WHERE [Name] = N'CPU Cooler';
                UPDATE [Categories] SET [AssemblySlot] = 0 WHERE [Name] = N'Prebuilt systems';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssemblySlot",
                table: "Categories");
        }
    }
}
