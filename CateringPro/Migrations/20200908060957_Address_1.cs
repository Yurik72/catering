using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class Address_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (this.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite")
                return;
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 50, nullable: true,type:"nvarchar(50)"),
                    Name = table.Column<string>(maxLength: 256, nullable: true, type: "nvarchar(256)"),
                    AddressType = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 256, nullable: true, type: "nvarchar(256)"),
                    PhoneNumber = table.Column<string>(maxLength: 15, nullable: true, type: "nvarchar(15)"),
                    ZipCode = table.Column<string>(maxLength: 10, nullable: true, type: "nvarchar(10)"),
                    Country = table.Column<string>(maxLength: 15, nullable: true, type: "nvarchar(15)"),
                    City = table.Column<string>(maxLength: 25, nullable: true, type: "nvarchar(25)"),
                    Address1 = table.Column<string>(maxLength: 40, nullable: true, type: "nvarchar(40)"),
                    Address2 = table.Column<string>(maxLength: 40, nullable: true, type: "nvarchar(40)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
            /*
                        migrationBuilder.CreateTable(
                            name: "NotOrderedQueues",
                            columns: table => new
                            {
                                Id = table.Column<int>(nullable: false)
                                    .Annotation("Sqlite:Autoincrement", true),
                                CompanyId = table.Column<int>(nullable: false),
                                TerminalId = table.Column<int>(nullable: false),
                                DayDate = table.Column<DateTime>(nullable: false),
                                UserId = table.Column<string>(maxLength: 100, nullable: true),
                                CategoryId = table.Column<int>(nullable: false)
                            },
                            constraints: table =>
                            {
                                table.PrimaryKey("PK_NotOrderedQueues", x => x.Id);
                                table.ForeignKey(
                                    name: "FK_NotOrderedQueues_Companies_CompanyId",
                                    column: x => x.CompanyId,
                                    principalTable: "Companies",
                                    principalColumn: "Id",
                                    onDelete: ReferentialAction.Cascade);
                            });
            */
            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CompanyId",
                table: "Addresses",
                column: "CompanyId");
            /*
                        migrationBuilder.CreateIndex(
                            name: "IX_NotOrderedQueues_CompanyId",
                            table: "NotOrderedQueues",
                            column: "CompanyId");
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (this.ActiveProvider == "Microsoft.EntityFrameworkCore.Sqlite")
                return;
            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "NotOrderedQueues");
        }
    }
}
