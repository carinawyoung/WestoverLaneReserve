using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestoverLaneReserve.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimeSlotAvailabilityToUseStrings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Time",
                table: "TimeSlotAvailabilities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Time");

            migrationBuilder.AlterColumn<string>(
                name: "Date",
                table: "TimeSlotAvailabilities",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "Date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Time",
                table: "TimeSlotAvailabilities",
                type: "Time",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "TimeSlotAvailabilities",
                type: "Date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
