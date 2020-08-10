using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class UserCardTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPreOrderBalance",
                table: "UserFinances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "CookingTechnologie",
                table: "Dishes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardTag",
                table: "AspNetUsers",
                maxLength: 40,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CardTag",
                table: "AspNetUsers",
                column: "CardTag",
                unique: true,
                filter: "[CardTag] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CardTag",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TotalPreOrderBalance",
                table: "UserFinances");

            migrationBuilder.DropColumn(
                name: "CookingTechnologie",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CardTag",
                table: "AspNetUsers");
        }
    }
}
