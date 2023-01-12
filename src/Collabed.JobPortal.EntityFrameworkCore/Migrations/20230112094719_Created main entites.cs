using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class Createdmainentites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserType",
                table: "AbpUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "AppJobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Skills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<int>(type: "int", nullable: true),
                    Industry = table.Column<int>(type: "int", nullable: true),
                    SalaryCurrency = table.Column<int>(type: "int", nullable: true),
                    SalaryFrom = table.Column<float>(type: "real", nullable: true),
                    SalaryTo = table.Column<float>(type: "real", nullable: true),
                    SalaryPeriod = table.Column<byte>(type: "tinyint", nullable: true),
                    SalaryBenefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExternal = table.Column<bool>(type: "bit", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppJobs_AppClients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "AppClients",
                        principalColumn: "Id");
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
                name: "IX_AppCandidateJobs_CandidateId_JobId",
                table: "AppCandidateJobs",
                columns: new[] { "CandidateId", "JobId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppCandidateJobs_JobId",
                table: "AppCandidateJobs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_ClientId",
                table: "AppJobs",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppCandidateJobs");

            migrationBuilder.DropTable(
                name: "AppCandidates");

            migrationBuilder.DropTable(
                name: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppClients");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "AbpUsers");
        }
    }
}
