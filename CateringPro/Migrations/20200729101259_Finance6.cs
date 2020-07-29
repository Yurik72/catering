using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class Finance6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "UserFinOutComes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "UserFinIncomes",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "UserFinances",
                nullable: false,
                
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_UserFinOutComes_CompanyId",
                table: "UserFinOutComes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFinIncomes_CompanyId",
                table: "UserFinIncomes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFinances_CompanyId",
                table: "UserFinances",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinances_Companies_CompanyId",
                table: "UserFinances",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinIncomes_Companies_CompanyId",
                table: "UserFinIncomes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinOutComes_Companies_CompanyId",
                table: "UserFinOutComes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinances_Companies_CompanyId",
                table: "UserFinances");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFinIncomes_Companies_CompanyId",
                table: "UserFinIncomes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFinOutComes_Companies_CompanyId",
                table: "UserFinOutComes");

            migrationBuilder.DropIndex(
                name: "IX_UserFinOutComes_CompanyId",
                table: "UserFinOutComes");

            migrationBuilder.DropIndex(
                name: "IX_UserFinIncomes_CompanyId",
                table: "UserFinIncomes");

            migrationBuilder.DropIndex(
                name: "IX_UserFinances_CompanyId",
                table: "UserFinances");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "UserFinOutComes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "UserFinIncomes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "UserFinances");
        }
    }
}
