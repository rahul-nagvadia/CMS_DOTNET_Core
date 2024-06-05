using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeMgmtSystem.Migrations
{
    /// <inheritdoc />
    public partial class init7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailId",
                table: "Students");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EmailId",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
