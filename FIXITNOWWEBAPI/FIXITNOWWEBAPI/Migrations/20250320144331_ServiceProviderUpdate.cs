using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FIXITNOWWEBAPI.Migrations
{
    /// <inheritdoc />
    public partial class ServiceProviderUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProfilePicturePath",
                table: "ServiceProviders",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "PreferredServices",
                table: "ServiceProviders",
                newName: "ServicesOffered");

            migrationBuilder.RenameColumn(
                name: "IdCardPath",
                table: "ServiceProviders",
                newName: "ProfileImagePath");

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "ServiceProviders",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BusinessName",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IdCardImagePath",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "ServiceProviders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "BusinessName",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "IdCardImagePath",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "ServiceProviders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "ServiceProviders");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ServiceProviders",
                newName: "ProfilePicturePath");

            migrationBuilder.RenameColumn(
                name: "ServicesOffered",
                table: "ServiceProviders",
                newName: "PreferredServices");

            migrationBuilder.RenameColumn(
                name: "ProfileImagePath",
                table: "ServiceProviders",
                newName: "IdCardPath");
        }
    }
}
