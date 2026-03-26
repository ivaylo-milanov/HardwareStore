using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    /// <summary>
    /// Creates full-text catalog and indexes when Full-Text Search is installed; otherwise no-ops (avoids error 7609 on LocalDB / some containers).
    /// The app uses LIKE for search; FTS is optional for future CONTAINS/FREETEXT.
    /// </summary>
    public partial class AddFullTextSearchCatalogAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Full-text DDL often cannot run inside a user transaction.
            migrationBuilder.Sql(
                """
                IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'product_catalog')
                        CREATE FULLTEXT CATALOG product_catalog;

                    IF NOT EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID(N'dbo.Products'))
                        CREATE FULLTEXT INDEX ON dbo.Products(Name, Description, Model)
                        KEY INDEX PK_Products
                        ON product_catalog
                        WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM);

                    IF NOT EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID(N'dbo.Manufacturers'))
                        CREATE FULLTEXT INDEX ON dbo.Manufacturers(Name)
                        KEY INDEX PK_Manufacturers
                        ON product_catalog
                        WITH (CHANGE_TRACKING = AUTO, STOPLIST = SYSTEM);
                END
                """,
                suppressTransaction: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                IF FULLTEXTSERVICEPROPERTY('IsFullTextInstalled') = 1
                BEGIN
                    IF EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID(N'dbo.Products'))
                        DROP FULLTEXT INDEX ON dbo.Products;

                    IF EXISTS (SELECT 1 FROM sys.fulltext_indexes WHERE object_id = OBJECT_ID(N'dbo.Manufacturers'))
                        DROP FULLTEXT INDEX ON dbo.Manufacturers;

                    IF EXISTS (SELECT 1 FROM sys.fulltext_catalogs WHERE name = N'product_catalog')
                        DROP FULLTEXT CATALOG product_catalog;
                END
                """,
                suppressTransaction: true);
        }
    }
}
