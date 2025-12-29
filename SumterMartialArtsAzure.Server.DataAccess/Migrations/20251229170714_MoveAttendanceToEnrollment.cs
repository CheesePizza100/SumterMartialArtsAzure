using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumterMartialArtsAzure.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class MoveAttendanceToEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendance_Last30Days",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Attendance_Rate",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Attendance_Total",
                table: "Students");

            migrationBuilder.AddColumn<int>(
                name: "AttendanceRate",
                table: "StudentProgramEnrollment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Last30Days",
                table: "StudentProgramEnrollment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalClasses",
                table: "StudentProgramEnrollment",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttendanceRate",
                table: "StudentProgramEnrollment");

            migrationBuilder.DropColumn(
                name: "Last30Days",
                table: "StudentProgramEnrollment");

            migrationBuilder.DropColumn(
                name: "TotalClasses",
                table: "StudentProgramEnrollment");

            migrationBuilder.AddColumn<int>(
                name: "Attendance_Last30Days",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Attendance_Rate",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Attendance_Total",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
