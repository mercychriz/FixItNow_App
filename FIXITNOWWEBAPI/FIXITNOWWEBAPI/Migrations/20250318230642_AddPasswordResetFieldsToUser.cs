using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIXITNOWWEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordResetFieldsToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
    name: "PasswordResetToken",
    table: "Users",
    type: "nvarchar(max)",
    nullable: true);  // ✅ Correct!

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiryTime",
                table: "Users",
                type: "datetime2",
                nullable: true);  // ✅ Correct!

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiryTime",
                table: "Users");
        }
    }
}
