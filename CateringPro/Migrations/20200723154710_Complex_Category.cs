using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class Complex_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriesId",
                table: "Complex",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Complex_CategoriesId",
                table: "Complex",
                column: "CategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complex_Categories_CategoriesId",
                table: "Complex",
                column: "CategoriesId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complex_Categories_CategoriesId",
                table: "Complex");

            migrationBuilder.DropIndex(
                name: "IX_Complex_CategoriesId",
                table: "Complex");

            migrationBuilder.DropColumn(
                name: "CategoriesId",
                table: "Complex");
        }
    }
}
