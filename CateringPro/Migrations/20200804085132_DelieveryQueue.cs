using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class DelieveryQueue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeliveryQueues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    TerminalId = table.Column<int>(nullable: false),
                    DayDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(maxLength: 100, nullable: true),
                    DishId = table.Column<int>(nullable: false),
                    DishCourse = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryQueues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeliveryQueues_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryQueues_CompanyId",
                table: "DeliveryQueues",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryQueues_DayDate_UserId_DishId",
                table: "DeliveryQueues",
                columns: new[] { "DayDate", "UserId", "DishId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryQueues");
        }
    }
}
