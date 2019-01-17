using Microsoft.EntityFrameworkCore.Migrations;

namespace RushHour.Data.Migrations
{
    public partial class ChangeActivityAppointmentToManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activities_Appointments_AppointmentId",
                table: "Activities");

            migrationBuilder.DropIndex(
                name: "IX_Activities_AppointmentId",
                table: "Activities");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Activities");

            migrationBuilder.CreateTable(
                name: "ActivityAppointment",
                columns: table => new
                {
                    ActivityId = table.Column<int>(nullable: false),
                    AppointmentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityAppointment", x => new { x.ActivityId, x.AppointmentId });
                    table.ForeignKey(
                        name: "FK_ActivityAppointment_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActivityAppointment_Appointments_AppointmentId",
                        column: x => x.AppointmentId,
                        principalTable: "Appointments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivityAppointment_AppointmentId",
                table: "ActivityAppointment",
                column: "AppointmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityAppointment");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Activities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Activities_AppointmentId",
                table: "Activities",
                column: "AppointmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activities_Appointments_AppointmentId",
                table: "Activities",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
