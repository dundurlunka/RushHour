using Microsoft.EntityFrameworkCore.Migrations;

namespace RushHour.Data.Migrations
{
    public partial class ChangeDurationToLong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "Duration",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Duration",
                table: "Activities",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
