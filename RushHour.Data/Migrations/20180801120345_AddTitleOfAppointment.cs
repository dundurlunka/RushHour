using Microsoft.EntityFrameworkCore.Migrations;

namespace RushHour.Data.Migrations
{
    public partial class AddTitleOfAppointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Appointments",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Title_StartDateTime_EndDateTime",
                table: "Appointments",
                columns: new[] { "Title", "StartDateTime", "EndDateTime" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_Name",
                table: "Activities",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Appointments_Title_StartDateTime_EndDateTime",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Activities_Name",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Appointments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
