using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HardwareStore.Infrastructure.Data.Migrations
{
    /// <summary>
    /// Seeds the Admin role and a default administrator account (idempotent).
    /// Default sign-in after update: email admin@localhost, password ChangeMe!1.
    /// Change the password immediately in non-development environments.
    /// </summary>
    public partial class SeedAdminUserAndRole : Migration
    {
        private const string SeedAdminUserId = "11111111-1111-1111-1111-111111111111";

        private const string SeedAdminRoleId = "22222222-2222-2222-2222-222222222222";

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Password hash for "ChangeMe!1" (ASP.NET Core Identity 8, PasswordHasher for Customer).
            const string passwordHash =
                "AQAAAAIAAYagAAAAEJjT9nbU/Wva/riP2DPCz5UJuZwJOQPEQ7b6OyUXU7uZ3yjNhBXuBD5vjctU7v1OPg==";

            migrationBuilder.Sql($@"
IF NOT EXISTS (SELECT 1 FROM [AspNetRoles] WHERE [NormalizedName] = N'ADMIN')
BEGIN
    INSERT INTO [AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
    VALUES (N'{SeedAdminRoleId}', N'Admin', N'ADMIN', N'e3c1f5a9-8b7d-4e6c-9a1b-2d3e4f506172');
END

IF NOT EXISTS (SELECT 1 FROM [AspNetUsers] WHERE [NormalizedEmail] = N'ADMIN@LOCALHOST')
BEGIN
    INSERT INTO [AspNetUsers] (
        [Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed],
        [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed],
        [TwoFactorEnabled], [LockoutEnabled], [LockoutEnd], [AccessFailedCount],
        [FirstName], [LastName], [City], [Area], [Address])
    VALUES (
        N'{SeedAdminUserId}',
        N'admin@localhost',
        N'ADMIN@LOCALHOST',
        N'admin@localhost',
        N'ADMIN@LOCALHOST',
        1,
        N'{passwordHash}',
        N'f4e2d8c6-b5a3-4921-8f0e-9d8c7b6a5043',
        N'a9b8c7d6-e5f4-4321-a0b9-c8d7e6f50413',
        N'0000000000',
        0,
        0,
        1,
        NULL,
        0,
        N'Admin',
        N'User',
        N'N/A',
        N'N/A',
        N'N/A');
END

INSERT INTO [AspNetUserRoles] ([UserId], [RoleId])
SELECT u.[Id], r.[Id]
FROM [AspNetUsers] u
INNER JOIN [AspNetRoles] r ON r.[NormalizedName] = N'ADMIN'
WHERE u.[NormalizedEmail] = N'ADMIN@LOCALHOST'
AND NOT EXISTS (
    SELECT 1 FROM [AspNetUserRoles] ur
    WHERE ur.[UserId] = u.[Id] AND ur.[RoleId] = r.[Id]);
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
DELETE ur
FROM [AspNetUserRoles] ur
INNER JOIN [AspNetRoles] r ON ur.[RoleId] = r.[Id]
WHERE ur.[UserId] = N'{SeedAdminUserId}' AND r.[NormalizedName] = N'ADMIN';

DELETE FROM [AspNetUsers]
WHERE [Id] = N'{SeedAdminUserId}' AND [NormalizedEmail] = N'ADMIN@LOCALHOST';

DELETE FROM [AspNetRoles]
WHERE [Id] = N'{SeedAdminRoleId}' AND [NormalizedName] = N'ADMIN';
");
        }
    }
}
