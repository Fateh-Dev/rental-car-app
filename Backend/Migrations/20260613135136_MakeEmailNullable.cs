using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class MakeEmailNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // SQLite does not support ALTER COLUMN, so we recreate the table
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS ""Clients_backup"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Clients"" PRIMARY KEY AUTOINCREMENT,
                    ""FullName"" TEXT NOT NULL,
                    ""NationalId"" TEXT NOT NULL,
                    ""DateOfBirth"" TEXT NOT NULL,
                    ""LicenseNumber"" TEXT NOT NULL,
                    ""LicenseCategory"" TEXT NOT NULL,
                    ""LicenseIssueDate"" TEXT NOT NULL,
                    ""LicenseExpiryDate"" TEXT NOT NULL,
                    ""Phone"" TEXT NOT NULL,
                    ""Email"" TEXT NULL,
                    ""Address"" TEXT NOT NULL,
                    ""Notes"" TEXT NOT NULL
                );
                INSERT INTO ""Clients_backup"" SELECT
                    ""Id"", ""FullName"", ""NationalId"", ""DateOfBirth"",
                    ""LicenseNumber"", ""LicenseCategory"", ""LicenseIssueDate"", ""LicenseExpiryDate"",
                    ""Phone"",
                    CASE WHEN ""Email"" = '' THEN NULL ELSE ""Email"" END,
                    ""Address"", ""Notes""
                FROM ""Clients"";
                DROP TABLE ""Clients"";
                ALTER TABLE ""Clients_backup"" RENAME TO ""Clients"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert: make Email NOT NULL again (set null values to empty string)
            migrationBuilder.Sql(@"
                CREATE TABLE IF NOT EXISTS ""Clients_backup"" (
                    ""Id"" INTEGER NOT NULL CONSTRAINT ""PK_Clients"" PRIMARY KEY AUTOINCREMENT,
                    ""FullName"" TEXT NOT NULL,
                    ""NationalId"" TEXT NOT NULL,
                    ""DateOfBirth"" TEXT NOT NULL,
                    ""LicenseNumber"" TEXT NOT NULL,
                    ""LicenseCategory"" TEXT NOT NULL,
                    ""LicenseIssueDate"" TEXT NOT NULL,
                    ""LicenseExpiryDate"" TEXT NOT NULL,
                    ""Phone"" TEXT NOT NULL,
                    ""Email"" TEXT NOT NULL DEFAULT '',
                    ""Address"" TEXT NOT NULL,
                    ""Notes"" TEXT NOT NULL
                );
                INSERT INTO ""Clients_backup"" SELECT
                    ""Id"", ""FullName"", ""NationalId"", ""DateOfBirth"",
                    ""LicenseNumber"", ""LicenseCategory"", ""LicenseIssueDate"", ""LicenseExpiryDate"",
                    ""Phone"",
                    COALESCE(""Email"", ''),
                    ""Address"", ""Notes""
                FROM ""Clients"";
                DROP TABLE ""Clients"";
                ALTER TABLE ""Clients_backup"" RENAME TO ""Clients"";
            ");
        }
    }
}
