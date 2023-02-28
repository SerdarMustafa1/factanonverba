using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class NullableOrganisationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppOrganisations_OrganisationId",
                table: "AppJobs");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "AppJobs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppOrganisations_OrganisationId",
                table: "AppJobs",
                column: "OrganisationId",
                principalTable: "AppOrganisations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppOrganisations_OrganisationId",
                table: "AppJobs");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrganisationId",
                table: "AppJobs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppOrganisations_OrganisationId",
                table: "AppJobs",
                column: "OrganisationId",
                principalTable: "AppOrganisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
