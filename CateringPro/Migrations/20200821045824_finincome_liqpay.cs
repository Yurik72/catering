using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class finincome_liqpay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {





            migrationBuilder.AlterColumn<string>(
                name: "TransactionData",
                table: "UserFinIncomes",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "UserFinIncomes",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "UserFinIncomes",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ProjectionAmount",
                table: "UserFinIncomes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ReturnCallBackData",
                table: "UserFinIncomes",
                maxLength: 3000,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "IngredientCategories",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Categories",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "UserFinIncomes");

            migrationBuilder.DropColumn(
                name: "ProjectionAmount",
                table: "UserFinIncomes");

            migrationBuilder.DropColumn(
                name: "ReturnCallBackData",
                table: "UserFinIncomes");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionData",
                table: "UserFinIncomes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "UserFinIncomes",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyUserId",
                table: "UserFinances",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "IngredientCategories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Categories",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

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
    }
}
