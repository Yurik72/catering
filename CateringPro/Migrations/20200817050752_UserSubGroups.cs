using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class UserSubGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSubGroupId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSubGroups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    ParentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSubGroups_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubGroups_UserSubGroups_ParentId",
                        column: x => x.ParentId,
                        principalTable: "UserSubGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserSubGroupId",
                table: "AspNetUsers",
                column: "UserSubGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubGroups_CompanyId",
                table: "UserSubGroups",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSubGroups_ParentId",
                table: "UserSubGroups",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserSubGroups_UserSubGroupId",
                table: "AspNetUsers",
                column: "UserSubGroupId",
                principalTable: "UserSubGroups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserSubGroups_UserSubGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserSubGroups");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserSubGroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserSubGroupId",
                table: "AspNetUsers");
        }
    }
}
