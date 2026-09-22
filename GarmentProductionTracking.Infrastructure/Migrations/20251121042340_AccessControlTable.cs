using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarmentProductionTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AccessControlTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Permission",
                schema: "aa",
                table: "RolePermissions",
                newName: "Controller");

            migrationBuilder.RenameColumn(
                name: "CanAccess",
                schema: "aa",
                table: "RolePermissions",
                newName: "Enabled");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                schema: "aa",
                table: "RolePermissions",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                schema: "aa",
                table: "RolePermissions");

            migrationBuilder.RenameColumn(
                name: "Enabled",
                schema: "aa",
                table: "RolePermissions",
                newName: "CanAccess");

            migrationBuilder.RenameColumn(
                name: "Controller",
                schema: "aa",
                table: "RolePermissions",
                newName: "Permission");
        }
    }
}
