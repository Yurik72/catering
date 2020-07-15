using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class UserGroupID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserGroupId",
                table: "UserGroups",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserGrpoupId",
                table: "UserGroups",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserGroupId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserGrpoupId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserGroupId",
                table: "UserGroups",
                column: "UserGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserGroupId",
                table: "AspNetUsers",
                column: "UserGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserGroups_UserGroupId",
                table: "AspNetUsers",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserGroups_UserGroups_UserGroupId",
                table: "UserGroups",
                column: "UserGroupId",
                principalTable: "UserGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserGroups_UserGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserGroups_UserGroups_UserGroupId",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_UserGroups_UserGroupId",
                table: "UserGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "UserGrpoupId",
                table: "UserGroups");

            migrationBuilder.DropColumn(
                name: "UserGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserGrpoupId",
                table: "AspNetUsers");
        }
    }
}
