using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class Address_relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Docs",
                nullable: true);


            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Consignment",
                nullable: true);
   
            migrationBuilder.CreateIndex(
                name: "IX_Docs_AddressId",
                table: "Docs",
                column: "AddressId");
            migrationBuilder.CreateIndex(
                    name: "IX_Consignment_AddressId",
                    table: "Consignment",
                    column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consignment_Addresses_AddressId",
                table: "Consignment",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Docs_Addresses_AddressId",
                table: "Docs",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
          }
    }
}
