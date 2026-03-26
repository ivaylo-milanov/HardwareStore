using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedPredefinedHardwareCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // CategoryGroup.Hardware = 0. AssemblySlot is set in migration AddCategoryAssemblySlot.
            // and satisfy CategoryNameMinLength (5) for admin forms.
            migrationBuilder.Sql(
                """
                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Processors')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Processors', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Graphics')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Graphics', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Memory')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Memory', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Power supplies')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Power supplies', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Motherboard')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Motherboard', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Cases')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Cases', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Internal drives')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Internal drives', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'CPU Cooler')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'CPU Cooler', 0);

                IF NOT EXISTS (SELECT 1 FROM [Categories] WHERE [Name] = N'Prebuilt systems')
                    INSERT INTO [Categories] ([Name], [Group]) VALUES (N'Prebuilt systems', 0);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DELETE FROM [c]
                FROM [Categories] AS [c]
                WHERE [c].[Name] IN (
                    N'Processors', N'Graphics', N'Memory', N'Power supplies', N'Motherboard',
                    N'Cases', N'Internal drives', N'CPU Cooler', N'Prebuilt systems')
                  AND NOT EXISTS (SELECT 1 FROM [Products] AS [p] WHERE [p].[CategoryId] = [c].[Id]);
                """);
        }
    }
}
