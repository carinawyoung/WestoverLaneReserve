using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestoverLaneReserve.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateV5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeSlotAvailabilities",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Time = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LanesAvailable = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlotAvailabilities", x => new { x.Date, x.Time });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeSlotAvailabilities");
        }
    }
}
