using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SumterMartialArtsAzure.Server.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Instructors",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Instructors");
        }
    }
}
