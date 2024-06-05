using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeMgmtSystem.Migrations
{
    /// <inheritdoc />
    public partial class init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
