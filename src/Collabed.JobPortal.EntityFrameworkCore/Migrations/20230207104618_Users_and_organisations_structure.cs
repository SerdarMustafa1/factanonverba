using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class Users_and_organisations_structure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppClients_ClientId",
                table: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppCandidateJobs");

            migrationBuilder.DropTable(
                name: "AppClients");

            migrationBuilder.DropTable(
                name: "AppCandidates");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_ClientId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "AppJobs");

            migrationBuilder.AddColumn<Guid>(
                name: "OrganisationId",
                table: "AppJobs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AppJobApplicants",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobApplicants", x => new { x.JobId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AppJobApplicants_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobApplicants_AppJobs_UserId",
                        column: x => x.UserId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganisations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganisations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppOrganisationMembers",
                columns: table => new
                {
                    OrganisationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppOrganisationMembers", x => new { x.OrganisationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_AppOrganisationMembers_AbpUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AbpUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppOrganisationMembers_AppOrganisations_UserId",
                        column: x => x.UserId,
                        principalTable: "AppOrganisations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_OrganisationId",
                table: "AppJobs",
                column: "OrganisationId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobApplicants_JobId_UserId",
                table: "AppJobApplicants",
                columns: new[] { "JobId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobApplicants_UserId",
                table: "AppJobApplicants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganisationMembers_OrganisationId_UserId",
                table: "AppOrganisationMembers",
                columns: new[] { "OrganisationId", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppOrganisationMembers_UserId",
                table: "AppOrganisationMembers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppOrganisations_OrganisationId",
                table: "AppJobs",
                column: "OrganisationId",
                principalTable: "AppOrganisations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppOrganisations_OrganisationId",
                table: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppJobApplicants");

            migrationBuilder.DropTable(
                name: "AppOrganisationMembers");

            migrationBuilder.DropTable(
                name: "AppOrganisations");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_OrganisationId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "OrganisationId",
                table: "AppJobs");

            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "AppJobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppCandidates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCandidates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCandidateJobs",
                columns: table => new
                {
                    CandidateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCandidateJobs", x => new { x.CandidateId, x.JobId });
                    table.ForeignKey(
                        name: "FK_AppCandidateJobs_AppCandidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "AppCandidates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppCandidateJobs_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_ClientId",
                table: "AppJobs",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCandidateJobs_CandidateId_JobId",
                table: "AppCandidateJobs",
                columns: new[] { "CandidateId", "JobId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCandidateJobs_JobId",
                table: "AppCandidateJobs",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppClients_ClientId",
                table: "AppJobs",
                column: "ClientId",
                principalTable: "AppClients",
                principalColumn: "Id");
        }
    }
}
