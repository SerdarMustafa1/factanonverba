using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class FixConstraintsOrganisationMembers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganisationMembers_AppOrganisations_UserId",
                table: "AppOrganisationMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganisationMembers_AppOrganisations_OrganisationId",
                table: "AppOrganisationMembers",
                column: "OrganisationId",
                principalTable: "AppOrganisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrganisationMembers_AppOrganisations_OrganisationId",
                table: "AppOrganisationMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrganisationMembers_AppOrganisations_UserId",
                table: "AppOrganisationMembers",
                column: "UserId",
                principalTable: "AppOrganisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
