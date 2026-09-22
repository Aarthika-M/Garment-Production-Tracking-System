using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GarmentProductionTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImageAndInstructionsToOrderAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerInstructions",
                schema: "aa",
                table: "tb_gp_orderass",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageContent",
                schema: "aa",
                table: "tb_gp_orderass",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerInstructions",
                schema: "aa",
                table: "tb_gp_orderass");

            migrationBuilder.DropColumn(
                name: "ImageContent",
                schema: "aa",
                table: "tb_gp_orderass");
        }
    }
}
