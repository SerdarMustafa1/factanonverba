using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class AdditionalUserProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AbpUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AbpUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AbpUsers");
        }
    }
}
