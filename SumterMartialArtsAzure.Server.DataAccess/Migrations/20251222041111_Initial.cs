using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumterMartialArtsAzure.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateLessonRequests_Instructors_InstructorId",
                table: "PrivateLessonRequests");

            migrationBuilder.DropTable(
                name: "AvailabilityRules");

            migrationBuilder.RenameColumn(
                name: "ScheduledTimeRange_Start",
                table: "PrivateLessonRequests",
                newName: "RequestedStart");

            migrationBuilder.RenameColumn(
                name: "ScheduledTimeRange_End",
                table: "PrivateLessonRequests",
                newName: "RequestedEnd");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "PrivateLessonRequests",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PrivateLessonRequests",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "PrivateLessonRequests",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StudentEmail",
                table: "PrivateLessonRequests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "PrivateLessonRequests",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentPhone",
                table: "PrivateLessonRequests",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AvailabilityRules",
                table: "Instructors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateLessonRequests_Instructors_InstructorId",
                table: "PrivateLessonRequests",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateLessonRequests_Instructors_InstructorId",
                table: "PrivateLessonRequests");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PrivateLessonRequests");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "PrivateLessonRequests");

            migrationBuilder.DropColumn(
                name: "StudentEmail",
                table: "PrivateLessonRequests");

            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "PrivateLessonRequests");

            migrationBuilder.DropColumn(
                name: "StudentPhone",
                table: "PrivateLessonRequests");

            migrationBuilder.DropColumn(
                name: "AvailabilityRules",
                table: "Instructors");

            migrationBuilder.RenameColumn(
                name: "RequestedStart",
                table: "PrivateLessonRequests",
                newName: "ScheduledTimeRange_Start");

            migrationBuilder.RenameColumn(
                name: "RequestedEnd",
                table: "PrivateLessonRequests",
                newName: "ScheduledTimeRange_End");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PrivateLessonRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "AvailabilityRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DaysOfWeekSerialized = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    InstructorId = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvailabilityRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AvailabilityRules_Instructors_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Instructors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AvailabilityRules_InstructorId",
                table: "AvailabilityRules",
                column: "InstructorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateLessonRequests_Instructors_InstructorId",
                table: "PrivateLessonRequests",
                column: "InstructorId",
                principalTable: "Instructors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
