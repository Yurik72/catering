using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class Finance1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinances_AspNetUsers_Id",
                table: "UserFinances");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinances_AspNetUsers_Id",
                table: "UserFinances",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinances_AspNetUsers_Id",
                table: "UserFinances");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinances_AspNetUsers_Id",
                table: "UserFinances",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
