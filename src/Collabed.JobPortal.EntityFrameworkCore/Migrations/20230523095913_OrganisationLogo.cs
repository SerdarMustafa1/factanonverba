using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class OrganisationLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LogoBlobName",
                table: "AppOrganisations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoContentType",
                table: "AppOrganisations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoFileName",
                table: "AppOrganisations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LogoBlobName",
                table: "AppOrganisations");

            migrationBuilder.DropColumn(
                name: "LogoContentType",
                table: "AppOrganisations");

            migrationBuilder.DropColumn(
                name: "LogoFileName",
                table: "AppOrganisations");
        }
    }
}
