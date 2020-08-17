using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class primaryKeyUserFin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinIncomes_UserFinances_Id",
                table: "UserFinIncomes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFinOutComes_UserFinances_Id",
                table: "UserFinOutComes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFinances",
                table: "UserFinances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFinances",
                table: "UserFinances",
                columns: new[] { "Id", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFinOutComes_Id_CompanyId",
                table: "UserFinOutComes",
                columns: new[] { "Id", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFinIncomes_Id_CompanyId",
                table: "UserFinIncomes",
                columns: new[] { "Id", "CompanyId" });

            migrationBuilder.CreateIndex(
                name: "IX_UserFinances_Id",
                table: "UserFinances",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinIncomes_UserFinances_Id_CompanyId",
                table: "UserFinIncomes",
                columns: new[] { "Id", "CompanyId" },
                principalTable: "UserFinances",
                principalColumns: new[] { "Id", "CompanyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinOutComes_UserFinances_Id_CompanyId",
                table: "UserFinOutComes",
                columns: new[] { "Id", "CompanyId" },
                principalTable: "UserFinances",
                principalColumns: new[] { "Id", "CompanyId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserFinIncomes_UserFinances_Id_CompanyId",
                table: "UserFinIncomes");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFinOutComes_UserFinances_Id_CompanyId",
                table: "UserFinOutComes");

            migrationBuilder.DropIndex(
                name: "IX_UserFinOutComes_Id_CompanyId",
                table: "UserFinOutComes");

            migrationBuilder.DropIndex(
                name: "IX_UserFinIncomes_Id_CompanyId",
                table: "UserFinIncomes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserFinances",
                table: "UserFinances");

            migrationBuilder.DropIndex(
                name: "IX_UserFinances_Id",
                table: "UserFinances");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserFinances",
                table: "UserFinances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinIncomes_UserFinances_Id",
                table: "UserFinIncomes",
                column: "Id",
                principalTable: "UserFinances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserFinOutComes_UserFinances_Id",
                table: "UserFinOutComes",
                column: "Id",
                principalTable: "UserFinances",
                principalColumn: "Id");
        }
    }
}
