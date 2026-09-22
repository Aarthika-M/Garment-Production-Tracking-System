using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GarmentProductionTracking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "aa");

            migrationBuilder.CreateTable(
                name: "tb_gp_users",
                schema: "aa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gp_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_gp_orders",
                schema: "aa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GarmentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeliveryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ImageContent = table.Column<byte[]>(type: "bytea", nullable: true),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    CustomerInstructions = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gp_orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_gp_orders_tb_gp_users_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "aa",
                        principalTable: "tb_gp_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_gp_orderass",
                schema: "aa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ManagerId = table.Column<int>(type: "integer", nullable: false),
                    WorkerId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_gp_orderass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_gp_orderass_tb_gp_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "aa",
                        principalTable: "tb_gp_orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_gp_orderass_tb_gp_users_ManagerId",
                        column: x => x.ManagerId,
                        principalSchema: "aa",
                        principalTable: "tb_gp_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_gp_orderass_tb_gp_users_WorkerId",
                        column: x => x.WorkerId,
                        principalSchema: "aa",
                        principalTable: "tb_gp_users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_gp_orderass_ManagerId",
                schema: "aa",
                table: "tb_gp_orderass",
                column: "ManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gp_orderass_OrderId",
                schema: "aa",
                table: "tb_gp_orderass",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gp_orderass_WorkerId",
                schema: "aa",
                table: "tb_gp_orderass",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_tb_gp_orders_CustomerId",
                schema: "aa",
                table: "tb_gp_orders",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_gp_orderass",
                schema: "aa");

            migrationBuilder.DropTable(
                name: "tb_gp_orders",
                schema: "aa");

            migrationBuilder.DropTable(
                name: "tb_gp_users",
                schema: "aa");
        }
    }
}
