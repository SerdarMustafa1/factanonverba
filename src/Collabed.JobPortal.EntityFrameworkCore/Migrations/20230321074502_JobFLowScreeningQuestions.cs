using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace Collabed.JobPortal.Migrations
{
    public partial class JobFLowScreeningQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppJobCategories");

            migrationBuilder.DropTable(
                name: "AppJobSupplementalPays");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSupplementalPays",
                table: "AppSupplementalPays");

            migrationBuilder.RenameTable(
                name: "AppSupplementalPays",
                newName: "SupplementalPays");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "AppJobs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAcceptingApplications",
                table: "AppJobs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "SupplementalPay",
                table: "AppJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ApplicationDate",
                table: "AppJobApplicants",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ApplicationStatus",
                table: "AppJobApplicants",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "InterviewDate",
                table: "AppJobApplicants",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NotificationSent",
                table: "AppJobApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StatusChangePublished",
                table: "AppJobApplicants",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SupplementalPays",
                table: "SupplementalPays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppScreeningQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAutoRejectQuestion = table.Column<bool>(type: "bit", nullable: false),
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppScreeningQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppScreeningQuestions_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobs_CategoryId",
                table: "AppJobs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppScreeningQuestions_JobId",
                table: "AppScreeningQuestions",
                column: "JobId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppJobs_AppCategories_CategoryId",
                table: "AppJobs",
                column: "CategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppJobs_AppCategories_CategoryId",
                table: "AppJobs");

            migrationBuilder.DropTable(
                name: "AppScreeningQuestions");

            migrationBuilder.DropIndex(
                name: "IX_AppJobs_CategoryId",
                table: "AppJobs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SupplementalPays",
                table: "SupplementalPays");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "IsAcceptingApplications",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "SupplementalPay",
                table: "AppJobs");

            migrationBuilder.DropColumn(
                name: "ApplicationDate",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "ApplicationStatus",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "InterviewDate",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "NotificationSent",
                table: "AppJobApplicants");

            migrationBuilder.DropColumn(
                name: "StatusChangePublished",
                table: "AppJobApplicants");

            migrationBuilder.RenameTable(
                name: "SupplementalPays",
                newName: "AppSupplementalPays");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSupplementalPays",
                table: "AppSupplementalPays",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AppJobCategories",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobCategories", x => new { x.JobId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_AppJobCategories_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppJobSupplementalPays",
                columns: table => new
                {
                    JobId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SupplementalPayId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobSupplementalPays", x => new { x.JobId, x.SupplementalPayId });
                    table.ForeignKey(
                        name: "FK_AppJobSupplementalPays_AppJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "AppJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppJobSupplementalPays_AppSupplementalPays_SupplementalPayId",
                        column: x => x.SupplementalPayId,
                        principalTable: "AppSupplementalPays",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobCategories_JobId_CategoryId",
                table: "AppJobCategories",
                columns: new[] { "JobId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSupplementalPays_JobId_SupplementalPayId",
                table: "AppJobSupplementalPays",
                columns: new[] { "JobId", "SupplementalPayId" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobSupplementalPays_SupplementalPayId",
                table: "AppJobSupplementalPays",
                column: "SupplementalPayId");
        }
    }
}
