using Microsoft.EntityFrameworkCore.Migrations;

namespace RushHour.Data.Migrations
{
    public partial class FixEntitiesNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_Appointment_AppointmentId",
                table: "Activity");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Users_UserId",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activity",
                table: "Activity");

            migrationBuilder.RenameTable(
                name: "Appointment",
                newName: "Appointments");

            migrationBuilder.RenameTable(
                name: "Activity",
                newName: "Activities");

            migrationBuilder.RenameIndex(
                name: "IX_Appointment_UserId",
                table: "Appointments",
                newName: "IX_Appointments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Activity_AppointmentId",
                table: "Activities",
                newName: "IX_Activities_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activities",
                table: "Activities",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Appointments_AppointmentId",
                table: "Activities",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Users_UserId",
                table: "Appointments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Appointments_AppointmentId",
                table: "Activities");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Users_UserId",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Appointments",
                table: "Appointments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Activities",
                table: "Activities");

            migrationBuilder.RenameTable(
                name: "Appointments",
                newName: "Appointment");

            migrationBuilder.RenameTable(
                name: "Activities",
                newName: "Activity");

            migrationBuilder.RenameIndex(
                name: "IX_Appointments_UserId",
                table: "Appointment",
                newName: "IX_Appointment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Activities_AppointmentId",
                table: "Activity",
                newName: "IX_Activity_AppointmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Appointment",
                table: "Appointment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Activity",
                table: "Activity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_Appointment_AppointmentId",
                table: "Activity",
                column: "AppointmentId",
                principalTable: "Appointment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Users_UserId",
                table: "Appointment",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
