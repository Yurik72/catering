using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CateringPro.Migrations
{
    public partial class MassEmailSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.AddColumn<string>(
                name: "SQLCommand",
                table: "MassEmail",
                nullable: true);

        }
    }
}
