using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class Finance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ChildNameSurname",
                table: "AspNetUsers",
                maxLength: 40,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UserFinances",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOutCome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalOrders = table.Column<int>(nullable: false),
                    TotalPreOrders = table.Column<int>(nullable: false),
                    TotalPreOrderedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LastUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFinances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserFinances_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserFinIncomes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    IncomeType = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TransactionData = table.Column<string>(maxLength: 200, nullable: true),
                    Comments = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFinIncomes", x => new { x.Id, x.TransactionDate });
                    table.ForeignKey(
                        name: "FK_UserFinIncomes_UserFinances_Id",
                        column: x => x.Id,
                        principalTable: "UserFinances",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserFinOutComes",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 100, nullable: false),
                    DayDate = table.Column<DateTime>(type: "date", nullable: false),
                    OutComeType = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ItemsCount = table.Column<int>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    Comments = table.Column<string>(maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFinOutComes", x => new { x.Id, x.DayDate });
                    table.ForeignKey(
                        name: "FK_UserFinOutComes_UserFinances_Id",
                        column: x => x.Id,
                        principalTable: "UserFinances",
                        principalColumn: "Id");
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserFinIncomes");

            migrationBuilder.DropTable(
                name: "UserFinOutComes");

            migrationBuilder.DropTable(
                name: "UserFinances");

            migrationBuilder.AlterColumn<string>(
                name: "ChildNameSurname",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 40,
                oldNullable: true);
        }
    }
}
