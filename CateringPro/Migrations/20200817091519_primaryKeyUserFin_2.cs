using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class primaryKeyUserFin_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinances_AspNetUsers_Id",
                table: "UserFinances");

            migrationBuilder.DropIndex(
                name: "IX_UserFinances_Id",
                table: "UserFinances");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUserId",
                table: "UserFinances",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserFinances_CompanyUserId",
                table: "UserFinances",
                column: "CompanyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinances_AspNetUsers_CompanyUserId",
                table: "UserFinances",
                column: "CompanyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinances_AspNetUsers_CompanyUserId",
                table: "UserFinances");

            migrationBuilder.DropIndex(
                name: "IX_UserFinances_CompanyUserId",
                table: "UserFinances");

            migrationBuilder.DropColumn(
                name: "CompanyUserId",
                table: "UserFinances");

            migrationBuilder.CreateIndex(
                name: "IX_UserFinances_Id",
                table: "UserFinances",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinances_AspNetUsers_Id",
                table: "UserFinances",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
