using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIXITNOWWEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLogin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserProfiles",
                newName: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserProfiles",
                newName: "UserId");
        }
    }
}
