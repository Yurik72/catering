using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class DishesKind : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DishKindId",
                table: "Complex",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DishesKind",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 10, nullable: true),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DishesKind", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DishesKind_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Complex_DishKindId",
                table: "Complex",
                column: "DishKindId");

            migrationBuilder.CreateIndex(
                name: "IX_DishesKind_CompanyId",
                table: "DishesKind",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Complex_DishesKind_DishKindId",
                table: "Complex",
                column: "DishKindId",
                principalTable: "DishesKind",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Complex_DishesKind_DishKindId",
                table: "Complex");

            migrationBuilder.DropTable(
                name: "DishesKind");

            migrationBuilder.DropIndex(
                name: "IX_Complex_DishKindId",
                table: "Complex");

            migrationBuilder.DropColumn(
                name: "DishKindId",
                table: "Complex");
        }
    }
}
