using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class liqpay_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRejected",
                table: "UserFinIncomes",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRejected",
                table: "UserFinIncomes");
        }
    }
}
