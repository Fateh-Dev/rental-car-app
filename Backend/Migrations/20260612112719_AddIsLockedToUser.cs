using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddIsLockedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FuelLogs");

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "FuelLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CostPerLiter = table.Column<decimal>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FuelType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    IsAnomaly = table.Column<bool>(type: "INTEGER", nullable: false),
                    KmValue = table.Column<int>(type: "INTEGER", nullable: false),
                    Liters = table.Column<decimal>(type: "TEXT", nullable: false),
                    StationName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    TotalCost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FuelLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FuelLogs_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FuelLogs_VehicleId",
                table: "FuelLogs",
                column: "VehicleId");
        }
    }
}
