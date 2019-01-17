using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RushHour.Data.Migrations
{
    public partial class RemoveEndDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_Title_StartDateTime_EndDateTime",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "EndDateTime",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "Duration",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Title_StartDateTime",
                table: "Appointments",
                columns: new[] { "Title", "StartDateTime" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_Title_StartDateTime",
                table: "Appointments");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateTime",
                table: "Appointments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<long>(
                name: "Duration",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Title_StartDateTime_EndDateTime",
                table: "Appointments",
                columns: new[] { "Title", "StartDateTime", "EndDateTime" },
                unique: true);
        }
    }
}
