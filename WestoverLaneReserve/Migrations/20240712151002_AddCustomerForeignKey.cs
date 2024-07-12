using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WestoverLaneReserve.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "LaneReservation",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.CreateIndex(
                name: "IX_LaneReservation_CustomerId",
                table: "LaneReservation",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LaneReservation_AspNetUsers_CustomerId",
                table: "LaneReservation",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaneReservation_AspNetUsers_CustomerId",
                table: "LaneReservation");

            migrationBuilder.DropIndex(
                name: "IX_LaneReservation_CustomerId",
                table: "LaneReservation");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "LaneReservation",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");
        }
    }
}
