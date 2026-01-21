using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumterMartialArtsAzure.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameStudentProgressionEventRecordTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserName",
                table: "Users",
                newName: "IX_Users_Username");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "AuditLogs",
                newName: "Username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Username",
                table: "Users",
                newName: "IX_Users_UserName");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "AuditLogs",
                newName: "UserName");
        }
    }
}
