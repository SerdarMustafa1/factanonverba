using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class AdjustJobStructure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppLocations_LocationId",
                table: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppJobLanguages");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_LocationId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ContactEmail",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "DaysToAdvertise",
                table: "AppJobs");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "AppJobs",
                newName: "PositionsAvailable");

            migrationBuilder.RenameColumn(
                name: "ContactUrl",
                table: "AppJobs",
                newName: "SubDescription");

            migrationBuilder.RenameColumn(
                name: "ContactPhone",
                table: "AppJobs",
                newName: "StartDateText");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "AppJobs",
                newName: "OtherDocuments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartDate",
                table: "AppJobs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SalaryPeriod",
                table: "AppJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDeadline",
                table: "AppJobs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EmploymentType",
                table: "AppJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceLevel",
                table: "AppJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HiringMultipleCandidates",
                table: "AppJobs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocalLanguageRequired",
                table: "AppJobs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSalaryNegotiable",
                table: "AppJobs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JobLocation",
                table: "AppJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LocalLanguageId",
                table: "AppJobs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OfferVisaSponsorship",
                table: "AppJobs",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OfficeLocationId",
                table: "AppJobs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_LocalLanguageId",
                table: "AppJobs",
                column: "LocalLanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_OfficeLocationId",
                table: "AppJobs",
                column: "OfficeLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppLanguages_LocalLanguageId",
                table: "AppJobs",
                column: "LocalLanguageId",
                principalTable: "AppLanguages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppLocations_OfficeLocationId",
                table: "AppJobs",
                column: "OfficeLocationId",
                principalTable: "AppLocations",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppLanguages_LocalLanguageId",
                table: "AppJobs");

            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppLocations_OfficeLocationId",
                table: "AppJobs");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_LocalLanguageId",
                table: "AppJobs");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_OfficeLocationId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ApplicationDeadline",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "HiringMultipleCandidates",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "IsLocalLanguageRequired",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "IsSalaryNegotiable",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "JobLocation",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "LocalLanguageId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "OfferVisaSponsorship",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "OfficeLocationId",
                table: "AppJobs");

            migrationBuilder.RenameColumn(
                name: "SubDescription",
                table: "AppJobs",
                newName: "ContactUrl");

            migrationBuilder.RenameColumn(
                name: "StartDateText",
                table: "AppJobs",
                newName: "ContactPhone");

            migrationBuilder.RenameColumn(
                name: "PositionsAvailable",
                table: "AppJobs",
                newName: "LocationId");

            migrationBuilder.RenameColumn(
                name: "OtherDocuments",
                table: "AppJobs",
                newName: "ContactName");

            migrationBuilder.AlterColumn<string>(
                name: "StartDate",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "SalaryPeriod",
                table: "AppJobs",
                type: "tinyint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ContactEmail",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DaysToAdvertise",
                table: "AppJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppJobLanguages",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobLanguages", x => new { x.JobId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_AppJobLanguages_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobLanguages_AppLanguages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "AppLanguages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_LocationId",
                table: "AppJobs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobLanguages_JobId_LanguageId",
                table: "AppJobLanguages",
                columns: new[] { "JobId", "LanguageId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobLanguages_LanguageId",
                table: "AppJobLanguages",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppLocations_LocationId",
                table: "AppJobs",
                column: "LocationId",
                principalTable: "AppLocations",
                principalColumn: "Id");
        }
    }
}
